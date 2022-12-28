using System;
using System.Globalization;
using Configs;
using GameModels;
using TMPro;
using UnityEngine;
using Zenject;

namespace UIControllers
{
    public class UIBetPanel : MonoBehaviour
    {
        [SerializeField] private UIBetButton[] _buttons;
        [SerializeField] private TextMeshProUGUI _textMesh;
        
        public float CurrentBet {get; private set;}
        public event Action<float> BetChanged;

        [Inject] private IGameModel _gameModel;
        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameConfigs _gameConfigs;
        [Inject] private INotificationService _notificationService;

        private void Awake()
        {
            foreach (var button in _buttons)
                button.OnClick += ChangeCurrentBetByButton;
        }

        public void SetBet(float bet)
        {
            CurrentBet = bet;
            _textMesh.text = $"{CurrentBet.ToString("0.00", CultureInfo.InvariantCulture)}";
        }

        private void ChangeCurrentBetByButton(UIBetButton uiBetButton)
        {
            if (_gameModel.GameState != GameState.PrepareGameState)
            {
                _notificationService.ShowNotification("Ставка уже сделана");
                return;
            }

            if (uiBetButton.Type is ButtonType.Max or ButtonType.Min)
                CurrentBet = uiBetButton.Value;
            else
            {
                if (CurrentBet + uiBetButton.Value > _gameConfigs.MaxBet)
                {
                    _notificationService.ShowNotification("Привыщает максимальную ставку");
                    return;
                }
                
                CurrentBet += uiBetButton.Value;
            }

            _gameStorage.BetAmount = CurrentBet;
            BetChanged?.Invoke(CurrentBet);
            _textMesh.text = $"{CurrentBet.ToString("0.00", CultureInfo.InvariantCulture)}";
        }
    }
}