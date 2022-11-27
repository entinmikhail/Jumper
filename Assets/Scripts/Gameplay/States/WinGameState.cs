using GameModels.StateMachine;

namespace Gameplay
{
    public interface IWinGameState : IState
    {
    }

    public class WinGameState : IWinGameState
    {
        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void Enter()
        {
            throw new System.NotImplementedException();
        }
    }
}