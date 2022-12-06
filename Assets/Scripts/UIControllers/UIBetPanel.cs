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
        
        public float CurrentBet {get; set;}

        [Inject] private IGameModel _gameModel;

        private void Awake()
        {
            foreach (var button in _buttons)
                button.OnClick += ChangeCurrentBetByButton;
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

            _textMesh.text = $"{CurrentBet}";
        }
    }
}