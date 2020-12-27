namespace Mospolyhelper.Data.Schedule.ModelDb
{
    public class LessonGroupDb
    {
        public int LessonId { get; set; }
        public LessonDb? Lesson { get; set; }
        public string? GroupKey { get; set; }
        public GroupDb? Group { get; set; }
    }
}
