using System;
using Server;
using Zenject;

namespace GameModels
{
    public interface IGameModel
    {
        event Action<GameState> GameStateChanged;
        void Initialize(InitialStateResponse initialStateResponse);
    }

    public class GameModel : IGameModel
    {
        public event Action<GameState> GameStateChanged;
        public event Action<int> Jumped;
        public event Action Lose;
        public event Action<float> Win;
        
        public GameState GameState;
        public float BetAmount { get; set; }
        public string Currency { get; set; }
        public bool IsWin { get; set; }
        public bool IsWithBonus{ get; set; }
        public Step LastStep { get; set; }
        public int CurrentAltitude { get; set; }
        public float CurrentCoefficient { get; set; }
        public float WinAmount { get; set; }

        
        [Inject] private IFakeServer _fakeServer;
        
        public void Initialize(InitialStateResponse initialStateResponse)
        {
            if (initialStateResponse == null)
            {
                SetGameState(GameState.PrepareGameState);
                return;
            }

            ContinueGame(initialStateResponse);
        }

        public void FirstJump(string currency, float betAmount, bool isWithBonus)
        {
            if (GameState == GameState.ContinueGame)
            {
                Jump();
                return;
            }

            JumpResponse response = _fakeServer.FirstJump(new FirstJumpRequest()
            {
                Currency = currency,
                BetAmount = betAmount,
                IsWithBonus = isWithBonus
            });
            SetGameState(GameState.Gameplay);
            RefreshData(response);
        }

        public void Jump()
        {
            var response = _fakeServer.Jump();
            RefreshData(response);
        }

        public void CashOut()
        {
            var response = _fakeServer.CashOut();
            RefreshData(response);
        }

        private void RefreshData(JumpResponse jumpResponse)
        {
            if (!IsWin)
            {
                OnLose();
                return;
            }
            
            BetAmount = jumpResponse.BetAmount;
            Currency = jumpResponse.Currency;
            IsWin = jumpResponse.IsWin;
            IsWithBonus = jumpResponse.IsWithBonus;
            
            if (IsWithBonus)
                SetGameState(GameState.Bonus);
            
            foreach (var step in jumpResponse.Steps)
                JumpToPlatform(step);
        }

        private void RefreshData(InitialStateResponse initialStateResponse)
        {
            BetAmount = initialStateResponse.BetAmount;
            Currency = initialStateResponse.Currency;
            IsWin = initialStateResponse.IsWin;
            IsWithBonus = initialStateResponse.IsWithBonus;
            
            if (IsWithBonus)
                SetGameState(GameState.Bonus);

            foreach (var step in initialStateResponse.Steps)
                JumpToPlatform(step);
        }

        private void JumpToPlatform(Step step)
        {
            CurrentCoefficient = step.Coefficient;
            CurrentAltitude = step.Altitude;
            LastStep = step;
            
            Jumped?.Invoke(CurrentAltitude);
        }

        private void RefreshData(CashOutResponse cashOutResponse)
        {
            BetAmount = cashOutResponse.BetAmount;
            Currency = cashOutResponse.Currency;
            IsWin = cashOutResponse.IsWin;
            WinAmount = cashOutResponse.WinAmount;
            Win?.Invoke(WinAmount);
            SetGameState(GameState.Win);

        }
        
        private void OnLose()
        {
            Lose?.Invoke();
            SetGameState(GameState.Lose);
        }

        private void ContinueGame(InitialStateResponse initialStateResponse)
        {
            RefreshData(initialStateResponse);
            SetGameState(GameState.ContinueGame);
        }

        private void SetGameState(GameState newGameState)
        {
            
            GameState = newGameState;
            GameStateChanged?.Invoke(newGameState);
        }
    }

    public enum GameState
    {
        PrepareGameState,
        ContinueGame,
        Gameplay,
        Lose,
        Win,
        Bonus
    }
}