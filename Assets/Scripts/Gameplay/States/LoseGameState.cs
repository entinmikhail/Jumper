using Character;
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
        [Inject] private IPlatformService _platformService;
        [Inject] private ICharacterMover _characterMover;

        public void Enter()
        {
            _characterMover.MoveToNextPlatform();
        }

        public void Exit()
        {
            _platformService.ResetPlatformsData();
        }
    }
}