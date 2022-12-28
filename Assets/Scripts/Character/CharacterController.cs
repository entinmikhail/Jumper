using System;
using UnityEngine;

namespace Character
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject _particle;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Lose = Animator.StringToHash("Lose");
        private static readonly int Win = Animator.StringToHash("Win");


        private void Start()
        {
            SetParticle(false);
        }

        public void SetParticle(bool value)
        {
            _particle.gameObject.SetActive(value);
        }
        
        public void PlayIdle()
        {
            _animator.SetTrigger(Idle);
        }
        
        public void PlayJump()
        {
            _animator.SetTrigger(Jump);
        }
        
        public void PlayLose()
        {
            _animator.SetTrigger(Lose);
        }
        
        public void PlayWin()
        {
            _animator.SetTrigger(Win);
        }
    }
}