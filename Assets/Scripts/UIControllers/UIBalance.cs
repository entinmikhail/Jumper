using System;
using Server;
using TMPro;
using UnityEngine;
using Zenject;

namespace UIControllers
{
    public class UIBalance : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        [Inject] private IFakeServer _fakeServer;

        private void Awake()
        {
            _textMesh.text = $"$ {1000}";
            _fakeServer.BalanceChanged += OnBalanceChanged;
        }

        private void OnBalanceChanged(float obj)
        {
            _textMesh.text = $"$ {Math.Round(obj, 2)}";
        }
    }
}