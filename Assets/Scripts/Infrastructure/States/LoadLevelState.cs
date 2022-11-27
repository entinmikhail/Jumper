using System;
using Zenject;

namespace GameModels.StateMachine
{
    public class LoadLevelState : IState
    {
        private const string Main = "Main";
        
        [Inject] private ISceneLoader _sceneLoader;
        public void Enter()
        {
            _sceneLoader.Load(Main);
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}