using System;
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
        [Inject] private ICharacterMover _characterMover;

        private void Awake()
        {
            _characterMover.MoveEnd += OnMoveEnd;
        }

        public void Initialize()
        {
            _textMeshPro.text = $"x{_gameStorage.CurrentCoefficient.ToString("0.00", CultureInfo.InvariantCulture)}";
        }

        private void OnMoveEnd()
        {
            
            _textMeshPro.text = $"x{_gameStorage.CurrentCoefficient.ToString("0.00", CultureInfo.InvariantCulture)}";
            
        }
    }
}