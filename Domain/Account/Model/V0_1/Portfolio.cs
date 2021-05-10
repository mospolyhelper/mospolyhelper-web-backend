namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    public class Portfolio
    {
        public Portfolio(
            int id,
            string name,
            string avatarUrl,
            string group, 
            string direction, 
            string specialization, 
            string course, 
            string educationForm
        )
        {
            Id = id;
            Name = name;
            AvatarUrl = avatarUrl;
            Group = group;
            Direction = direction;
            Specialization = specialization;
            Course = course;
            EducationForm = educationForm;
        }

        public int Id { get; }
        public string Name { get; }
        public string AvatarUrl { get; }
        public string Group { get; }
        public string Direction { get; }
        public string Specialization { get; }
        public string Course { get; }
        public string EducationForm { get; }
    }
}
