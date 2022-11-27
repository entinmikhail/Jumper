using GameModels.StateMachine;

namespace Gameplay
{
    public interface ILoseGameState : IState
    {
    }

    public class LoseGameState : ILoseGameState
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