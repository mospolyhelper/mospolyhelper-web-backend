namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    using System.Collections.Generic;

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
