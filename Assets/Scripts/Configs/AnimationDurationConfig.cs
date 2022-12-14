using UnityEngine;

namespace Configs
{
    public interface IAnimationDurationConfig
    {
        float IdleDelayAnimationTime { get; }
        float LoseAnimationTime { get; }
        float WinAnimationTime { get; }
        float DefaultJumpAnimationTime { get; }
        float BonusJumpAnimationTime { get; }
        float AwaitingBonusAnimationTime { get; }
        float BonusRotateAnimationTime { get; }
    }

    [CreateAssetMenu(fileName = @"AnimationDurationConfig", menuName = @"Configurations/Animation Duration Config", order = 20)]

    public class AnimationDurationConfig : ScriptableObject, IAnimationDurationConfig
    {
        [SerializeField] private float _awaitingBonusAnimationTime;
        [SerializeField] private float _bonusRotateAnimationTime;
        [SerializeField] private float _defaultJumpAnimationTime;
        [SerializeField] private float _bonusJumpAnimationTime;
        [SerializeField] private float _idleDelayAnimationTime;
        [SerializeField] private float _loseAnimationTime;
        [SerializeField] private float _winAnimationTime;

        public float AwaitingBonusAnimationTime => _awaitingBonusAnimationTime;

        public float BonusRotateAnimationTime => _bonusRotateAnimationTime;
        public float IdleDelayAnimationTime => _idleDelayAnimationTime;
        public float LoseAnimationTime => _loseAnimationTime;
        public float WinAnimationTime => _winAnimationTime;
        public float DefaultJumpAnimationTime => _defaultJumpAnimationTime;
        public float BonusJumpAnimationTime => _bonusJumpAnimationTime;
    }
}