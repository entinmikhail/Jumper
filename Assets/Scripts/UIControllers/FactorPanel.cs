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
        
        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameHandler _jumpInvoker;
        [Inject] private IGameModel _gameModel;

        private void Awake()
        {
            _textMeshPro.text = $"X {Math.Round(_gameStorage.CurrentCoefficient, 2)}";
            _jumpInvoker.Jumped += OnJump;
            _gameModel.GameStateChanged += OnJump;
        }
        

        private void OnJump(GameState obj)
        {
            _textMeshPro.text = $"X {Math.Round(_gameStorage.CurrentCoefficient, 2)}";
        }

        private void OnJump(int obj, string arg2)
        {
            _textMeshPro.text = $"X {Math.Round(_gameStorage.CurrentCoefficient, 2)}";
        }
    }
}