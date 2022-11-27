using GameModels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Popups
{
    public class LosePopup : PopupBase
    {
        [SerializeField] private Button _button;

        [Inject] private IGameModel _gameModel;
        private void Awake()
        {
            _button.onClick.AddListener(OnButton);
        }

        private void OnButton()
        {
            gameObject.SetActive(false);
            _gameModel.SetGameState(GameState.PrepareGameState);
        }
    }
}