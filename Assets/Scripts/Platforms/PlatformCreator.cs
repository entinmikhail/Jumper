using UnityEngine;

namespace Platforms
{
    public interface IPlatformCreator
    {
        PlatformContainer CreatePlatform(int currentNumber, PlatformData data);
    }

    public class PlatformCreator : MonoBehaviour, IPlatformCreator
    {
        [SerializeField] private PlatformContainer _platformPrefab;
        [SerializeField] private Vector2 _platformOffset;
        
        public PlatformContainer CreatePlatform(int currentNumber, PlatformData data)
        {
            var platform = Instantiate(_platformPrefab);
            var platformTransform = platform.transform;
            var posX = currentNumber % 2 == 1 ? platformTransform.position.x : -platformTransform.position.x;
            platformTransform.position = new Vector3(posX, _platformOffset.y * currentNumber);
            platform.SetData(data);
            
            return platform;
        }
    }
}
