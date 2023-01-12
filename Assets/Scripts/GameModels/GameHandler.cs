using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Configs;
using Server;
using UnityEngine;
using Zenject;

namespace GameModels
{
    public interface IJumpInvoker
    {
        event Action<int, string> Jumped;
    }
    
    public interface IGameHandler : IJumpInvoker
    {
        void InitializeHandle(GetStateResponse initialStateResponse);
        void JumpHandle(JumpResponse jumpResponse);
        void CashOutHandle(CashoutResponse cashoutResponse);
        void BetHandle(BetResponse response);
        void BonusBetHandle(BonusBetResponse response);
        void OnBetsRanges(BetsRangesData data);
        void OnCurrencies(Dictionary<string, object> data);
        void OnBalanceChanged(OnBalanceChangeData data);
    }

    public class GameHandler : IGameHandler
    {
        public event Action<int, string> Jumped;
        
        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameConfigs _gameConfigs;
        [Inject] private IAccountModel _accountModel;
        [Inject] private IGameModel _gameModel;

        public void BonusBetHandle(BonusBetResponse response)
        {
            if (_gameModel.GameState == GameState.PrepareGameState)
                _gameModel.SetGameState(GameState.Bonus);
            
            _gameStorage.RefreshData(response);

            Jumped?.Invoke(_gameStorage.CurrentAltitude, null);
        }

        public void OnBetsRanges(BetsRangesData data)
        {
            _gameConfigs.MaxBet = data.MaxBet;
            _gameConfigs.MinBet = data.MinBet;
        }

        public void OnCurrencies(Dictionary<string, object> data)
        {
            _gameConfigs.SetCurrencyFactor(data);
        }

        public void OnBalanceChanged(OnBalanceChangeData data)
        {
            _accountModel.RefreshBalance(data.balance, data.currency);
        }
        
        public void InitializeHandle(GetStateResponse initialStateResponse)
        {
            if (initialStateResponse == null 
                || initialStateResponse.steps?.First().altitude == 0 
                || initialStateResponse.steps == null)
            {
                _gameModel.SetGameState(GameState.PrepareGameState);
                _gameConfigs.BonusFactor = initialStateResponse?.bonusBuyK ?? 10;
                return;
            }

            ContinueGameHandle(initialStateResponse);
        }

        public void JumpHandle(JumpResponse jumpResponse)
        {
            _gameStorage.RefreshData(jumpResponse);

            if (jumpResponse.steps != null)
            {
                foreach (var step in jumpResponse.steps)
                    JumpToPlatform(step);
            }

            if (!jumpResponse.isWin)
            {
                _gameStorage.ResetData();
                _gameModel.SetGameState(GameState.Lose);
            }
        }

        public void BetHandle(BetResponse response)
        {
            if (_gameModel.GameState == GameState.PrepareGameState)
                _gameModel.SetGameState(_gameStorage.IsWithBonus ? GameState.Bonus : GameState.StartGameplay);
            
            _gameStorage.RefreshData(response);
            
            if (response.steps != null)
            {
                foreach (var step in response.steps)
                    JumpToPlatform(step);
            }
            
            if (response.isWin)
                return;
            _gameStorage.ResetData();
            _gameModel.SetGameState(GameState.Lose);

        }


        private void ContinueGameHandle(GetStateResponse initialStateResponse)
        {
            _gameStorage.RefreshData(initialStateResponse);
            _gameModel.SetGameState(GameState.ContinueGameAfterLogin);
        }

        public void CashOutHandle(CashoutResponse cashoutResponse)
        {
            _gameStorage.RefreshData(cashoutResponse);
            _gameModel.SetGameState(GameState.Win);
        }

        private void JumpToPlatform(Step step)
        {
            _gameStorage.PrevCoefficient = _gameStorage.CurrentFactor;
            _gameStorage.CurrentFactor = float.Parse(step.coefficient, CultureInfo.InvariantCulture.NumberFormat);
            _gameStorage.CurrentAltitude = step.altitude;
            
            Jumped?.Invoke(_gameStorage.CurrentAltitude, step.box);
        }
    }

    public enum GameState
    {
        PrepareGameState,
        LoadingState,
        ContinueGameAfterLogin,
        StartGameplay,
        Lose,
        Win,
        Bonus
    }
}