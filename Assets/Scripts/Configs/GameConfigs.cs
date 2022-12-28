namespace Configs
{
    public interface IGameConfigs
    {
        float MaxBet { get; }
        float MinBet { get; }
        float BonusFactor { get; set; }
        float MaxBonusFactor { get; }
        float DefaultFactor { get; }
    }

    public class GameConfigs : IGameConfigs
    {
        public float MaxBet { get; } = 100;
        public float MinBet { get; } = 1;
        public float BonusFactor { get; set; }
        public float MaxBonusFactor { get; } = 50;
        public float DefaultFactor { get; } = 1;
    }
}