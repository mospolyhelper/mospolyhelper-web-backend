namespace Mospolyhelper.Data.Account.Repository
{
    using Mospolyhelper.Data.Account.Remote;
    using Mospolyhelper.Domain.Account.Model;
    using Mospolyhelper.Domain.Account.Repository;
    using Mospolyhelper.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountRepository : IAccountRepository
    {
        private readonly AccountRemoteDataSource remoteDataSource;

        public AccountRepository(AccountRemoteDataSource remoteDataSource)
        {
            this.remoteDataSource = remoteDataSource;
        }

        public Task<Result<string>> GetSessionId(string login, string password, string? sessionId = null)
        {
            return this.remoteDataSource.GetSessionId(login, password, sessionId);
        }

        public Task<Result<IList<string>>> GetPermissions(string sessionId)
        {
            return this.remoteDataSource.GetPermissions(sessionId);
        }

        public Task<Result<Students>> GetPortfolios(string searchQuery, int page)
        {
            return this.remoteDataSource.GetPortfolios(searchQuery, page);
        }

        public Task<Result<AccountTeachers>> GetTeachers(string sessionId, string searchQuery, int page)
        {
            return this.remoteDataSource.GetTeachers(sessionId, searchQuery, page);
        }

        public Task<Result<IList<Application>>> GetApplications(string sessionId)
        {
            return this.remoteDataSource.GetApplications(sessionId);
        }

        public Task<Result<IList<Classmate>>> GetClassmates(string sessionId)
        {
            return this.remoteDataSource.GetClassmates(sessionId);
        }

        public Task<Result<IList<AccountMessage>>> GetDialog(string sessionId, string dialogKey)
        {
            return this.remoteDataSource.GetDialog(sessionId, dialogKey);
        }

        public Task<Result<IList<DialogPreview>>> GetDialogs(string sessionId)
        {
            return this.remoteDataSource.GetDialogs(sessionId);
        }

        public Task<Result<Info>> GetInfo(string sessionId)
        {
            return this.remoteDataSource.GetInfo(sessionId);
        }

        public Task<Result<AccountMarks>> GetMarks(string sessionId)
        {
            return this.remoteDataSource.GetMarks(sessionId);
        }

        public Task<Result<MyPortfolio>> GetMyPortfolio(string sessionId)
        {
            return this.remoteDataSource.GetMyPortfolio(sessionId);
        }

        public Task<Result<MyPortfolio>> SetMyPortfolio(string sessionId, string otherInfo, bool isPublic)
        {
            return this.remoteDataSource.SetMyPortfolio(sessionId, otherInfo, isPublic);
        }

        public Task<Result<IList<AccountMessage>>> SendMessage(
            string sessionId,
            string dialogKey,
            string message,
            IList<string> fileNames
            )
        {
            return this.remoteDataSource.SendMessage(sessionId, dialogKey, message, fileNames);
        }
    }
}
