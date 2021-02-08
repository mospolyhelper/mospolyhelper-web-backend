namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    public class AccountAttachment
    {
        public AccountAttachment(string url, string name)
        {
            Url = url;
            Name = name;
        }

        public string Url { get; }
        public string Name { get; }
    }
}
