using System;
using System.Globalization;
using Configs;
using TMPro;
using UnityEngine;
using Zenject;

namespace UIControllers
{
    public class BonusPointerRotator : MonoBehaviour
    {
        [SerializeField] private GameObject _pointer;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _minRotate;
        [SerializeField] private float _maxRotate;

        private Quaternion _targetTransformRotation;
        private Quaternion _savedRotation;
        private float _rotationSpeed;
        private bool _isRotating;

        private float _totalFactor;
        private float _maxFactor;
        private float _curFactor;
        private float _prevFactor;
        private float _animationTime;

        [Inject] private IGameConfigs _gameConfigs;

        private void Awake()
        {
            _savedRotation = _pointer.transform.rotation;
        }

        private void Update()
        {
            if (!_isRotating)
                return;
            
            var newRotateTowards= Quaternion.RotateTowards(_pointer.transform.rotation, _targetTransformRotation, _rotationSpeed * Time.deltaTime);
            _pointer.transform.rotation = newRotateTowards;
            
            // _curFactor += _totalFactor/ _animationTime / _prevFactor * Time.deltaTime;
            
            _text.text = $"x{Math.Round(_curFactor, 2)}";
            _text.text = _curFactor.ToString("0.00", CultureInfo.InvariantCulture);

            if (_targetTransformRotation == _pointer.transform.rotation)
                _isRotating = false;
        }

        public void SetData(float totalFactor, float animationTime, float prevFactor)
        {
            _totalFactor = totalFactor;
            _prevFactor = prevFactor;
            _animationTime = animationTime;
            
            var lerp = Mathf.InverseLerp(0, _gameConfigs.MaxBonusFactor, totalFactor);
            var targetAngle = Mathf.Lerp(_minRotate, _maxRotate, lerp);
            
            _targetTransformRotation = Quaternion.Euler(0, 0, targetAngle);
            _rotationSpeed = targetAngle / animationTime;
            _isRotating = true;
        }

        public void ResetRotation()
        {
            _pointer.transform.rotation = _savedRotation;
        }
    }
}