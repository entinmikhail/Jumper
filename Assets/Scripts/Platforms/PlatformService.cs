using System.Collections.Generic;
using Zenject;

namespace Platforms
{
    public interface IPlatformService
    {
        bool TryAddPlatformObjectByData(int currentNumber, PlatformData platform);
        bool TryGetPlatformContainer(int currentNumber, out PlatformContainer platform);
        void ResetPlatformsData();
    }

    public class PlatformService : IPlatformService
    {
        private Dictionary<int, PlatformContainer> _platformContainersByNumber = new();
        
        private int _currentNumber = 1;

        [Inject] private IPlatformCreator _platformCreator;

        public void ResetPlatformsData()
        {
            _platformContainersByNumber.Clear();
        }
        
        public bool TryAddPlatformObjectByData(int currentNumber, PlatformData platformData)
        {
            return _platformContainersByNumber.TryAdd(currentNumber, _platformCreator.CreatePlatform(currentNumber, platformData));
        }

        public bool TryGetPlatformContainer(int currentNumber, out PlatformContainer platform) => _platformContainersByNumber.TryGetValue(currentNumber, out platform);
    }
}