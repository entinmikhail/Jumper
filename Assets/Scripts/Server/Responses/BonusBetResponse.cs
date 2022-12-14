using System;

namespace Server
{
    [Serializable]
    public class BonusBetResponse : BaseResponse
    {
        public float betAmount;
        public string currency;
        public bool isWin;
        public bool isWithBonus;
        public Step[] steps;
    }
}