using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Domain.Account.Model
{
    public class AccountTeacher
    {
        public AccountTeacher(int id, string name, string info, string avatarUrl, string status, string dialogKey)
        {
            Id = id;
            Name = name;
            Info = info;
            AvatarUrl = avatarUrl;
            Status = status;
            DialogKey = dialogKey;
        }

        public int Id { get; }
        public string Name { get; }
        public string Info { get; }
        public string AvatarUrl { get; }
        public string Status { get; }
        public string DialogKey { get; }
    }
}
