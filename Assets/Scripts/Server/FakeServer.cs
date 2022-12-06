using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Server
{
    public interface IFakeServer
    {
        event Action<float> BalanceChanged;
        void InitializeConnection();
        InitialStateResponse GetInitialState();
        JumpResponse FirstJump(FirstJumpRequest firstJumpRequest);
        CashOutResponse CashOut();
        JumpResponse Jump();
    }

    public class FakeServer : MonoBehaviour, IFakeServer
    {
        private InitialStateResponse _gameState = null;
        private int _altitude = 0;
        private float _coefficient = 0f;
        private float _totalCash = 1000f;
        private bool _isInitialize;

        public event Action<float> BalanceChanged;
        public void InitializeConnection()
        {
            _isInitialize = true;
        }
        
        public InitialStateResponse GetInitialState()
        {
            if (!_isInitialize)
            {
                Debug.LogError("Not initialize");
                return null;
            }
            
            _altitude = 0;
            var isWin = Random.Range(0, 100) > 75;
             isWin = true;
             
            if (isWin)
            {
                _altitude = Random.Range(1, 20);
                _coefficient = Random.Range(1, 20);
            }
        
            _gameState = new InitialStateResponse()
            {
                BetAmount = Random.Range(1f, 14),
                Currency = "RUB",
                IsWin = isWin,
                IsWithBonus = isWin && Random.Range(0, 100) > 50,
                Steps = new []{new Step()
                {
                    Altitude = _altitude,
                    Coefficient = _coefficient
                }}
            };
        
            // return _gameState;
            return null;
        }

        public JumpResponse FirstJump(FirstJumpRequest firstJumpRequest)
        {
            if (!_isInitialize)
            {
                Debug.LogError("Not initialize");
                return null;
            }
            
            _gameState.BetAmount = firstJumpRequest.BetAmount;
            _gameState.Currency = firstJumpRequest.Currency;
            _gameState.IsWithBonus = firstJumpRequest.IsWithBonus;
        
            var isWin = Random.Range(0, 100) < 80;
            isWin = true;

            if (isWin)
                _altitude = 1;
            
            _coefficient += 0.2f;
            return new JumpResponse()
            {
                Currency = _gameState.Currency,
                BetAmount = _gameState.BetAmount,
                IsWin = isWin,
                IsWithBonus = _gameState.IsWithBonus,
                Steps = isWin
                    ? new []{new Step
                    {
                        Altitude = _altitude,
                        Coefficient = _coefficient
                    }}
                    : null
            };
        }

        public CashOutResponse CashOut()
        {
            if (!_isInitialize)
            {
                Debug.LogError("Not initialize");
                return null;
            }

            _altitude = 0;
            _totalCash += _coefficient * _gameState.BetAmount;
            _coefficient = 0;
            BalanceChanged?.Invoke(_totalCash);
            return new CashOutResponse()
            {
                Currency = _gameState.Currency,
                IsWin = true,
                BetAmount = _gameState.BetAmount,
                WinAmount = _coefficient * _gameState.BetAmount
            };
        }

        public JumpResponse Jump()
        {
            if (!_isInitialize)
            {
                Debug.LogError("Not initialize");
                return null;
            }
            
            var isWin = Random.Range(0, 100) < 80;
            // isWin = true;

            if (isWin)
                _altitude++;
            else
            {
                _coefficient = 0;
                _totalCash -= _gameState.BetAmount;
                BalanceChanged?.Invoke(_totalCash);
            }

            _coefficient += Random.Range(0.1f, 0.3f);

            if (isWin && Random.Range(0, 100) < 20)
            {
                _altitude++;
                var bonusFactor = Random.Range(0.1f, 0.3f);
                _coefficient += bonusFactor;
                return new JumpResponse()
                {
                    Currency = _gameState.Currency,
                    BetAmount = _gameState.BetAmount,
                    IsWin = isWin,
                    IsWithBonus = _gameState.IsWithBonus,
                    
                    Steps = new []
                    { 
                            new Step { Altitude = _altitude - 1, Coefficient = _coefficient - bonusFactor, Box = "PLUS1" },
                            new Step { Altitude = _altitude, Coefficient = _coefficient }
                    }
                };
            }

            if (isWin && Random.Range(0, 100) < 20)
            {
                _coefficient *= 2;
                
                return new JumpResponse()
                {
                    Currency = _gameState.Currency,
                    BetAmount = _gameState.BetAmount,
                    IsWin = isWin,
                    IsWithBonus = _gameState.IsWithBonus,
                    Steps = isWin
                        ? new []{ new Step
                        {
                            Altitude = _altitude,
                            Coefficient = _coefficient,
                            Box = "X2" 
                        }} 
                        : null
                };
            }
            
            return new JumpResponse()
            {
                Currency = _gameState.Currency,
                BetAmount = _gameState.BetAmount,
                IsWin = isWin,
                IsWithBonus = _gameState.IsWithBonus,
                Steps = isWin
                    ? new []{ new Step
                    {
                        Altitude = _altitude,
                        Coefficient = _coefficient
                    }} 
                    : null
            };
        }
    }

    public record InitialStateResponse
    {
        public float BetAmount;
        public string Currency;
        public bool IsWin;
        public bool IsWithBonus;
        public Step[] Steps;
    }

    public record FirstJumpRequest
    {
        public float BetAmount;
        public string Currency;
        public bool IsWithBonus;
    }

    public record JumpResponse
    {
        public float BetAmount;
        public string Currency;
        public bool IsWin;
        public bool IsWithBonus;
        public Step[] Steps;
    }
    public record CashOutResponse
    {
        public float BetAmount;
        public string Currency;
        public float WinAmount;
        public bool IsWin;
    }

    public class Step
    {
        public int Altitude;
        public float Coefficient;
        public string Box;
    }
}