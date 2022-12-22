using GameModels;
using GameModels.StateMachine;
using Server;
using UnityEngine;
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
            _jumperServerApi.AuthRequest(() => _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel));
        }

        private void EnterLoadLevel()
        {
            Debug.LogError("Screen.width = " + Screen.width);
            Debug.LogError("Screen.height = " + Screen.height);
            var screenFactor = Screen.width / Screen.height;
            _gameStateMachine.Enter<LoadLevelState, string>(screenFactor >= 1 ? Horisontal : Vertical);
        }

        public void Exit()
        {
            
        }
    }
}