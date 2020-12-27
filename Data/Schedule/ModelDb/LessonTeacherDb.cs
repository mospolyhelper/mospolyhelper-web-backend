namespace Mospolyhelper.Data.Schedule.ModelDb
{
    public class LessonTeacherDb
    {
        public int LessonId { get; set; }
        public LessonDb? Lesson { get; set; }
        public int TeacherId { get; set; }
        public TeacherDb? Teacher { get; set; }
    }
}
