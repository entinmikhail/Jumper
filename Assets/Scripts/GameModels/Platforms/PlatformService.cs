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
    }

    public class PlatformService : IPlatformService
    {
        private Dictionary<int, PlatformContainer> _platformContainersByNumber = new();
        
        [Inject] private IPlatformCreator _platformCreator;
        
        public void ResetPlatformsData()
        {
            foreach (var platform in _platformContainersByNumber.Values)
                Object.Destroy(platform.gameObject);
            
            _platformContainersByNumber.Clear();
        }
        
        public bool TryAddPlatformObjectByData(int currentNumber)
        {

            if (_platformContainersByNumber.TryGetValue(currentNumber, out var platformContainer))
                return false;

            return _platformContainersByNumber.TryAdd(currentNumber, _platformCreator.CreatePlatform(currentNumber));
        }

        public bool TryGetPlatformContainer(int currentNumber, out PlatformContainer platform) => _platformContainersByNumber.TryGetValue(currentNumber, out platform);
    }
}