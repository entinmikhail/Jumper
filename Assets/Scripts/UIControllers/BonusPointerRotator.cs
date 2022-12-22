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
        private float _curFactor = 1;
        private float _prevFactor;
        private float _animationTime;
        
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
            
            _curFactor += (_totalFactor - 1) / _animationTime * Time.deltaTime;
            _text.text = $"x{_curFactor.ToString("0.00", CultureInfo.InvariantCulture)}";

            if (_targetTransformRotation == _pointer.transform.rotation)
                _isRotating = false;
        }

        public void SetData(float totalFactor, float animationTime, float prevFactor)
        {
            _totalFactor = totalFactor;
            _prevFactor = prevFactor;
            _animationTime = animationTime;
            
            var lerp = Mathf.InverseLerp(0, totalFactor, totalFactor);
            var targetAngle = Mathf.Lerp(_minRotate, _maxRotate, lerp);
            var path = Math.Abs(_minRotate) + Math.Abs(targetAngle);
          
            _targetTransformRotation = Quaternion.Euler(0, 0, targetAngle);
            _rotationSpeed = path / animationTime;
            _isRotating = true;
        }

        public void ResetRotation()
        {
            _curFactor = 1;
            _pointer.transform.rotation = _savedRotation;
        }
    }
}