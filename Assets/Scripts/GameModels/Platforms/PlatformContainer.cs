using UnityEngine;

namespace Platforms
{
    public class PlatformContainer : MonoBehaviour
    {
        [SerializeField] private Transform _characterRoot;
        [SerializeField] private GameObject _extraJump;
        [SerializeField] private GameObject _extraFactor;
        
        public Transform CharacterRoot => _characterRoot;
        public BonusType BonusType { get; private set; }

        public void SetData(BonusType bonusType)
        {
            BonusType = bonusType;
            _extraFactor.SetActive(false);
            _extraJump.SetActive(false);
            
            switch (bonusType)
            {
                case BonusType.Non:
                    break;
                case BonusType.ExtraJump:
                    _extraJump.SetActive(true);
                    break;
                case BonusType.ExtraFactor:
                    _extraFactor.SetActive(true);
                    break;
                case BonusType.Unknown:
                    break;
            }
        }
    }
}