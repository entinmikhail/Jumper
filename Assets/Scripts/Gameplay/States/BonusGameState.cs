using GameModels.StateMachine;

namespace Gameplay
{
    public interface IBonusGameState : IState
    {
    }

    public class BonusGameState : IBonusGameState
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