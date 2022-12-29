using System;
using Cinemachine;
using Configs;
using GameModels;
using Platforms;
using Popups;
using UnityEngine;
using Zenject;

namespace Character
{
    public interface ICharacterMover
    {
        void MoveToNextPlatform(float time);
        void ResetCharacterPosition();
        void SetNumberPlatform(int numberPlatform, float time);
        void SetNumberPlatformWithoutAnimation(int numberPlatform);
        void RefreshCharacter();
        void SetIdle();
        event Action PlatformBroke;
        event Action MoveEnd;
        void SetActive(bool value);
        void RotateCharacter();
        void RotateCharacter(int currentCharacterPlatformNumber);
        CharacterController CharacterController { get; }
    }

    public class CharacterMover : MonoBehaviour, ICharacterMover
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private Transform _characterSpawnRoot;

        public CharacterController CharacterController => _characterController;
        public event Action PlatformBroke;
        public event Action MoveEnd;

        private Transform _nextPlatformTransform;
        
        private int _currentCharacterPlatformNumber;
        private float _speed;
        private bool _inMove;

        [Inject] private IGameModel _gameModel;
        [Inject] private IPopupService _popupService;
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private IPlatformService _platformService;
        [Inject] private IAnimationDurationConfig _animationDurationConfig;

        private void Awake()
        {
            MoveEnd += OnMoveEnd;
        }
        
        private void Update()
        {
            if (!_inMove)
                return;

            _characterController.transform.position = Vector2.MoveTowards(
                _characterController.transform.position, 
                _nextPlatformTransform.position, 1 * Time.deltaTime * _speed);

            if (_characterController.transform.position != _nextPlatformTransform.position)
                return;
                
            _inMove = false;
            
            MoveEnd?.Invoke();
        }

        public void SetIdle() => _characterController.PlayIdle();
        public void SetActive(bool value) => _characterController.SpriteRenderer.gameObject.SetActive(value);

        private void OnMoveEnd()
        {
            if (_gameModel.GameState is GameState.StartGameplay or GameState.Bonus or GameState.ContinueGameAfterLogin)
            {
                _characterController.PlayWin();
                RotateCharacter();
            }

            if (_gameModel.GameState == GameState.Lose)
            {
                if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var platform))
                {
                    GenerateUndignifiedPlatform();
                    _platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out platform);
                }

                _coroutineRunner.StartAfterDelay(_animationDurationConfig.IdleDelayAnimationTime, () =>
                {
                    platform.gameObject.SetActive(false);
                });

                _characterController.PlayLose();
                RotateCharacter();

                _coroutineRunner.StartAfterDelay(_animationDurationConfig.LoseAnimationTime, () =>
                {
                    _characterController.SpriteRenderer.enabled = false;
                    _popupService.ShowPopup(PopupType.LosePopup);
                });
            }
        }
        
        public void SetNumberPlatformWithoutAnimation(int numberPlatform)
        {
            _currentCharacterPlatformNumber = numberPlatform;
            if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var nextPlatformContainer))
            {
                Debug.LogError($"PlatformContainer {_currentCharacterPlatformNumber} not found ");
                GenerateUndignifiedPlatform();
                _platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out nextPlatformContainer);

            }
            
            _characterController.transform.position = nextPlatformContainer.CharacterRoot.position;
        }
        

        public void RefreshCharacter()
        {
            _characterController.SpriteRenderer.enabled = true;
            _characterController.PlayIdle();
            ResetCharacterPosition();
            RotateCharacter();
        }

        public void ResetCharacterPosition()
        {
            _currentCharacterPlatformNumber = 0;
            _characterController.gameObject.transform.position = _characterSpawnRoot.position;
        }

        public void SetNumberPlatform(int numberPlatform, float time)
        {
            _currentCharacterPlatformNumber = numberPlatform;

            if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var nextPlatformContainer))
            {
                Debug.LogError($"PlatformContainer {_currentCharacterPlatformNumber} not found ");

                GenerateUndignifiedPlatform();
                
                _platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out nextPlatformContainer);
            }
            
            _characterController.PlayJump();
            
            _speed = Vector2.Distance(nextPlatformContainer.CharacterRoot.position, _characterController.transform.position) / time;
            
            _coroutineRunner.StartAfterDelay(_animationDurationConfig.IdleDelayAnimationTime, () =>
            {
                _nextPlatformTransform = nextPlatformContainer.CharacterRoot;
                _inMove = true;
            });
        }

        private void GenerateUndignifiedPlatform()
        {
            for (int i = 1; i < _currentCharacterPlatformNumber + 2; i++)
            {
                if (_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var p))
                    continue;

                _platformService.TryAddPlatformObjectByData(i);
            }
        }

        public void MoveToNextPlatform(float time)
        {
            _currentCharacterPlatformNumber++;
            if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var nextPlatformContainer))
            {
                Debug.LogError($"PlatformContainer {_currentCharacterPlatformNumber} not found ");
                
                GenerateUndignifiedPlatform();
                _platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out nextPlatformContainer);

            }

            _characterController.PlayJump();

            _speed = Vector2.Distance(nextPlatformContainer.CharacterRoot.position, _characterController.transform.position) / time;
            
            _coroutineRunner.StartAfterDelay(_animationDurationConfig.IdleDelayAnimationTime, () =>
            {
                _nextPlatformTransform = nextPlatformContainer.CharacterRoot;
                _inMove = true;
            });
        }

        public void RotateCharacter()
        {
            _characterController.gameObject.transform.localScale = _currentCharacterPlatformNumber % 2 == 0
                ? new Vector3(1, 1, 1) 
                : new Vector3(-1, 1, 1);
        }
        
        public void RotateCharacter(int currentCharacterPlatformNumber)
        {
            _characterController.gameObject.transform.localScale =  currentCharacterPlatformNumber% 2 == 0
                ? new Vector3(1, 1, 1) 
                : new Vector3(-1, 1, 1);
        }
    }
}