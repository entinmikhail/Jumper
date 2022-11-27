using Character;
using Popups;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IGameplayState : IState
    {
    }

    public class GameplayState : IGameplayState
    {
        // [Inject] private ICharacterMover _characterMover;
        // [Inject] private IPopupService _popupService;
        // [Inject] private IGameStateController _gameStateController;
        //
        // public void Enter()
        // {
        //     _characterMover.PlatformBroke += OnPlatformBroke;
        //     _gameStateController.Start();
        // }
        //
        // public void Exit()
        // {
        //     _characterMover.PlatformBroke -= OnPlatformBroke;
        // }
        //
        // private void OnPlatformBroke()
        // {
        //     _popupService.ShowPopup(PopupType.LosePopup);
        // }

        public void Enter()
        {
        }
        

        public void Exit()
        {
            
        }
    }
}