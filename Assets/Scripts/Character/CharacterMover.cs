using System;
using GameModels;
using Platforms;
using Popups;
using UnityEngine;
using Zenject;

namespace Character
{
    public interface ICharacterMover
    {
        void MoveToNextPlatform();
        void ResetCharacterPosition();
        void SetNumberPlatform(int numberPlatform);
        void SetNumberPlatformSync(int numberPlatform);
        void RefreshCharacter();
        void SetIdle();
        void SetCharacterToBonusPosition();
        event Action PlatformBroke;
        event Action MoveEnd;
        void SetActive(bool value);
    }

    public class CharacterMover : MonoBehaviour, ICharacterMover
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _characterSpawnRoot;
        [SerializeField] private Transform _characterBonusAnimationRoot;
        [SerializeField] private int _characterSpeed = 10;

        public event Action PlatformBroke;
        public event Action MoveEnd;
        
        private int _currentCharacterPlatformNumber;
        private Transform _nextPlatformTransform;
        private bool _inMove;
        
        [Inject] private IPlatformService _platformService;
        [Inject] private IGameModel _gameModel;
        [Inject] private IPopupService _popupService;
        [Inject] private ICoroutineRunner _coroutineRunner;
        
        private void Awake()
        {
            _gameModel.Jumped += numberPlatform => SetNumberPlatform(numberPlatform);
            MoveEnd += OnMoveEnd;
        }
        
        private void Update()
        {
            if (!_inMove)
                return;

            _characterController.transform.position = Vector2.MoveTowards(
                _characterController.transform.position, 
                _nextPlatformTransform.position, 1 * Time.deltaTime * _characterSpeed);

            if (_characterController.transform.position != _nextPlatformTransform.position)
                return;
                
            _inMove = false;
            
            MoveEnd?.Invoke();
        }

        public void SetIdle() => _characterController.PlayIdle();
        public void SetActive(bool value) => _characterController.SpriteRenderer.gameObject.SetActive(value);
        public void SetCharacterToBonusPosition() => _characterController.transform.position = _characterBonusAnimationRoot.position;

        private void OnMoveEnd()
        {
            if (_gameModel.GameState == GameState.StartGameplay)
            {
                RotateCharacter();
                _characterController.PlayWin();
            }

            if (_gameModel.GameState == GameState.Lose)
            {
                RotateCharacter();
                
                if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var platform))
                {
                    Debug.LogError($"PlatformContainer {_currentCharacterPlatformNumber} not found ");
                    return;
                }

                platform.gameObject.SetActive(false);
                _characterController.PlayLose();
                
                _coroutineRunner.StartAfterDelay(1.5f, () =>
                {
                    _characterController.SpriteRenderer.enabled = false;
                    _popupService.ShowPopup(PopupType.LosePopup);
                });

            }
        }
        
        public void SetNumberPlatformSync(int numberPlatform)
        {
            _currentCharacterPlatformNumber = numberPlatform;
            if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var nextPlatformContainer))
            {
                Debug.LogError($"PlatformContainer {_currentCharacterPlatformNumber} not found ");
                _currentCharacterPlatformNumber--;
                return;
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

        public void SetNumberPlatform(int numberPlatform)
        {
            _currentCharacterPlatformNumber = numberPlatform;

            Debug.LogError(numberPlatform);
            if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var nextPlatformContainer))
            {
                Debug.LogError($"PlatformContainer {_currentCharacterPlatformNumber} not found ");
                _currentCharacterPlatformNumber--;
                return;
            }
            
            _characterController.PlayJump();

            _coroutineRunner.StartAfterDelay(0.3f, () =>
            {
                _nextPlatformTransform = nextPlatformContainer.CharacterRoot;
                _inMove = true;
            });

        }

        public void MoveToNextPlatform()
        {
            _currentCharacterPlatformNumber++;
            if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var nextPlatformContainer))
            {
                Debug.LogError($"PlatformContainer {_currentCharacterPlatformNumber} not found ");
                _currentCharacterPlatformNumber--;
                return;
            }

            _characterController.PlayJump();
            
            _coroutineRunner.StartAfterDelay(0.3f, () =>
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
    }
}