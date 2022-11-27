using Zenject;

namespace GameModels.StateMachine
{
    public interface ILoadLevelState : IState
    {
    }

    public class LoadLevelState : ILoadLevelState
    {
        private const string Main = "Main";
        
        [Inject] private ISceneLoader _sceneLoader;
        
        public void Enter(string name)
        {
            _sceneLoader.Load(Main);
        }

        public void Exit()
        {
        }
    }
}