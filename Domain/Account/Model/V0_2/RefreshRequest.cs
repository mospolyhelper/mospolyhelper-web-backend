namespace Mospolyhelper.Domain.Account.Model.V0_2
{
    public class RefreshRequest
    {
        public string ExpiredAccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
