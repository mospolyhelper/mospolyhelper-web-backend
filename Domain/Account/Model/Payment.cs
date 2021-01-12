namespace Mospolyhelper.Domain.Account.Model
{
    public class Payment
    {
        public Payment(string date, int amount)
        {
            Date = date;
            Amount = amount;
        }

        public string Date { get; }
        public int Amount { get; }
    }
}
