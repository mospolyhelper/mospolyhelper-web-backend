﻿namespace Mospolyhelper.Domain.Account.Model
{
    using System.Collections.Generic;

    public class AccountStudents
    {
        public AccountStudents(int pageCount, int currentPage, IList<AccountPortfolio> portolios)
        {
            PageCount = pageCount;
            CurrentPage = currentPage;
            Portolios = portolios;
        }

        public int PageCount { get; }
        public int CurrentPage { get; }
        public IList<AccountPortfolio> Portolios { get; }
    }
}