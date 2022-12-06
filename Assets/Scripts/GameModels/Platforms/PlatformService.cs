using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Platforms
{
    public interface IPlatformService
    {
        bool TryAddPlatformObjectByData(int currentNumber);
        bool TryGetPlatformContainer(int currentNumber, out PlatformContainer platform);
        void ResetPlatformsData();
        bool TryGenerateNextPlatform();
    }

    public class PlatformService : IPlatformService
    {
        private Dictionary<int, PlatformContainer> _platformContainersByNumber = new();
        
        private int _currentNumber = 1;

        [Inject] private IPlatformCreator _platformCreator;

        public void ResetPlatformsData()
        {
            foreach (var platform in _platformContainersByNumber.Values)
                Object.Destroy(platform.gameObject);
            
            _platformContainersByNumber.Clear();
        }

        public bool TryGenerateNextPlatform()
        {
            if (_platformContainersByNumber.ContainsKey(_currentNumber + 1))
                return false;
            
            _currentNumber++;
            return _platformContainersByNumber.TryAdd(_currentNumber, _platformCreator.CreatePlatform(_currentNumber));
        }
        
        public bool TryAddPlatformObjectByData(int currentNumber)
        {
            if (_platformContainersByNumber.ContainsKey(currentNumber))
                return false;
                    
            _currentNumber = currentNumber;
            return _platformContainersByNumber.TryAdd(currentNumber, _platformCreator.CreatePlatform(currentNumber));
        }

        public bool TryGetPlatformContainer(int currentNumber, out PlatformContainer platform) => _platformContainersByNumber.TryGetValue(currentNumber, out platform);
    }
}