using Character;
using Platforms;
using Popups;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IPopupService _popupService;
        [Inject] private IGameStateController _gameStateController;
        
        private void Awake()
        {
            _characterMover.PlatformBroke += OnPlatformBroke;
        }

        private void Start()
        {
            _gameStateController.Start();
        }
        
        private void OnPlatformBroke()
        {
            _popupService.ShowPopup(PopupType.LosePopup);
        }
    }
}