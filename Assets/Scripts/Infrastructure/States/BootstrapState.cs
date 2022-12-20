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

        [Inject] private IServer _fakeServer;
        [Inject] private IServerApi _serverApi;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IGameStateMachine _gameStateMachine;

        public void Enter()
        {
            _serverApi.AuthRequest(new AuthRequest("3bdda719-8b47-4e19-9282-4ea1df4b1da5",
                "be67dc74323f3f1142f6152aa3ff0d32",
                "USD"), 
                () => _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel),
                null);
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