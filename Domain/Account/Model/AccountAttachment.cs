namespace Mospolyhelper.Domain.Account.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountAttachment
    {
        public AccountAttachment(string url, string name)
        {
            Url = url;
            Name = name;
        }

        public string Url { get; }
        public string Name { get; }
    }
}
