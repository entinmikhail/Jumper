using Character;
using Platforms;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IStartGameState : IState
    {
    }

    public class StartGameState : IStartGameState
    {
        [Inject] private IGameModel _gameModel;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IPlatformService _platformService;
        
        public void Enter()
        {
            _gameModel.Jumped += OnJumped;
        }

        private void OnJumped(int index)
        {
            _platformService.TryAddPlatformObjectByData(index + 2);
            _characterMover.SetNumberPlatform(index);
        }

        public void Exit()
        {
            _gameModel.Jumped -= OnJumped;
        }
    }
}