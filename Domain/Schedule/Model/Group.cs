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

        public override string ToString()
        {
            return Title;
        }
    }
}
