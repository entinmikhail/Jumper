using System.Globalization;
using GameModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Popups
{
    public class WinPopup : PopupBase
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameModel _gameModel;
        
        private void Awake()
        {
            _button.onClick.AddListener(OnButton);
        }

        protected override void OnOpen()
        {
            _textMeshPro.text = $"$ {(_gameStorage.BetAmount * _gameStorage.CurrentFactor).ToString("0.00", CultureInfo.InvariantCulture)}";
        } 

        private void OnButton()
        {
            gameObject.SetActive(false);
            _gameModel.SetGameState(GameState.PrepareGameState);
        }
    }
}