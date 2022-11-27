using UnityEngine;

namespace Platforms
{
    public class PlatformContainer : MonoBehaviour
    {
        [SerializeField] private Transform _characterRoot;
        [SerializeField] private Transform _bonusRoot;

        public Transform CharacterRoot => _characterRoot;
        public Transform BonusRoot => _bonusRoot;
        public PlatformData PlatformData => _platformData;

        private PlatformData _platformData;

        public void SetData(PlatformData platformData)
        {
            _platformData = platformData;
        }
    }
}