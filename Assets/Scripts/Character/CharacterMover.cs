using System;
using Platforms;
using UnityEngine;
using Zenject;

namespace Character
{
    public interface ICharacterMover
    {
        void MoveToNextPlatform();
        event Action PlatformBroke;

    }

    public class CharacterMover : MonoBehaviour, ICharacterMover
    {
        [SerializeField] private CharacterController _characterController;

        public event Action PlatformBroke;
        public int CurrentCharacterPlatformNumber => _currentCharacterPlatformNumber;
        
        private int _currentCharacterPlatformNumber;
        
        [Inject] private IPlatformService _platformService;

        public void MoveToNextPlatform()
        {
            _currentCharacterPlatformNumber++;
            if (!_platformService.TryGetPlatformContainer(_currentCharacterPlatformNumber, out var nextPlatformContainer))
            {
                Debug.LogError($"PlatformContainer {_currentCharacterPlatformNumber} not found ");
                _currentCharacterPlatformNumber--;
                return;
            }

            if (nextPlatformContainer.PlatformData.IsBroken)
            {
                PlatformBroke?.Invoke();
                return;
            }

            switch (nextPlatformContainer.PlatformData.BonusType)
            {
                case BonusType.ExtraMultiplayer:
                    Debug.LogError("ExtraMultiplayer");
                    break;
                case BonusType.Non:
                    break;
                case BonusType.ExtraJump:
                    Debug.LogError("ExtraJump");
                    break;
            }

            _characterController.transform.position = nextPlatformContainer.CharacterRoot.position;
        }
    }
}