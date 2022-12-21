using System;
using System.Globalization;
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

        [Inject] private IGameModel _gameModel;
        [Inject] private IGameStorage _gameStorage;

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
                Debug.LogError("GameState != GameState.PrepareGameState");
                return;
            }
            
            if (uiBetButton.Type is ButtonType.Max or ButtonType.Min)
                CurrentBet = uiBetButton.Value;
            else
                CurrentBet += uiBetButton.Value;

            _gameStorage.BetAmount = CurrentBet;
            
            _textMesh.text = $"{CurrentBet.ToString("0.00", CultureInfo.InvariantCulture)}";
        }
    }
}