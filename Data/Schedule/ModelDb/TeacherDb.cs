namespace Mospolyhelper.Data.Schedule.ModelDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TeacherDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IList<LessonTeacherDb> LessonTeachers { get; set; } = new List<LessonTeacherDb>();
    }
}
