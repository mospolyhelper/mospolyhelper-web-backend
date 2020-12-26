namespace Mospolyhelper.Domain.Account.UseCase
{
    using Microsoft.Extensions.Logging;
    using Mospolyhelper.Domain.Account.Model;
    using Mospolyhelper.Domain.Account.Repository;
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

        public Task<Result<string>> GetSessionId(string login, string password, string? sessionId = null)
        {
            return this.accountRepository.GetSessionId(login, password, sessionId);
        }

        public Task<Result<IList<string>>> GetPermissions(string sessionId)
        {
            return this.accountRepository.GetPermissions(sessionId);
        }

        public Task<Result<Info>> GetInfo(string sessionId)
        {
            return this.accountRepository.GetInfo(sessionId);
        }

        public Task<Result<Students>> GetPortfolios(string searchQuery, int page)
        {
            return this.accountRepository.GetPortfolios(searchQuery, page);
        }

        public Task<Result<AccountTeachers>> GetTeachers(string sessionId, string searchQuery, int page)
        {
            return this.accountRepository.GetTeachers(sessionId, searchQuery, page);
        }

        public Task<Result<IList<Application>>> GetApplications(string sessionId)
        {
            return this.accountRepository.GetApplications(sessionId);
        }

        public Task<Result<IList<Classmate>>> GetClassmates(string sessionId)
        {
            return this.accountRepository.GetClassmates(sessionId);
        }

        public Task<Result<AccountMarks>> GetMarks(string sessionId)
        {
            return this.accountRepository.GetMarks(sessionId);
        }

        public Task<Result<MyPortfolio>> GetMyPortfolio(string sessionId)
        {
            return this.accountRepository.GetMyPortfolio(sessionId);
        }

        public Task<Result<MyPortfolio>> SetMyPortfolio(string sessionId, string otherInfo, bool isPublic)
        {
            return this.accountRepository.SetMyPortfolio(sessionId, otherInfo, isPublic);
        }

        public Task<Result<IList<DialogPreview>>> GetDialogs(string sessionId)
        {
            return this.accountRepository.GetDialogs(sessionId);
        }

        public Task<Result<IList<AccountMessage>>> GetDialog(string sessionId, string dialogKey)
        {
            return this.accountRepository.GetDialog(sessionId, dialogKey);
        }

        public Task<Result<IList<AccountMessage>>> SendMessage(
            string sessionId, 
            string dialogKey, 
            string message, 
            IList<string> fileNames
            )
        {
            return this.accountRepository.SendMessage(sessionId, dialogKey, message, fileNames);
        }
    }
}
