using System;
using GameModels;
using Server;
using TMPro;
using UnityEngine;
using Zenject;

namespace UIControllers
{
    public class UIBalance : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        
        [Inject] private IAccountModel _accountModel;

        private void Awake()
        {
            _textMesh.text = $"$ {1000}";
            _accountModel.BalanceChanged += OnBalanceChanged;
        }

        private void OnBalanceChanged(float newBalance, string currency)
        {
            _textMesh.text = $"$ {Math.Round(newBalance, 2)}";
        }
    }
}