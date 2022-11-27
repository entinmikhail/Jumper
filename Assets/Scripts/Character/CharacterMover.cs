using System;
using GameModels;
using Platforms;
using UnityEngine;
using Zenject;

namespace Character
{
    public interface ICharacterMover
    {
        void MoveToNextPlatform();
        void ResetCharacterPosition();
        void SetNumberPlatform(int numberPlatform);
        event Action PlatformBroke;
    }

    public class CharacterMover : MonoBehaviour, ICharacterMover
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _characterSpawnRoot;

        public event Action PlatformBroke;
        
        private int _currentCharacterPlatformNumber;
        
        [Inject] private IPlatformService _platformService;
        [Inject] private IGameModel _gameModel;


        private void Awake()
        {
            _gameModel.Jumped += SetNumberPlatform;
        }

        public void ResetCharacterPosition()
        {
            _currentCharacterPlatformNumber = 0;
            _characterController.gameObject.transform.position = _characterSpawnRoot.position;
        }

        public void SetNumberPlatform(int numberPlatform)
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
        
        public void MoveToNextPlatform()
        {
            _currentCharacterPlatformNumber++;
            if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var nextPlatformContainer))
            {
                Debug.LogError($"PlatformContainer {_currentCharacterPlatformNumber} not found ");
                _currentCharacterPlatformNumber--;
                return;
            }
            
            _characterController.transform.position = nextPlatformContainer.CharacterRoot.position;
        }
    }
}