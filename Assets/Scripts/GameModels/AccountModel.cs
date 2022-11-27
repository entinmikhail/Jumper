namespace GameModels
{
    public class AccountModel
    {
        public double CurrentBalance;
        public string CurrentCurrency;

        public void ChangeBalance(double newBalance, string currency)
        {
            CurrentCurrency = currency;
            CurrentBalance = newBalance;
        }
    }
}