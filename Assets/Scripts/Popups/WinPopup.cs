using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Popups
{
    public class WinPopup : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [Inject] private IGameStateController _gameStateController;
        private void Awake()
        {
            _button.onClick.AddListener(OnButton);
        }

        private void OnButton()
        {
            gameObject.SetActive(false);
            _gameStateController.Restart();
        }
    }
}