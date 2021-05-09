namespace Mospolyhelper.Domain.Account.Model.V0_2
{
    public class AuthResponse
    {
        public AuthResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}
