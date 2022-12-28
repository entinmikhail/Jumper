using System;

namespace Server
{
    [Serializable]
    public class GetStateResponse : BaseResponse
    {
        public float betAmount;
        public float bonusBuyK;
        public string currency;
        public bool isWin;
        public bool isWithBonus;
        public Step[] steps;
    }
}