using Character;
using GameModels.StateMachine;
using Platforms;
using Popups;
using Zenject;

namespace Gameplay
{
    public interface ILoseGameState : IState
    {
    }

    public class LoseGameState : ILoseGameState
    {
        [Inject] private IPopupService _popupService;
        [Inject] private IPlatformService _platformService;
        [Inject] private ICharacterMover _characterMover;

        public async void Enter()
        {
            await _characterMover.MoveToNextPlatform();
        }

        public void Exit()
        {
            _platformService.ResetPlatformsData();
        }
    }
}