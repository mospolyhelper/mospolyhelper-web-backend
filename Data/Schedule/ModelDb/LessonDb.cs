namespace Mospolyhelper.Data.Schedule.ModelDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class LessonDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Order { get; set; }
        public DayOfWeek Day { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public ICollection<LessonTeacherDb> LessonTeachers { get; set; } = new List<LessonTeacherDb>();
        public ICollection<LessonAuditoriumDb> LessonAuditoriums { get; set; } = new List<LessonAuditoriumDb>();
        public ICollection<LessonGroupDb> LessonGroups { get; set; } = new List<LessonGroupDb>();
        public DateTime DateFrom { get; set; } = DateTime.MinValue;
        public DateTime DateTo { get; set; } = DateTime.MaxValue;
    }
}
