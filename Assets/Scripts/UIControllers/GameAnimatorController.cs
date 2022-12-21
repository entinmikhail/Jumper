using UnityEngine;

namespace UIControllers
{
    public interface IGameAnimatorController
    {
        void PlayBonusJump();
        void PlayIdle();
        void StartRotationAnimation(float totalFactor, float animationTime, float prevFactor);
        void ResetAnimations();
    }

    public class GameAnimatorController : MonoBehaviour, IGameAnimatorController
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private BonusPointerRotator _bonusPointerRotator;
        
        private static readonly int Bonus = Animator.StringToHash("Bonus");
        private static readonly int Idle = Animator.StringToHash("Idle");


        public void StartRotationAnimation(float totalFactor, float animationTime, float prevFactor)
        {
            _bonusPointerRotator.SetData(totalFactor, animationTime, prevFactor);
        }

        public void ResetAnimations()
        {
            _bonusPointerRotator.ResetRotation();
        }

        public void PlayBonusJump()
        {
            _animator.SetTrigger(Bonus);
        }
        public void PlayIdle()
        {
            _animator.SetTrigger(Idle);
        }
    }
}