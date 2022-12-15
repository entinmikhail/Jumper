using System;
using System.Collections.Generic;
using Character;
using Platforms;
using Server;
using UnityEngine;
using Zenject;

namespace GameModels
{
    public interface IGameModel
    {
        void Initialize(InitialStateResponse initialStateResponse);
        void Jump();
        void CashOut();
        void SetGameState(GameState newGameState);
        void BuyBonusJump();
        
        event Action<GameState> GameStateChanged;
        event Action<int> Jumped;
        event Action<int, BonusType> BonusPiked;
        
        float BetAmount { get; set; }
        string Currency { get; set; }
        bool IsWin { get; set; }
        bool IsWithBonus { get; set; }
        Step LastStep { get; set; }
        int CurrentAltitude { get; set; }
        float CurrentCoefficient { get; set; }
        float WinAmount { get; set; }
        GameState GameState { get; set; }
        Dictionary<int, BonusType> BonusTypes { get; }
    }

    public class GameModel : IGameModel
    {
        public event Action<GameState> GameStateChanged;
        public event Action<int> Jumped;
        public event Action<int, BonusType> BonusPiked;
        public GameState GameState { get; set; }
        public float BetAmount { get; set; }
        public string Currency { get; set; }
        public bool IsWin { get; set; }
        public bool IsWithBonus{ get; set; }
        public Step LastStep { get; set; }
        public int CurrentAltitude { get; set; }
        public float CurrentCoefficient { get; set; }
        public float WinAmount { get; set; }

        public Dictionary<int, BonusType> BonusTypes { get; } = new();

        [Inject] private IServer _fakeServer;
        [Inject] private IAccountModel _accountModel;
        
        public void Initialize(InitialStateResponse initialStateResponse)
        {
            if (initialStateResponse == null)
            {
                SetGameState(GameState.PrepareGameState);
                return;
            }

            ContinueGame(initialStateResponse);
            _fakeServer.BalanceChanged += OnBalanceChanged;
        }
        
        private void OnBalanceChanged(float newBalance)
        {
            _accountModel.CurrentBalance = newBalance;
        }

        public void Jump()
        {
            if (GameState == GameState.PrepareGameState)
            {
                JumpResponse jumpResponse = _fakeServer.FirstJump(new FirstJumpRequest()
                {
                    Currency = Currency,
                    BetAmount = BetAmount,
                    IsWithBonus = IsWithBonus
                });
                

                if (IsWithBonus)
                {
                    RefreshData(jumpResponse); 
                    SetGameState(GameState.Bonus);
                }
                else
                {
                    SetGameState(GameState.StartGameplay);
                    RefreshData(jumpResponse);
                }

                return;
            }
            
            var response = _fakeServer.Jump();
            RefreshData(response);
        }

        public void BuyBonusJump()
        {
            IsWithBonus = true;
            _accountModel.CurrentBalance -= 100;
        }

        public void CashOut()
        {
            var response = _fakeServer.CashOut();
            RefreshData(response);
        }

        private void RefreshData(JumpResponse jumpResponse)
        {
            IsWin = jumpResponse.IsWin;

            if (!IsWin)
            {
                OnLose();
                return;
            }
            
            BetAmount = jumpResponse.BetAmount;
            Currency = jumpResponse.Currency;
            IsWithBonus = jumpResponse.IsWithBonusNextWithBonus;

            foreach (var step in jumpResponse.Steps)
                JumpToPlatform(step);
        }

        private void RefreshData(InitialStateResponse initialStateResponse)
        {
            BetAmount = initialStateResponse.BetAmount;
            Currency = initialStateResponse.Currency;
            IsWin = initialStateResponse.IsWin;
            IsWithBonus = initialStateResponse.IsWithBonus;
            
            foreach (var step in initialStateResponse.Steps)
            {
                CurrentCoefficient = step.Coefficient;
                CurrentAltitude = step.Altitude;
                LastStep = step;
            }
            
            // if (IsWithBonus)
            //     SetGameState(GameState.Bonus);
        }

        private void JumpToPlatform(Step step)
        {
            CurrentCoefficient = step.Coefficient;
            CurrentAltitude = step.Altitude;

            if (BonusTypes.ContainsKey(step.Altitude))
                BonusTypes[step.Altitude+1] = step.Box;
            else
                BonusTypes.Add(step.Altitude+1, step.Box);

            if (BonusTypes.TryGetValue(step.Altitude, out var bonusType))
            {
                if (bonusType is BonusType.ExtraJump or BonusType.ExtraFactor)
                {
                    Debug.LogError(bonusType.ToString());
                    BonusPiked?.Invoke(step.Altitude, bonusType);
                }
            }
            
            LastStep = step;

            Jumped?.Invoke(CurrentAltitude);
        }

        private void RefreshData(CashOutResponse cashOutResponse)
        {
            BetAmount = cashOutResponse.BetAmount;
            Currency = cashOutResponse.Currency;
            IsWin = cashOutResponse.IsWin;
            WinAmount = cashOutResponse.WinAmount;
            IsWithBonus = false;
            SetGameState(GameState.Win);
        }
        
        private void OnLose()
        {
            CurrentAltitude = 0;
            CurrentCoefficient = 0;
            IsWithBonus = false;

            BonusTypes.Clear();
            SetGameState(GameState.Lose);
        }

        private void ContinueGame(InitialStateResponse initialStateResponse)
        {
            RefreshData(initialStateResponse);
            
            SetGameState(GameState.ContinueGameAfterLogin);
        }

        public void SetGameState(GameState newGameState)
        {
            GameState = newGameState;
            GameStateChanged?.Invoke(newGameState);
        }
    }

    public enum GameState
    {
        PrepareGameState,
        ContinueGameAfterLogin,
        StartGameplay,
        Lose,
        Win,
        Bonus
    }
}