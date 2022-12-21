using GameModels.StateMachine;
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

        public void Enter()
        {
            _popupService.ShowPopup(PopupType.WinPopup);
        }

        public void Exit()
        {
        }
    }
}