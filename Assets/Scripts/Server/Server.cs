using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Server
{
    public class Server : IServer
    {
        
        public event Action<float> BalanceChanged;
        public void InitializeConnection()
        {
            
        }

        public InitialStateResponse GetInitialState()
        {
            throw new NotImplementedException();
        }

        public JumpResponse FirstJump(FirstJumpRequest firstJumpRequest)
        {
            throw new NotImplementedException();
        }

        public CashOutResponse CashOut()
        {
            throw new NotImplementedException();
        }

        public JumpResponse Jump()
        {
            throw new NotImplementedException();
        }
    }
}