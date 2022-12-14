using Character;
using Configs;
using GameModels;
using GameModels.StateMachine;
using Platforms;
using UIControllers;
using Zenject;

namespace Gameplay
{
    public interface IBonusGameState : IState
    {
    }

    public class BonusGameState : IBonusGameState
    {
        [Inject] private IAnimationDurationConfig _animationDurationConfig;
        [Inject] private IGameAnimatorController _gameAnimatorController;
        [Inject] private IGameLoopStateMachine _gameLoopStateMachine;
        [Inject] private IPlatformService _platformService;
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IGameModel _gameModel;

        public void Enter()
        {
            _gameAnimatorController.PlayBonusJump();
            
            for (int i = 1; i < _gameModel.CurrentAltitude + 3; i++)
                _platformService.TryAddPlatformObjectByData(i);
            
            _coroutineRunner.StartAfterDelay(_animationDurationConfig.AwaitingBonusAnimationTime, () =>
            {
                _characterMover.SetNumberPlatform(_gameModel.CurrentAltitude, _animationDurationConfig.BonusJumpAnimationTime);
                _gameAnimatorController.StartRotationAnimation(10, 50, _animationDurationConfig.BonusJumpAnimationTime);
            });

            _coroutineRunner.StartAfterDelay(_animationDurationConfig.BonusJumpAnimationTime
                                             + _animationDurationConfig.AwaitingBonusAnimationTime
                                             + _animationDurationConfig.IdleDelayAnimationTime 
                                             + _animationDurationConfig.WinAnimationTime, 
                () => 
                { 
                    _gameLoopStateMachine.Enter<StartGameState>(); 
                });
        }

        public void Exit()
        {
            _gameAnimatorController.PlayIdle();
            _gameAnimatorController.ResetAnimations();
        }
    }
}