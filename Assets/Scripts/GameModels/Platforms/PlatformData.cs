namespace Platforms
{
    public enum BonusType
    {
        Non,
        ExtraJump,
        ExtraMultiplayer
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