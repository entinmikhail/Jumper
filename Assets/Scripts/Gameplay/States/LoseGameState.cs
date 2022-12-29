using Character;
using Configs;
using GameModels.StateMachine;
using UIControllers;
using Zenject;

namespace Gameplay
{
    public interface ILoseGameState : IState
    {
    }

    public class LoseGameState : ILoseGameState
    {
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IAnimationDurationConfig _animationDurationConfig;
        [Inject] private IButtonsLockService _buttonsLockService;

        public void Enter()
        {
            _characterMover.MoveToNextPlatform(_animationDurationConfig.DefaultJumpAnimationTime);
            _buttonsLockService.LockAllButtons();
        }

        public void Exit()
        {

        }
    }
}