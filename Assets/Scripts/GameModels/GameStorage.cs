using System;
using System.Globalization;
using Configs;
using Server;
using UnityEngine;
using Zenject;

namespace GameModels
{
    public interface IGameStorage
    {
        float BetAmount { get; set; }
        string Currency { get; set; }
        bool IsWin { get; set; }
        bool IsWithBonus { get; set; }
        int CurrentAltitude { get; set; }
        float CurrentFactor{ get; set; }
        float WinAmount { get; set; }
        float PrevCoefficient { get; set; }
        
        void RefreshData(CashoutResponse cashOutResponse);
        void RefreshData(BonusBetResponse response);
        void RefreshData(JumpResponse jumpResponse);
        void RefreshData(GetStateResponse initialStateResponse);
        void RefreshData(BetResponse initialStateResponse);
        void SetBonusStart(bool value);
        void ResetData();
    }

    public class GameStorage : IGameStorage
    {
        public float BetAmount { get; set; }
        public string Currency { get; set; }
        public bool IsWin { get; set; }
        public bool IsWithBonus { get; set; }
        public int CurrentAltitude { get; set; }
        public float CurrentFactor  { get; set; }
        public float PrevCoefficient { get; set; } = 1;
        public float WinAmount { get; set; }

        [Inject] private IAccountModel _accountModel;
        [Inject] private IGameConfigs _gameConfigs;

        public void RefreshData(CashoutResponse cashOutResponse)
        {
            BetAmount = cashOutResponse.betAmount;
            Currency = cashOutResponse.currency;
            IsWin = cashOutResponse.isWin;
            WinAmount = cashOutResponse.winAmount;
            IsWithBonus = false;
            
            _accountModel.ChangeBalance(WinAmount);

        }

        public void RefreshData(JumpResponse jumpResponse)
        {
            IsWin = jumpResponse.isWin;
            BetAmount = jumpResponse.betAmount;
            Currency = jumpResponse.currency;
            IsWithBonus = jumpResponse.isWithBonus;
        }

        public void RefreshData(GetStateResponse initialStateResponse)
        {
            if (initialStateResponse == null)
            {
                Debug.LogError("initialStateResponse");
                return;
            }
            
            BetAmount = initialStateResponse.betAmount;
            Currency = initialStateResponse.currency;
            IsWin = initialStateResponse.isWin;
            IsWithBonus = initialStateResponse.isWithBonus;

            if (initialStateResponse.steps == null)
            {

                Debug.LogError("initialStateResponse.steps == null");
                return;
            }
            foreach (var step in initialStateResponse.steps)
            {
                PrevCoefficient = CurrentFactor;
                CurrentFactor = float.Parse(step.coefficient, CultureInfo.InvariantCulture.NumberFormat);
                CurrentAltitude = step.altitude;
            }
            _accountModel.ChangeBalance(-BetAmount);
        }
        
        public void RefreshData(BetResponse initialStateResponse)
        {
            BetAmount = initialStateResponse.betAmount;
            Currency = initialStateResponse.currency;
            IsWin = initialStateResponse.isWin;
            IsWithBonus = initialStateResponse.isWithBonus;
            
            foreach (var step in initialStateResponse.steps)
            {
                PrevCoefficient = CurrentFactor;
                CurrentFactor = float.Parse(step.coefficient, CultureInfo.InvariantCulture.NumberFormat);
                CurrentAltitude = step.altitude;
            }
        } 
        
        public void RefreshData(BonusBetResponse response)
        {
            IsWin = true;
            IsWithBonus = response.isWithBonus;
            PrevCoefficient = CurrentFactor;
            CurrentFactor = response.betMultiplayer;
            CurrentAltitude = response.step;
            CurrentAltitude = response.startPoint;
        }

        public void SetBonusStart(bool value)
        {
            IsWithBonus = value;
            _accountModel.ChangeBalance(-_gameConfigs.BonusPrice);
        }

        public void ResetData()
        {
            CurrentAltitude = 0;
            PrevCoefficient = CurrentFactor;
            CurrentFactor = _gameConfigs.DefaultFactor;
            IsWithBonus = false;
        }
    }
}