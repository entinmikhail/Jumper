
using System;

namespace GameModels
{
    public interface IAccountModel
    {
        float CurrentBalance { get; set; }
        string CurrentCurrency { get; set; }
        event Action<float, string> BalanceChanged;
        void RefreshBalance(float newBalance, string currency = null);
        void ChangeBalance(float delta);
    }

    public class AccountModel : IAccountModel
    {
        public float CurrentBalance { get; set; }
        public string CurrentCurrency { get; set; }
        public event Action<float, string> BalanceChanged;

        public void RefreshBalance(float newBalance, string currency = null)
        {
            if (currency != null)
                CurrentCurrency = currency;

            CurrentBalance = newBalance;
            BalanceChanged?.Invoke(CurrentBalance, CurrentCurrency);
        }

        public void ChangeBalance(float delta)
        {
            CurrentBalance += delta;
            BalanceChanged?.Invoke(CurrentBalance, CurrentCurrency);
        }
    }
}