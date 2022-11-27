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

        public void Enter()
        {
            _popupService.ShowPopup(PopupType.LosePopup);
        }

        public void Exit()
        {
            _platformService.ResetPlatformsData();
        }
    }
}