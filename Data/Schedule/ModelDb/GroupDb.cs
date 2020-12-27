namespace Mospolyhelper.Data.Schedule.ModelDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class GroupDb
    {
        [Key]
        public string Title { get; set; } = string.Empty;
        public bool Evening { get; set; }

        public IList<LessonGroupDb> LessonGroups { get; set; } = new List<LessonGroupDb>();
    }
}
