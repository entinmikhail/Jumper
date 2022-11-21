using Character;
using Popups;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
    public class PlayerUiController : MonoBehaviour
    {
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _cashOutButton;

        [Inject] private ICharacterMover _characterMover;
        [Inject] private IPopupService _popupService;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(OnJump);
            _cashOutButton.onClick.AddListener(OnCashOut);
        }

        private void OnCashOut()
        {
            _popupService.ShowPopup(PopupType.WinPopup);
        }

        private void OnJump()
        {
            _characterMover.MoveToNextPlatform();
        }
    }
}