using System;

namespace Server
{
    [Serializable]
    public class AuthRequest : BaseRequest
    {
        public string @operator;
        public string authToken;
        public string currency;

        public AuthRequest(string @operator, string authToken, string currency)
        {
            this.@operator = @operator;
            this.authToken = authToken;
            this.currency = currency;
        }
    }
}