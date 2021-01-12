namespace Mospolyhelper.Domain.Account.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Payments
    {
        public Payments(IDictionary<string, Contract> contracts)
        {
            Contracts = contracts;
        }

        public IDictionary<string, Contract> Contracts { get; }
    }
}
