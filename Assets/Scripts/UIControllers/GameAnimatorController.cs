using UnityEngine;

namespace UIControllers
{
    public interface IGameAnimatorController
    {
        void PlayBonusJump();
    }

    public class GameAnimatorController : MonoBehaviour, IGameAnimatorController
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int Bonus = Animator.StringToHash("Bonus");
        
        public void PlayBonusJump()
        {
            _animator.SetTrigger(Bonus);
        }
    }
}