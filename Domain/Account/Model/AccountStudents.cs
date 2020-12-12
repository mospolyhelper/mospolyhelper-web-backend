namespace Mospolyhelper.Domain.Account.Model
{
    using System.Collections.Generic;

    public class AccountStudents
    {
        public AccountStudents(int pageCount, int currentPage, IList<AccountPortfolio> portfolios)
        {
            PageCount = pageCount;
            CurrentPage = currentPage;
            Portfolios = portfolios;
        }

        public int PageCount { get; }
        public int CurrentPage { get; }
        public IList<AccountPortfolio> Portfolios { get; }
    }
}
