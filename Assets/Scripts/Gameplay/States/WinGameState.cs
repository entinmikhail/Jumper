using GameModels.StateMachine;
using Platforms;
using Popups;
using Zenject;

namespace Gameplay
{
    public interface IWinGameState : IState
    {
    }

    public class WinGameState : IWinGameState
    {
        [Inject] private IPopupService _popupService;
        [Inject] private IPlatformService _platformService;

        public void Enter()
        {
            _popupService.ShowPopup(PopupType.WinPopup);
        }

        public void Exit()
        {
            _platformService.ResetPlatformsData();
        }
    }
}