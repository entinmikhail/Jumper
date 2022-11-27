using System;
using GameModels;
using TMPro;
using UnityEngine;
using Zenject;

namespace UIControllers
{
    public class FactorPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        
        [Inject] private IGameModel _gameModel;

        private void Awake()
        {
            _textMeshPro.text = $"X {Math.Round(_gameModel.CurrentCoefficient, 2)}";
            _gameModel.Jumped += OnJump;
            _gameModel.GameStateChanged += OnJump;
        }

        private void OnJump(GameState obj)
        {
            _textMeshPro.text = $"X {Math.Round(_gameModel.CurrentCoefficient, 2)}";
        }

        private void OnJump(int obj)
        {
            _textMeshPro.text = $"X {Math.Round(_gameModel.CurrentCoefficient, 2)}";
        }
    }
}