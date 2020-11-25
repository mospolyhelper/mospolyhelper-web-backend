using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Domain.Account.Model
{
    public class AccountClassmate
    {
        public AccountClassmate(int id, string name, string avatarUrl, string status, string dialogKey)
        {
            Id = id;
            Name = name;
            AvatarUrl = avatarUrl;
            Status = status;
            DialogKey = dialogKey;
        }

        public int Id { get; }
        public string Name { get; }
        public string AvatarUrl { get; }
        public string Status { get; }
        public string DialogKey { get; }
    }
}
