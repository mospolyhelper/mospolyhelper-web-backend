namespace Mospolyhelper.Domain.Account.Repository
{
    using Mospolyhelper.Domain.Account.Model;
    using Mospolyhelper.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAccountRepository
    {
        public Task<Result<string>> GetSessionId(string login, string password, string? sessionId = null);

        public Task<Result<IList<string>>> GetPermissions(string sessionId);

        public Task<Result<Students>> GetPortfolios(string searchQuery, int page);

        public Task<Result<AccountTeachers>> GetTeachers(string sessionId, string searchQuery, int page);

        public Task<Result<Info>> GetInfo(string sessionId);

        public Task<Result<AccountMarks>> GetMarks(string sessionId);

        public Task<Result<IList<Application>>> GetApplications(string sessionId);

        public Task<Result<IList<Classmate>>> GetClassmates(string sessionId);

        public Task<Result<MyPortfolio>> GetMyPortfolio(string sessionId);

        public Task<Result<MyPortfolio>> SetMyPortfolio(string sessionId, string otherInfo, bool isPublic);

        public Task<Result<IList<DialogPreview>>> GetDialogs(string sessionId);

        public Task<Result<IList<AccountMessage>>> GetDialog(string sessionId, string dialogKey);

        public Task<Result<IList<AccountMessage>>> SendMessage(
            string sessionId,
            string dialogKey,
            string message,
            IList<string> fileNames
            );
    }
}
