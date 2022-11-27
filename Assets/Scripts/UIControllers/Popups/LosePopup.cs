using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Popups
{
    public class LosePopup : MonoBehaviour
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
            Debug.LogError("Ты лох");
            _gameStateController.Restart();
        }
    }
}