using System;
using Configs;
using GameModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIControllers
{
    public class UIBetButton : MonoBehaviour
    {
        [SerializeField] private ButtonType _type;
        [SerializeField] private float _value;
        [Space]
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _textMesh;

        [Inject] private IGameConfigs _gameConfigs;
        
        public event Action<UIBetButton> OnClick;

        public float Value => _value;
        public ButtonType Type => _type;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnClick?.Invoke(this));

            RefreshValues();
        }

        private void RefreshValues()
        {
            switch (_type)
            {
                case ButtonType.Max:
                    _textMesh.text = $"Max";
                    _value = _gameConfigs.MaxBet;
                    break;
                case ButtonType.Min:
                    _textMesh.text = $"Min";
                    _value = _gameConfigs.MinBet;
                    break;
                case ButtonType.Value:
                    _textMesh.text = $"${_value}";
                    break;
            }
        }
    }
}