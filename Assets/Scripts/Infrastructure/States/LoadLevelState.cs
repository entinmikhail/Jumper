using Server;
using Zenject;

namespace GameModels.StateMachine
{
    public interface ILoadLevelState : IPayloadedState<string> { }

    public class LoadLevelState : ILoadLevelState
    {
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IGameStateMachine _gameStateMachine;
        
        public void Enter(string name)
        {
            _sceneLoader.Load(name, OnLoaded);
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