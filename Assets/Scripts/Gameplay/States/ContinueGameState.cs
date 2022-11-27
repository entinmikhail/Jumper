namespace GameModels.StateMachine
{
    public interface IContinueGameState : IState
    {
    }

    public class ContinueGameState : IContinueGameState
    {
        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}