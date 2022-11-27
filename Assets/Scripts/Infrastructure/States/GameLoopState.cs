using Server;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IGameLoopState : IState
    {
    }
    public class GameLoopState : IGameLoopState
    {
        [Inject] private IGameModel _gameModel;
        [Inject] private IFakeServer _fakeServer;

        public void Exit()
        {
            
        }

        public void Enter()
        {
            var result = _fakeServer.GetInitialState();
            _gameModel.Initialize(result);
        }
    }
}