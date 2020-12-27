namespace Mospolyhelper.Data.Schedule.ModelDb
{
    public class LessonAuditoriumDb
    {
        public int LessonId { get; set; }
        public LessonDb? Lesson { get; set; }
        public string? AuditoriumKey { get; set; }
        public AuditoriumDb? Auditorium { get; set; }
    }
}
