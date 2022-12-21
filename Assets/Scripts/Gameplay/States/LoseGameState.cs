using Character;
using Configs;
using GameModels.StateMachine;
using Platforms;
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

        public void Enter()
        {
            _characterMover.MoveToNextPlatform(_animationDurationConfig.DefaultJumpAnimationTime);
        }

        public void Exit()
        {
        }
    }
}