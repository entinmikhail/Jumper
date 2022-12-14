using Character;
using Configs;
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
        [Inject] private IAnimationDurationConfig _animationDurationConfig;

        public void Enter()
        {
            _gameModel.Jumped += OnJumped;
            _characterMover.SetActive(true);
        }

        private void OnJumped(int index)
        {
            _platformService.TryAddPlatformObjectByData(index + 2);
            _characterMover.SetNumberPlatform(index, _animationDurationConfig.DefaultJumpAnimationTime);
        }

        public void Exit()
        {
            _gameModel.Jumped -= OnJumped;
        }
    }
}