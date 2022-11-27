namespace GameModels.StateMachine
{
    public interface IStartGameState : IState
    {
    }

    public class StartGameState : IStartGameState
    {
        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}