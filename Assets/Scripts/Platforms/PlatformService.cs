using System.Collections.Generic;
using Zenject;

namespace Platforms
{
    public interface IPlatformService
    {
        bool TryAddPlatformObjectByData(int currentNumber, PlatformData platform);
        bool TryGetPlatformContainer(int currentNumber, out PlatformContainer platform);
    }

    public class PlatformService : IPlatformService
    {
        private Dictionary<int, PlatformContainer> _platformContainersByNumber = new();
        
        private int _currentNumber = 1;

        [Inject] private IPlatformCreator _platformCreator;

        public bool OldTryAdd(int currentNumber, PlatformContainer platform) => _platformContainersByNumber.TryAdd(currentNumber, platform);
        
        public bool TryAddPlatformObjectByData(int currentNumber, PlatformData platformData)
        {
            return _platformContainersByNumber.TryAdd(currentNumber, _platformCreator.CreatePlatform(currentNumber, platformData));
        }

        public bool TryGetPlatformContainer(int currentNumber, out PlatformContainer platform) => _platformContainersByNumber.TryGetValue(currentNumber, out platform);
    }
}