namespace Mospolyhelper.Domain.Account.Model.V0_1
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
