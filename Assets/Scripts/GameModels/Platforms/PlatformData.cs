namespace Platforms
{
    public enum BonusType
    {
        Non,
        ExtraJump,
        ExtraFactor,
        Unknown
    }
    
    public class PlatformData
    {
        public bool IsBroken;
        public BonusType BonusType;

        public PlatformData(bool isBroken, BonusType bonusType)
        {
            IsBroken = isBroken;
            BonusType = bonusType;
        }
    }
}