namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    using System.Collections.Generic;

    public class Payments
    {
        public Payments(IDictionary<string, Contract> contracts)
        {
            Contracts = contracts;
        }

        public IDictionary<string, Contract> Contracts { get; }
    }
}
