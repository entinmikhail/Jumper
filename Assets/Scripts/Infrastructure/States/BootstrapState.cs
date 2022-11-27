using GameModels.StateMachine;
using Server;
using Zenject;

namespace Infrastructure.States
{
    public interface IBootstrapState : IState
    {
    }

    public class BootstrapState : IBootstrapState
    {
        private const string Initial = "Initial";
        private const string Payload = "Main";

        [Inject] private IFakeServer _fakeServer;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IGameStateMachine _gameStateMachine;

        public void Enter()
        {
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _fakeServer.InitializeConnection();
            _gameStateMachine.Enter<LoadLevelState, string>(Payload);
        }

        public void Exit()
        {
            
        }
    }
}