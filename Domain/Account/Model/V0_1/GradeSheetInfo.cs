namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    using System.Collections.Generic;


    public class GradeSheetInfo
    {
        public GradeSheetInfo(
            string id, string guid, string documentType, string examType, 
            string department, string school, string examDate, 
            string examTime, string closeDate, string year, string course, 
            string semester, string @group, string disciplineName, 
            string educationForm, string direction, string directionCode, string specialization, 
            IList<GradeSheetTeacher> teachers, IList<GradeSheetStudent> students,
            bool @fixed, string modifiedDate)
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
            Teachers = teachers;
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
        public IList<GradeSheetTeacher> Teachers { get; set; }
        public IList<GradeSheetStudent> Students { get; set; }
        public bool Fixed { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class GradeSheetTeacher
    {
        public GradeSheetTeacher(string uid, string name, bool signed)
        {
            Uid = uid;
            Name = name;
            Signed = signed;
        }

        public string Uid { get; set; }
        public string Name { get; set; }
        public bool Signed { get; set; }
    }

    public class GradeSheetStudent
    {
        public GradeSheetStudent(string name, string mark, string ticket, string recordBook, bool blocked)
        {
            Name = name;
            Mark = mark;
            Ticket = ticket;
            RecordBook = recordBook;
            Blocked = blocked;
        }

        public string Name { get; set; }
        public string Mark { get; set; }
        public string Ticket { get; set; }
        public string RecordBook { get; set; }
        public bool Blocked { get; set; }
    }
}
