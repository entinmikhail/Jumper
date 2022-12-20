using System;

namespace Server
{
    [Serializable]
    public class NewJumpResponse : BaseResponse
    {
        public float betAmount;
        public string currency;
        public bool isWin;
        public bool isWithBonus;
        public NewStep[] steps;
    }
}