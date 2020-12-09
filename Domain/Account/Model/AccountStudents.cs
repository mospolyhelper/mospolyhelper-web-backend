namespace Mospolyhelper.Domain.Account.Model
{
    using System.Collections.Generic;

    public class AccountStudents
    {
        public AccountStudents(int pagesCount, int currentPage, IList<AccountPortfolio> portolios)
        {
            PagesCount = pagesCount;
            CurrentPage = currentPage;
            Portolios = portolios;
        }

        public int PagesCount { get; }
        public int CurrentPage { get; }
        public IList<AccountPortfolio> Portolios { get; }
    }
}
