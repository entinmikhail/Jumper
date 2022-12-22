using System;
using GameModels;
using GameModels.StateMachine;
using Gameplay;
using Zenject;

public interface IGameStateController
{
    void Initialize();
}

public class GameStateController : IGameStateController
{
    [Inject] private IGameLoopStateMachine _gameLoopStateMachine;
    [Inject] private IGameModel _gameModel;
    
    public void Initialize()
    {
        _gameModel.GameStateChanged += OnGameStateChanged;
        OnGameStateChanged(GameState.LoadingState);
    }

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.PrepareGameState: OnPrepare(); break;
            case GameState.ContinueGameAfterLogin: OnContinue(); break;
            case GameState.StartGameplay: OnStart(); break;
            case GameState.Lose: OnLose(); break;
            case GameState.Win: OnWin(); break;
            case GameState.Bonus: OnBonus(); break;
            case GameState.LoadingState: OnLoading(); break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
        }
    }

    private void OnBonus() => _gameLoopStateMachine.Enter<BonusGameState>();
    private void OnPrepare() => _gameLoopStateMachine.Enter<PrepareGameState>();
    private void OnStart() => _gameLoopStateMachine.Enter<MainGameState>();
    private void OnLose() => _gameLoopStateMachine.Enter<LoseGameState>();
    private void OnWin() => _gameLoopStateMachine.Enter<WinGameState>();
    private void OnContinue() => _gameLoopStateMachine.Enter<ContinueGameState>();
    private void OnLoading() => _gameLoopStateMachine.Enter<LoadingState>();
}