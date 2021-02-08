namespace Mospolyhelper.Data.Account.Repository.V0_1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Account.Model;
    using Domain.Account.Model.V0_1;
    using Domain.Account.Repository;
    using Domain.Account.Repository.V0_1;
    using Microsoft.Extensions.Logging;
    using Remote.V0_1;
    using Utils;

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

        public Task<Result<string>> GetSessionId(string login, string password, string? sessionId = null)
        {
            this.logger.LogDebug("GetSessionId");
            return this.remoteDataSource.GetSessionId(login, password, sessionId);
        }

        public Task<Result<IList<string>>> GetPermissions(string sessionId)
        {
            this.logger.LogDebug("GetPermissions");
            return this.remoteDataSource.GetPermissions(sessionId);
        }

        public Task<Result<Students>> GetPortfolios(string searchQuery, int page)
        {
            this.logger.LogDebug("GetPortfolios");
            return this.remoteDataSource.GetPortfolios(searchQuery, page);
        }

        public Task<Result<AccountTeachers>> GetTeachers(string sessionId, string searchQuery, int page)
        {
            this.logger.LogDebug("GetTeachers");
            return this.remoteDataSource.GetTeachers(sessionId, searchQuery, page);
        }

        public Task<Result<IList<Application>>> GetApplications(string sessionId)
        {
            this.logger.LogDebug("GetApplications");
            return this.remoteDataSource.GetApplications(sessionId);
        }

        public Task<Result<Payments>> GetPayments(string sessionId)
        {
            this.logger.LogDebug("GetPayments");
            return this.remoteDataSource.GetPayments(sessionId);
        }

        public Task<Result<IList<Classmate>>> GetClassmates(string sessionId)
        {
            this.logger.LogDebug("GetClassmates");
            return this.remoteDataSource.GetClassmates(sessionId);
        }

        public Task<Result<IList<AccountMessage>>> GetDialog(string sessionId, string dialogKey)
        {
            this.logger.LogDebug("GetDialog");
            return this.remoteDataSource.GetDialog(sessionId, dialogKey);
        }

        public Task<Result<IList<DialogPreview>>> GetDialogs(string sessionId)
        {
            this.logger.LogDebug("GetDialogs");
            return this.remoteDataSource.GetDialogs(sessionId);
        }

        public Task<Result<Info>> GetInfo(string sessionId)
        {
            this.logger.LogDebug("GetInfo");
            return this.remoteDataSource.GetInfo(sessionId);
        }

        public Task<Result<AccountMarks>> GetMarks(string sessionId)
        {
            this.logger.LogDebug("GetMarks");
            return this.remoteDataSource.GetMarks(sessionId);
        }

        public Task<Result<GradeSheets>> GetGradeSheets(string sessionId, string semester)
        {
            this.logger.LogDebug("GetGradeSheets");
            return this.remoteDataSource.GetGradeSheets(sessionId, semester);
        }

        public Task<Result<MyPortfolio>> GetMyPortfolio(string sessionId)
        {
            this.logger.LogDebug("GetMyPortfolio");
            return this.remoteDataSource.GetMyPortfolio(sessionId);
        }

        public Task<Result<MyPortfolio>> SetMyPortfolio(string sessionId, string otherInfo, bool isPublic)
        {
            this.logger.LogDebug("SetMyPortfolio");
            return this.remoteDataSource.SetMyPortfolio(sessionId, otherInfo, isPublic);
        }

        public Task<Result<IList<AccountMessage>>> SendMessage(
            string sessionId,
            string dialogKey,
            string message,
            IList<string> fileNames
            )
        {
            this.logger.LogDebug("SendMessage");
            return this.remoteDataSource.SendMessage(sessionId, dialogKey, message, fileNames);
        }
    }
}
