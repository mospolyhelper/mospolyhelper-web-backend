namespace Mospolyhelper.Domain.Account.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GradeSheets
    {
        public GradeSheets(string semester, IList<string> semesterList, IList<GradeSheet> sheets)
        {
            Semester = semester;
            SemesterList = semesterList;
            Sheets = sheets;
        }

        public string Semester { get; }
        public IList<string> SemesterList { get; }
        public IList<GradeSheet> Sheets { get; }
    }
}
