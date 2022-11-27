using Server;
using UnityEngine;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IBootstrapState : IState
    {
    }

    public class BootstrapState : IBootstrapState
    {
        private const string Initial = "Initial";
        
        [Inject] private IFakeServer _fakeServer;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IGameStateMachine _gameStateMachine;

        public void Enter()
        {
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
            _fakeServer.InitializeConnection();
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.Enter<LoadLevelState>();
        }

        public void Exit()
        {
            
        }
    }
}