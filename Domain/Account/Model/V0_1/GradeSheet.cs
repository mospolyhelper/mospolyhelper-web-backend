namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    public class GradeSheet
    {
        public GradeSheet(
            string id,
            string number, 
            string subject, 
            string sheetType, 
            string loadType, 
            string appraisalsDate, 
            string grade, 
            string courseAndSemester
            )
        {
            Id = id;
            Number = number;
            Subject = subject;
            SheetType = sheetType;
            LoadType = loadType;
            AppraisalsDate = appraisalsDate;
            Grade = grade;
            CourseAndSemester = courseAndSemester;
        }

        public string Id { get; }
        public string Number { get; }
        public string Subject { get; }
        public string SheetType { get; }
        public string LoadType { get; }
        public string AppraisalsDate { get; }
        public string Grade { get; }
        public string CourseAndSemester { get; }
    }
}
