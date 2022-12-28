namespace GameModels.StateMachine
{
    public interface ILoadingState : IState
    {
    }
    
    public class LoadingState : ILoadingState
    {
        public void Exit()
        {
        }

        public void Enter()
        {

        }
    }
}