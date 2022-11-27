namespace GameModels
{
    public interface IAccountModel
    {
        float CurrentBalance { get; set; }
        string CurrentCurrency { get; set; }
        void ChangeBalance(float newBalance, string currency);
    }

    public class AccountModel : IAccountModel
    {
        public float CurrentBalance { get; set; }
        public string CurrentCurrency { get; set; }

        public void ChangeBalance(float newBalance, string currency)
        {
            CurrentCurrency = currency;
            CurrentBalance = newBalance;
        }
    }
}