namespace Mospolyhelper.Domain.Account.UseCase.V0_2
{
    using Microsoft.Extensions.Logging;
    using Mospolyhelper.Domain.Account.Model.V0_2;
    using Mospolyhelper.Domain.Account.Repository.V0_2;
    using Mospolyhelper.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountUseCase
    {
        private readonly ILogger logger;
        private readonly IAccountRepository accountRepository;

        public AccountUseCase(
            ILogger<AccountUseCase> logger,
            IAccountRepository accountRepository
            )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
        }

        public Task<Result<UserAuth>> Auth(string login, string password, string? sessionId = null)
        {
            this.logger.LogDebug("Auth");
            return this.accountRepository.Auth(login, password, sessionId);
        }
    }
}
