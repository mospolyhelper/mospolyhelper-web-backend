namespace Mospolyhelper.Domain.Account.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GradeSheet
    {
        public GradeSheet(
            string number, 
            string subject, 
            string sheetType, 
            string loadType, 
            string appraisalsDate, 
            string grade, 
            string courseAndSemester
            )
        {
            Number = number;
            Subject = subject;
            SheetType = sheetType;
            LoadType = loadType;
            AppraisalsDate = appraisalsDate;
            Grade = grade;
            CourseAndSemester = courseAndSemester;
        }

        public string Number { get; }
        public string Subject { get; }
        public string SheetType { get; }
        public string LoadType { get; }
        public string AppraisalsDate { get; }
        public string Grade { get; }
        public string CourseAndSemester { get; }
    }
}
