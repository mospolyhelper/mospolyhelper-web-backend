using System;
using System.Text.Json.Serialization;

namespace Mospolyhelper.Domain.Schedule.Model
{
    public class Group
    {
        public static Group Empty { get; } = new Group(string.Empty, false);


        public string Title { get; }
        public bool Evening { get; }

        public Group(string title, bool evening)
        {
            this.Title = title;
            this.Evening = evening;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Group other)
            {
                return Equals(other);
            }
            else
            {
                return false;
            }
        }

        protected bool Equals(Group other)
        {
            return Title == other.Title && Evening == other.Evening;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Evening);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
