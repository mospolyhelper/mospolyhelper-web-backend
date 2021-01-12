namespace Mospolyhelper.Domain.Account.Model
{
    using System.Collections.Generic;

    public class Contract
    {
        public Contract(
            string name, 
            int paidAmount, 
            int debt, 
            string debtDate, 
            int remainingAmount, 
            string expirationDate, 
            IList<Payment> payments, 
            string sberQR
            )
        {
            Name = name;
            PaidAmount = paidAmount;
            Debt = debt;
            DebtDate = debtDate;
            RemainingAmount = remainingAmount;
            ExpirationDate = expirationDate;
            Payments = payments;
            SberQR = sberQR;
        }

        public string Name { get; }
        public int PaidAmount { get; }
        public int Debt { get; }
        public string DebtDate { get; }
        public int RemainingAmount { get; }
        public string ExpirationDate { get; }
        public IList<Payment> Payments { get; }
        public string SberQR { get; }
    }
}
