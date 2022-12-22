using UnityEngine;

namespace Platforms
{
    public class PlatformContainer : MonoBehaviour
    {
        [SerializeField] private Transform _characterRoot;
        [SerializeField] private GameObject _extraJump;
        [SerializeField] private GameObject _extraFactor;
        [SerializeField] private Animator _animator;
        
        private static readonly int Open2X = Animator.StringToHash("Open2X");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int OpenPlus1 = Animator.StringToHash("OpenPLUS1");
        private static readonly int Disable = Animator.StringToHash("Disable");

        public Transform CharacterRoot => _characterRoot;
        public BonusType BonusType { get; private set; }
        
        public void PlayOpen2X()
        {
            _animator.SetTrigger(Open2X);
        }
        public void PlayIdle()
        {
            _animator.SetTrigger(Idle);
        }
        public void PlayOpenPLUS1()
        {
            _animator.SetTrigger(OpenPlus1);
        }
        public void PlayDisable()
        {
            _animator.SetTrigger(Disable);
        }

        public void OnMoveEnd()
        {
            switch (BonusType)
            {
                case BonusType.Non:
                    break;
                case BonusType.ExtraJump:
                    PlayOpenPLUS1();
                    break;
                case BonusType.ExtraFactor:
                    PlayOpen2X();
                    break;
                case BonusType.Unknown:
                    break;
            }
        }
        
        public void SetBonus(BonusType bonusType)
        {
            BonusType = bonusType;
            _extraFactor.SetActive(false);
            _extraJump.SetActive(false);
            
            switch (bonusType)
            {
                case BonusType.Non:
                    break;
                case BonusType.ExtraJump:
                    PlayIdle();
                    break;
                case BonusType.ExtraFactor:
                    PlayIdle();
                    break;
                case BonusType.Unknown:
                    break;
            }
        }
    }
}