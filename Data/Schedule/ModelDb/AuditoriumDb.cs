namespace Mospolyhelper.Data.Schedule.ModelDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AuditoriumDb
    {
        [Key]
        public string Title { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public IList<LessonAuditoriumDb> LessonAuditoriums { get; set; } = new List<LessonAuditoriumDb>();
    }
}
