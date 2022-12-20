using Server;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IGameLoopState : IState
    {
    }
    public class GameLoopState : IGameLoopState
    {
        [Inject] private IJumperServerApi _jumperServerApi;

        public void Exit()
        {
            
        }

        public void Enter()
        {
            _jumperServerApi.GetState();
        }
    }
}