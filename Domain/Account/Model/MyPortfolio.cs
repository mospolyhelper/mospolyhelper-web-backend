namespace Mospolyhelper.Domain.Account.Model
{
    public class MyPortfolio
    {
        public MyPortfolio(string otherInformation, bool isPublic)
        {
            OtherInformation = otherInformation;
            IsPublic = isPublic;
        }

        public string OtherInformation { get; }
        public bool IsPublic { get; }
    }
}
