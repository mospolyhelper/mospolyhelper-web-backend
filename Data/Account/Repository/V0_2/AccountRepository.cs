﻿namespace Mospolyhelper.Data.Account.Repository.V0_2
{
    using Microsoft.Extensions.Logging;
    using Data.Account.Remote.V0_2;
    using Domain.Account.Model.V0_2;
    using Domain.Account.Repository.V0_2;
    using Utils;
    using System.Threading.Tasks;

    public class AccountRepository : IAccountRepository
    {
        private readonly ILogger logger;
        private readonly AccountRemoteDataSource remoteDataSource;

        public AccountRepository(
            ILogger<AccountRepository> logger,
            AccountRemoteDataSource remoteDataSource
            )
        {
            this.logger = logger;
            this.remoteDataSource = remoteDataSource;
        }

        public Task<Result<UserAuth>> Auth(string login, string password, string? sessionId = null)
        {
            this.logger.LogDebug("Auth");
            return this.remoteDataSource.Auth(login, password, sessionId);
        }
    }
}
