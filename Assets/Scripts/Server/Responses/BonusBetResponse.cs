using System;

namespace Server
{
    [Serializable]
    public class BonusBetResponse : BaseResponse
    {
        public bool isWithBonus;
        public string clientSeed;
        public string serverSeed;
        public int nonce;
        public int startPoint;
        public int step;
        public int betMultiplayer;
    }
}