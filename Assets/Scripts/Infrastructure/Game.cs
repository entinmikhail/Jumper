using GameModels.StateMachine;
using Zenject;

public interface IGame
{
    void Start();
}

public class Game : IGame
{
    [Inject] private IGameStateMachine _gameStateMachine;
    
    public void Start()
    {
        _gameStateMachine.Initialize();
        _gameStateMachine.Enter<BootstrapState>();
    }
}