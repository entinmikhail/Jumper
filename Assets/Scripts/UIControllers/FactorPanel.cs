using System.Globalization;
using Character;
using GameModels;
using TMPro;
using UnityEngine;
using Zenject;

namespace UIControllers
{
    public class FactorPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        
        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameModel _gameModel;
        [Inject] private ICharacterMover _characterMover;

        private void Awake()
        {
            _characterMover.MoveEnd += OnMoveEnd;
        }

        private void OnMoveEnd()
        {
            if (_gameModel.GameState == GameState.Lose)
                return;
            
            Refresh();
        }

        public void Refresh()
        {
            _textMeshPro.text = $"x{_gameStorage.CurrentFactor.ToString("0.00", CultureInfo.InvariantCulture)}";
        }
    }
}