using UnityEngine;

namespace UIControllers
{
    public class BonusPointerRotator : MonoBehaviour
    {
        [SerializeField] private GameObject _pointer;
        [SerializeField] private float _minRotate;
        [SerializeField] private float _maxRotate;

        private Quaternion _targetTransformRotation;
        private Quaternion _savedRotation;
        private float _rotationSpeed;
        private bool _isRotating;

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
            if (_targetTransformRotation == _pointer.transform.rotation)
                _isRotating = false;
        }

        public void SetData(float totalFactor, float maxFactor, float animationTime)
        {
            var lerp = Mathf.InverseLerp(0, maxFactor, totalFactor);
            float targetAngle = Mathf.Lerp(_minRotate, _maxRotate, lerp);
            
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