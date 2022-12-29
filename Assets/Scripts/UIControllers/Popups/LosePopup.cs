using GameModels;
using UIControllers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Popups
{
    public class LosePopup : PopupBase
    {
        [SerializeField] private Button _button;

        [Inject] private IGameModel _gameModel;
        [Inject] private IUiBus _uiBus;
        [Inject] private IButtonsLockService _buttonsLockService;
        
        private void Awake()
        {
            _uiBus.SetActiveFactor(false);
            _button.onClick.AddListener(OnButton);
        }

        private void OnButton()
        {
            gameObject.SetActive(false);
            _gameModel.SetGameState(GameState.PrepareGameState);
            _uiBus.SetActiveFactor(true);
            _buttonsLockService.UnlockAllButtons();
        }
    }
}