using System;
using UnityEngine;

namespace Platforms
{
    public interface IPlatformCreator
    {
        PlatformContainer CreatePlatform(int currentNumber);
    }

    public class PlatformCreator : MonoBehaviour, IPlatformCreator
    {
        [SerializeField] private PlatformContainer _platformPrefab;
        [SerializeField] private Transform _platformSpawnRoot;
        [SerializeField] private Vector2 _platformOffset;

        public PlatformContainer CreatePlatform(int currentNumber)
        {
            var platform = Instantiate(_platformPrefab, _platformSpawnRoot.position, Quaternion.identity);
            var platformTransform = platform.transform;
            var posX = currentNumber % 2 == 1 ? _platformOffset.y : -_platformOffset.y;
            platformTransform.position = new Vector3(posX, _platformOffset.y * currentNumber);
            platform.gameObject.name = $"Platform - {currentNumber}";
            platform.gameObject.SetActive(true);
            
            return platform;
        }
    }
}
