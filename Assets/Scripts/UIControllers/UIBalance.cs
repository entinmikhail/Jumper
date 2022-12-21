using System.Globalization;
using GameModels;
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
            _textMesh.text = $"$ {_accountModel.CurrentBalance.ToString("0.00", CultureInfo.InvariantCulture)}";

            _accountModel.BalanceChanged += OnBalanceChanged;
        }

        private void OnBalanceChanged(float newBalance, string currency)
        {
            _textMesh.text = $"$ {newBalance.ToString("0.00", CultureInfo.InvariantCulture)}";
        }
    }
}