namespace Mospolyhelper.Data.Schedule.ModelDb
{
    using System.ComponentModel.DataAnnotations;


    public class PreferenceDb
    {
        [Key]
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
