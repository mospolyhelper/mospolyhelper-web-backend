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
            this.logger.LogDebug("GetSessionId");
            return this.accountRepository.GetSessionId(login, password, sessionId);
        }

        public Task<Result<IList<string>>> GetPermissions(string sessionId)
        {
            this.logger.LogDebug("GetPermissions");
            return this.accountRepository.GetPermissions(sessionId);
        }

        public Task<Result<Info>> GetInfo(string sessionId)
        {
            this.logger.LogDebug("GetInfo");
            return this.accountRepository.GetInfo(sessionId);
        }

        public Task<Result<Students>> GetPortfolios(string searchQuery, int page)
        {
            this.logger.LogDebug("GetPortfolios");
            return this.accountRepository.GetPortfolios(searchQuery, page);
        }

        public Task<Result<AccountTeachers>> GetTeachers(string sessionId, string searchQuery, int page)
        {
            this.logger.LogDebug("GetTeachers");
            return this.accountRepository.GetTeachers(sessionId, searchQuery, page);
        }

        public Task<Result<IList<Application>>> GetApplications(string sessionId)
        {
            this.logger.LogDebug("GetApplications");
            return this.accountRepository.GetApplications(sessionId);
        }

        public Task<Result<Payments>> GetPayments(string sessionId)
        {
            this.logger.LogDebug("GetPayments");
            return this.accountRepository.GetPayments(sessionId);
        }

        public Task<Result<IList<Classmate>>> GetClassmates(string sessionId)
        {
            this.logger.LogDebug("GetClassmates");
            return this.accountRepository.GetClassmates(sessionId);
        }

        public Task<Result<AccountMarks>> GetMarks(string sessionId)
        {
            this.logger.LogDebug("GetMarks");
            return this.accountRepository.GetMarks(sessionId);
        }

        public Task<Result<GradeSheets>> GetGradeSheets(string sessionId, string semester)
        {
            this.logger.LogDebug("GetGradeSheets");
            return this.accountRepository.GetGradeSheets(sessionId, semester);
        }

        public Task<Result<MyPortfolio>> GetMyPortfolio(string sessionId)
        {
            this.logger.LogDebug("GetMyPortfolio");
            return this.accountRepository.GetMyPortfolio(sessionId);
        }

        public Task<Result<MyPortfolio>> SetMyPortfolio(string sessionId, string otherInfo, bool isPublic)
        {
            this.logger.LogDebug("SetMyPortfolio");
            return this.accountRepository.SetMyPortfolio(sessionId, otherInfo, isPublic);
        }

        public Task<Result<IList<DialogPreview>>> GetDialogs(string sessionId)
        {
            this.logger.LogDebug("GetDialogs");
            return this.accountRepository.GetDialogs(sessionId);
        }

        public Task<Result<IList<AccountMessage>>> GetDialog(string sessionId, string dialogKey)
        {
            this.logger.LogDebug("GetDialog");
            return this.accountRepository.GetDialog(sessionId, dialogKey);
        }

        public Task<Result<IList<AccountMessage>>> SendMessage(
            string sessionId, 
            string dialogKey, 
            string message, 
            IList<string> fileNames
            )
        {
            this.logger.LogDebug("SendMessage");
            return this.accountRepository.SendMessage(sessionId, dialogKey, message, fileNames);
        }
    }
}
