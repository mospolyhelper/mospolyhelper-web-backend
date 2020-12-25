namespace Mospolyhelper.Domain.Account.Model
{
    using System.Collections.Generic;

    public class Students
    {
        public Students(int pageCount, int currentPage, IList<Portfolio> portfolios)
        {
            PageCount = pageCount;
            CurrentPage = currentPage;
            Portfolios = portfolios;
        }

        public int PageCount { get; }
        public int CurrentPage { get; }
        public IList<Portfolio> Portfolios { get; }
    }
}
