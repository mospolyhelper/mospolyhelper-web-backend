namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    using System.Collections.Generic;


    public class GradeSheetInfo
    {
        public GradeSheetInfo(string id, string guid, string documentType, string examType, string department, string school, string examDate, string examTime, string closeDate, string year, string course, string semester, string @group, string disciplineName, string educationForm, string direction, string directionCode, string specialization, IDictionary<string, GradeSheetPps> pps, IDictionary<string, GradeSheetStudents> students, int @fixed, string modifiedDate)
        {
            Id = id;
            Guid = guid;
            DocumentType = documentType;
            ExamType = examType;
            Department = department;
            School = school;
            ExamDate = examDate;
            ExamTime = examTime;
            CloseDate = closeDate;
            Year = year;
            Course = course;
            Semester = semester;
            Group = @group;
            DisciplineName = disciplineName;
            EducationForm = educationForm;
            Direction = direction;
            DirectionCode = directionCode;
            Specialization = specialization;
            Pps = pps;
            Students = students;
            Fixed = @fixed;
            ModifiedDate = modifiedDate;
        }

        public string Id { get; set; }
        public string Guid { get; set; }
        public string DocumentType { get; set; }
        public string ExamType { get; set; }
        public string Department { get; set; }
        public string School { get; set; }
        public string ExamDate { get; set; }
        public string ExamTime { get; set; }
        public string CloseDate { get; set; }
        public string Year { get; set; }
        public string Course { get; set; }
        public string Semester { get; set; }
        public string Group { get; set; }
        public string DisciplineName { get; set; }
        public string EducationForm { get; set; }
        public string Direction { get; set; }
        public string DirectionCode { get; set; }
        public string Specialization { get; set; }
        public IDictionary<string, GradeSheetPps> Pps { get; set; }
        public IDictionary<string, GradeSheetStudents> Students { get; set; }
        public int Fixed { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class GradeSheetPps
    {
        public GradeSheetPps(string uid, string fio, int signed)
        {
            Uid = uid;
            Fio = fio;
            Signed = signed;
        }

        public string Uid { get; set; }
        public string Fio { get; set; }
        public int Signed { get; set; }
    }

    public class GradeSheetStudents
    {
        public GradeSheetStudents(string name, string mark, string ticket, string recordBook, int canChange)
        {
            Name = name;
            Mark = mark;
            Ticket = ticket;
            RecordBook = recordBook;
            CanChange = canChange;
        }

        public string Name { get; set; }
        public string Mark { get; set; }
        public string Ticket { get; set; }
        public string RecordBook { get; set; }
        public int CanChange { get; set; }
    }
}
