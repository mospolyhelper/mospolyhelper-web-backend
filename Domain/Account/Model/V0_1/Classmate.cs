namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    public class Classmate
    {
        public Classmate(int id, string name, string avatarUrl, string status, string dialogKey)
        {
            Id = id;
            Name = name;
            AvatarUrl = avatarUrl;
            Status = status;
            DialogKey = dialogKey;
        }

        public int Id { get; }
        public string Name { get; }
        public string AvatarUrl { get; }
        public string Status { get; }
        public string DialogKey { get; }
    }
}
