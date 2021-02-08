namespace Mospolyhelper.Data.Account.Remote.V0_2
{
    using Api.V0_1;
    using Microsoft.Extensions.Logging;
    using Converters.V0_2;
    using Domain.Account.Model.V0_2;
    using Utils;
    using System;
    using System.Threading.Tasks;

    public class AccountRemoteDataSource
    {
        private readonly ILogger logger;
        private readonly AccountClient client;
        private readonly AccountConverter converter;

        public AccountRemoteDataSource(
            ILogger<AccountRemoteDataSource> logger,
            AccountClient client,
            AccountConverter converter
            )
        {
            this.logger = logger;
            this.client = client;
            this.converter = converter;
        }

        private bool CheckAuthorization(string html)
        {
            return !html.Contains("upassword");
        }

        public async Task<Result<UserAuth>> Auth(string login, string password, string? sessionId = null)
        {
            this.logger.LogDebug("Auth");
            try
            {
                var res = await client.GetSessionId(login, password, sessionId);
                var isAuthorized = CheckAuthorization(res.Item2);
                if (!isAuthorized)
                {
                    return Result<UserAuth>.Failure(new UnauthorizedAccessException());
                }
                return Result<UserAuth>.Success(converter.PasrseAuth(res.Item2, res.Item1));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Auth");
                return Result<UserAuth>.Failure(e);
            }
        }
    }
}
