using GameModels;
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
        private const string Vertical = "Vertical";
        private const string Horisontal = "Horisontal";

        [Inject] private IJumperServerApi _jumperServerApi;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IGameStateMachine _gameStateMachine;
        [Inject] private IAccountModel _accountModel;

        public void Enter()
        {
            _accountModel.RefreshBalance(1000f, "USD");
            _jumperServerApi.AuthRequest( 
                () => _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel),
                null);
        }

        private void EnterLoadLevel()
        {
            // _gameStateMachine.Enter<LoadLevelState, string>(Payload);
            _gameStateMachine.Enter<LoadLevelState, string>(Horisontal);
        }

        public void Exit()
        {
            
        }
    }
}