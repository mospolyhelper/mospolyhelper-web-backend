namespace Mospolyhelper.Domain.Account.Model.V0_2
{
    public class RefreshRequest
    {
        public RefreshRequest(string expiredAccessToken, string refreshToken)
        {
            ExpiredAccessToken = expiredAccessToken;
            RefreshToken = refreshToken;
        }

        public string ExpiredAccessToken { get; }
        public string RefreshToken { get; }
    }
}
