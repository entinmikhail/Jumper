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
        [Inject] private IServerApi _serverApi;
        [Inject] private IServer _fakeServer;

        public void Exit()
        {
            
        }

        public void Enter()
        {
            var result = _fakeServer.GetInitialState();
            
            _serverApi.GetState();
            _gameModel.Initialize(result);
        }
    }
}