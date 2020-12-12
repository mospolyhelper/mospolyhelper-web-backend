namespace Mospolyhelper.Domain.Account.Model
{
    using System.Collections.Generic;

    public class AccountTeachers
    {
        public AccountTeachers(int pageCount, int currentPage, IList<AccountTeacher> teachers)
        {
            PageCount = pageCount;
            CurrentPage = currentPage;
            Teachers = teachers;
        }

        public int PageCount { get; }
        public int CurrentPage { get; }
        public IList<AccountTeacher> Teachers { get; }
    }
}
