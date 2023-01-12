using System;
using System.Collections.Generic;
using ModestTree.Util;

namespace Configs
{
    public interface IGameConfigs
    {
        float MaxBet { get; set; }
        float MinBet { get; set; }
        float BonusFactor { get; set; }
        float MaxBonusFactor { get; }
        float DefaultFactor { get; }
        Dictionary<string, object> CurrencyFactor { get; }
        event Action ConfigsRefreshed;
        void SetCurrencyFactor(Dictionary<string, object> data);
    }

    public class GameConfigs : IGameConfigs
    {
        public float MaxBet { get; set; } = 100;
        public float MinBet { get; set; } = 1;
        public float BonusFactor { get; set; }
        public float MaxBonusFactor { get; } = 50;
        public float DefaultFactor { get; } = 1;
        public Dictionary<string, object> CurrencyFactor { get; set; } = new();
        public event Action ConfigsRefreshed;

        public void SetCurrencyFactor(Dictionary<string, object> data)
        {
            CurrencyFactor = data;
            ConfigsRefreshed?.Invoke();
        }
    }
}