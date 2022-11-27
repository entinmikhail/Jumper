using Server;
using Zenject;

namespace GameModels.StateMachine
{
    public interface ILoadLevelState : IPayloadedState<string> { }

    public class LoadLevelState : ILoadLevelState
    {
        private const string Main = "Main";
        
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IGameStateMachine _gameStateMachine;
        
        public void Enter(string name)
        {
            _sceneLoader.Load(Main, OnLoaded);
        }

        private void OnLoaded()
        {
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }
    }
}