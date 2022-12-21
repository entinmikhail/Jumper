namespace Configs
{
    public interface IGameConfigs
    {
        float MaxBet { get; }
        float MinBet { get; }
        float BonusPrice { get; }
        float MaxBonusFactor { get; }
        float DefaultFactor { get; }
    }

    public class GameConfigs : IGameConfigs
    {
        public float MaxBet { get; } = 100;
        public float MinBet { get; }  = 1;
        public float BonusPrice { get; }  = 30;
        public float MaxBonusFactor { get; }  = 50;
        public float DefaultFactor { get; }  = 1;
    }
}