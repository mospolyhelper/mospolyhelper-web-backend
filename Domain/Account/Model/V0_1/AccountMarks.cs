namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    using System.Collections.Generic;

    public class AccountMarks
    {
        public AccountMarks(IDictionary<string, IDictionary<string, IList<AccountMark>>> marks)
        {
            Marks = marks;
        }

        public IDictionary<string, IDictionary<string, IList<AccountMark>>> Marks { get; }
    }
}
