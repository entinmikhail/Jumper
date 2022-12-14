using Character;
using Configs;
using Platforms;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IContinueGameState : IState
    {
    }

    public class ContinueGameState : IContinueGameState
    {
        [Inject] private IGameModel _gameModel;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IPlatformService _platformService;
        [Inject] private IAnimationDurationConfig _animationDurationConfig;

        public void Enter()
        {
            _gameModel.Jumped += OnJumped;
            
            for (int i = 1; i < _gameModel.CurrentAltitude + 3; i++)
                _platformService.TryAddPlatformObjectByData(i);
            
            _characterMover.SetNumberPlatformWithoutAnimation(_gameModel.CurrentAltitude);
            // _characterMover.RefreshCharacter();
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