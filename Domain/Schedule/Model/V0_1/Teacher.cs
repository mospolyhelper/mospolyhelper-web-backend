namespace Mospolyhelper.Domain.Schedule.Model.V0_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;

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

        public override bool Equals(object? obj)
        {
            if (obj is Teacher other)
            {
                return Equals(other);
            }
            else
            {
                return false;
            }
        }

        protected bool Equals(Teacher other)
        {
            return Names.SequenceEqual(other.Names);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 19;
                foreach (var name in Names)
                {
                    hash = hash * 31 + name.GetHashCode();
                }
                return hash;
            }
        }
    }
}
