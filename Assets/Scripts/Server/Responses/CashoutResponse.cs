using System;

namespace Server
{
    [Serializable]
    public class CashoutResponse : BaseResponse
    {
        public float betAmount;
        public string currency;
        public float winAmount;
        public bool isWin;
    }
}