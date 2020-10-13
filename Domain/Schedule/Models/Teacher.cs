using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Mospolyhelper.Domain.Schedule.Models
{
    public class Teacher
    {
        public static Teacher FromFullName(string fullName)
        {
            return new Teacher(
                new StringBuilder(fullName)
                    .Replace(" - ", "-")
                    .Replace(" -", "-")
                    .Replace(" -", "-")
                    .ToString()
                    .Split(' ', '.', StringSplitOptions.RemoveEmptyEntries)
                    .Where(it => string.IsNullOrWhiteSpace(it) || it != string.Empty)
                    .ToArray()
            );
        }

        public IList<string> Names { get; }

        [JsonIgnore]
        public string FullName => string.Join(' ', Names);

        public Teacher(IList<string> names)
        {
            this.Names = names;
        }
    }
}
