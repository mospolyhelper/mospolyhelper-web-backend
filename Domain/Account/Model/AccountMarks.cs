using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Domain.Account.Model
{
    public class AccountMarks
    {
        public AccountMarks(IDictionary<string, IDictionary<string, IList<AccountMark>>> marks)
        {
            Marks = marks;
        }

        public IDictionary<string, IDictionary<string, IList<AccountMark>>> Marks { get; }
    }
}
