using System;

namespace Server
{
    [Serializable]
    public class GetStateResponse : BaseResponse
    {
        public float betAmount;
        public string currency;
        public bool isWin;
        public bool isWithBonus;
        public NewStep[] steps;
    }
}