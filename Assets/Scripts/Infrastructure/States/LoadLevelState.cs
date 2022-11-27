using Server;
using Zenject;

namespace GameModels.StateMachine
{
    public interface ILoadLevelState : IPayloadedState<string> { }

    public class LoadLevelState : ILoadLevelState
    {
        private const string Main = "Main";
        
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IFakeServer _fakeServer;
        [Inject] private IGameStateMachine _gameStateMachine;
        [Inject] private IGameModel _gameModel;
        
        public void Enter(string name)
        {
            _sceneLoader.Load(Main, OnLoaded);
        }

        private void OnLoaded()
        {
            var result = _fakeServer.GetInitialState();
            _gameModel.Initialize(result);
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }
    }
}