using Character;
using Configs;
using GameModels;
using GameModels.StateMachine;
using Platforms;
using UIControllers;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public interface IBonusGameState : IState { }
    public class BonusGameState : IBonusGameState
    {
        [Inject] private IAnimationDurationConfig _animationDurationConfig;
        [Inject] private IGameAnimatorController _gameAnimatorController;
        [Inject] private IGameLoopStateMachine _gameLoopStateMachine;
        [Inject] private IPlatformService _platformService;
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameHandler _gameHandler;
        
        public void Enter()
        {
            _gameHandler.Jumped += OnJumped;
            _characterMover.MoveEnd += OnMoveEnd;
        }

        private void OnMoveEnd()
        {
            _characterMover.CharacterController.SetParticle(false);
        }

        private void OnJumped(int arg1, string arg2)
        {
            _gameAnimatorController.PlayBonusJump();
            for (int i = 1; i < _gameStorage.CurrentAltitude + 4; i++)
                _platformService.TryAddPlatformObjectByData(i);
            
            _characterMover.RotateCharacter(_gameStorage.CurrentAltitude + 1);
            
            _coroutineRunner.StartAfterDelay(_animationDurationConfig.AwaitingBonusAnimationTime, () =>
            {
                _characterMover.SetNumberPlatform(_gameStorage.CurrentAltitude, _animationDurationConfig.BonusJumpAnimationTime);
                _characterMover.CharacterController.SetParticle(true);
                _gameAnimatorController.StartRotationAnimation(
                    _gameStorage.CurrentFactor, _animationDurationConfig.BonusJumpAnimationTime, 
                    _gameStorage.PrevCoefficient);
            });

            _coroutineRunner.StartAfterDelay(_animationDurationConfig.BonusJumpAnimationTime
                                             + _animationDurationConfig.AwaitingBonusAnimationTime
                                             + _animationDurationConfig.IdleDelayAnimationTime 
                                             + _animationDurationConfig.WinAnimationTime, 
                () => 
                {
                    _gameLoopStateMachine.Enter<MainGameState>(); 
                });
        }

        public void Exit()
        {
            _gameHandler.Jumped -= OnJumped;

            _gameAnimatorController.PlayIdle();
            _gameAnimatorController.ResetAnimations();
        }
    }
}