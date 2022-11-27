using System;
using Server;
using Zenject;

namespace GameModels
{
    public interface IGameModel
    {
        event Action<GameState> GameStateChanged;
        event Action<int> Jumped;
        event Action Lose;
        event Action<float> Win;
        float BetAmount { get; set; }
        string Currency { get; set; }
        bool IsWin { get; set; }
        bool IsWithBonus { get; set; }
        Step LastStep { get; set; }
        int CurrentAltitude { get; set; }
        float CurrentCoefficient { get; set; }
        float WinAmount { get; set; }
        GameState GameState { get; set; }
        void Initialize(InitialStateResponse initialStateResponse);
        void Jump();
        void CashOut();
        void SetGameState(GameState newGameState);
    }

    public class GameModel : IGameModel
    {
        public event Action<GameState> GameStateChanged;
        public event Action<int> Jumped;
        public event Action Lose;
        public event Action<float> Win;
        
        public GameState GameState { get; set; }
        public float BetAmount { get; set; }
        public string Currency { get; set; }
        public bool IsWin { get; set; }
        public bool IsWithBonus{ get; set; }
        public Step LastStep { get; set; }
        public int CurrentAltitude { get; set; }
        public float CurrentCoefficient { get; set; }
        public float WinAmount { get; set; }
        
        [Inject] private IFakeServer _fakeServer;
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
                
                SetGameState(GameState.StartGameplay);
                RefreshData(jumpResponse);
                return;
            }
            
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
            IsWin = jumpResponse.IsWin;

            if (!IsWin)
            {
                OnLose();
                return;
            }
            
            BetAmount = jumpResponse.BetAmount;
            Currency = jumpResponse.Currency;
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
            CurrentAltitude = 0;
            CurrentCoefficient = 0;
            Lose?.Invoke();
            SetGameState(GameState.Lose);
        }

        private void ContinueGame(InitialStateResponse initialStateResponse)
        {
            SetGameState(GameState.ContinueGameAfterLogin);
            RefreshData(initialStateResponse);
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