using System;

namespace Server
{
    [Serializable]
    public class BetRequest : BaseRequest
    {
        public float betAmount;
        public string currency;
        public bool isWithBonus;

        public BetRequest(float betAmount, string currency, bool isWithBonus)
        {
            this.betAmount = betAmount;
            this.currency = currency;
            this.isWithBonus = isWithBonus;
        }
    }
}