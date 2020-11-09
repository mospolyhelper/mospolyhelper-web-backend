using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Domain.Account.Model
{
    public class AccountMark
    {
        public AccountMark(string subject, string loadType, string mark)
        {
            Subject = subject;
            LoadType = loadType;
            Mark = mark;
        }


        /// <example>Математика</example>
        public string Subject { get; }
        /// <example>Экзамен</example>
        public string LoadType { get; }
        /// <example>Отлично</example>
        public string Mark { get; }
    }
}
