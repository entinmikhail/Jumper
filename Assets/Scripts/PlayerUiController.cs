using Character;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
    public class PlayerUiController : MonoBehaviour
    {
        [SerializeField] private Button _jumpButton;

        [Inject] private ICharacterMover _characterMover;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(OnJump);
        }

        private void OnJump()
        {
            _characterMover.MoveToNextPlatform();
        }
    }
}