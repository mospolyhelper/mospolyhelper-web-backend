namespace Mospolyhelper.Domain.Account.Repository.V0_1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.V0_1;
    using Utils;

    public interface IAccountRepository
    {
        public Task<Result<string>> GetSessionId(string login, string password, string? sessionId = null);

        public Task<Result<IList<string>>> GetPermissions(string sessionId);

        public Task<Result<Students>> GetPortfolios(string searchQuery, int page);

        public Task<Result<AccountTeachers>> GetTeachers(string sessionId, string searchQuery, int page);

        public Task<Result<Info>> GetInfo(string sessionId);

        public Task<Result<AccountMarks>> GetMarks(string sessionId);

        public Task<Result<GradeSheets>> GetGradeSheets(string sessionId, string semester);

        public Task<Result<GradeSheetInfo>> GetGradeSheetInfo(string sessionId, string guid);

        public Task<Result<IList<GradeSheetMark>>> GetGradeSheetAllMarks(string sessionId, string guid);

        public Task<Result<IList<Application>>> GetApplications(string sessionId);

        public Task<Result<Payments>> GetPayments(string sessionId);

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

        public Task<Result<IList<AccountMessage>>> RemoveMessage(
            string sessionId,
            string dialogAndMessage
        );
    }
}
