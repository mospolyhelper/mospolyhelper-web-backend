namespace Mospolyhelper.Data.Account.Remote.V0_1
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Api.V0_1;
    using Converters.V0_1;
    using Domain.Account.Model;
    using Domain.Account.Model.V0_1;
    using Utils;

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

        public async Task<Result<string>> GetSessionId(string login, string password, string? sessionId = null)
        {
            this.logger.LogDebug("GetSessionId");
            try
            {
                var res = await client.GetSessionId(login, password, sessionId);
                var isAuthorized = CheckAuthorization(res.Item2);
                if (!isAuthorized)
                {
                    return Result<string>.Failure(new UnauthorizedAccessException());
                }
                return Result<string>.Success(res.Item1);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetSessionId");
                return Result<string>.Failure(e);
            }
        }

        public async Task<Result<IList<string>>> GetPermissions(string sessionId)
        {
            this.logger.LogDebug("GetPermissions");
            try
            {
                var res = await client.GetPermissions(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<string>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<string>>.Success(converter.ParsePermissions(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetPermissions");
                return Result<IList<string>>.Failure(e);
            }
        }

        public async Task<Result<Students>> GetPortfolios(string searchQuery, int page)
        {
            this.logger.LogDebug("GetPortfolios");
            try
            {
                var res = await client.GetPortfolio(searchQuery, page);
                return Result<Students>.Success(converter.ParsePortfolios(res, page));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetPortfolios");
                return Result<Students>.Failure(e);
            }
        }

        public async Task<Result<AccountTeachers>> GetTeachers(string sessionId, string searchQuery, int page)
        {
            this.logger.LogDebug("GetTeachers");
            try
            {
                var res = await client.GetTeachers(sessionId, searchQuery, page);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<AccountTeachers>.Failure(new UnauthorizedAccessException());
                }
                return Result<AccountTeachers>.Success(converter.ParseTeachers(res, page)); 
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetTeachers");
                return Result<AccountTeachers>.Failure(e);
            }
        }

        public async Task<Result<Info>> GetInfo(string sessionId)
        {
            this.logger.LogDebug("GetInfo");
            try
            {
                var res = await client.GetInfo(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<Info>.Failure(new UnauthorizedAccessException());
                }
                return Result<Info>.Success(converter.ParseInfo(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetInfo");
                return Result<Info>.Failure(e);
            }
        }

        public async Task<Result<AccountMarks>> GetMarks(string sessionId)
        {
            this.logger.LogDebug("GetMarks");
            try
            {
                var res = await client.GetMarks(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<AccountMarks>.Failure(new UnauthorizedAccessException());
                }
                return Result<AccountMarks>.Success(
                    converter.ParseMarks(res) ?? 
                    new AccountMarks(new Dictionary<string, IDictionary<string, IList<AccountMark>>>())
                    );
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetMarks");
                return Result<AccountMarks>.Failure(e);
            }
        }

        public async Task<Result<GradeSheets>> GetGradeSheets(string sessionId, string semester)
        {
            this.logger.LogDebug("GetGradeSheets");
            try
            {
                var res = await client.GetGradeSheets(sessionId, semester);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<GradeSheets>.Failure(new UnauthorizedAccessException());
                }
                return Result<GradeSheets>.Success(
                    converter.ParseGradeSheets(res) ??
                    new GradeSheets("Нет", Array.Empty<string>(), Array.Empty<GradeSheet>())
                    );
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetGradeSheets");
                return Result<GradeSheets>.Failure(e);
            }
        }

        public async Task<Result<IList<Application>>> GetApplications(string sessionId)
        {
            this.logger.LogDebug("GetApplications");
            try
            {
                var res = await client.GetApplications(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<Application>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<Application>>.Success(converter.ParseApplications(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetApplications");
                return Result<IList<Application>>.Failure(e);
            }
        }

        public async Task<Result<Payments>> GetPayments(string sessionId)
        {
            this.logger.LogDebug("GetPayments");
            try
            {
                var res = await client.GetPayments(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<Payments>.Failure(new UnauthorizedAccessException());
                }
                return Result<Payments>.Success(converter.ParsePayments(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetApplications");
                return Result<Payments>.Failure(e);
            }
        }

        public async Task<Result<IList<Classmate>>> GetClassmates(string sessionId)
        {
            this.logger.LogDebug("GetClassmates");
            try
            {
                var res = await client.GetClassmates(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<Classmate>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<Classmate>>.Success(converter.ParseClassmates(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetClassmates");
                return Result<IList<Classmate>>.Failure(e);
            }
        }

        public async Task<Result<MyPortfolio>> GetMyPortfolio(string sessionId)
        {
            this.logger.LogDebug("GetMyPortfolio");
            try
            {
                var res = await client.GetMyPortfolio(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<MyPortfolio>.Failure(new UnauthorizedAccessException());
                }
                return Result<MyPortfolio>.Success(converter.ParseMyPortfolio(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetMyPortfolio");
                return Result<MyPortfolio>.Failure(e);
            }
        }

        public async Task<Result<MyPortfolio>> SetMyPortfolio(string sessionId, string otherInfo, bool isPublic)
        {
            this.logger.LogDebug("SetMyPortfolio");
            try
            {
                var res = await client.SetMyPortfolio(sessionId, otherInfo, isPublic);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<MyPortfolio>.Failure(new UnauthorizedAccessException());
                }
                return Result<MyPortfolio>.Success(converter.ParseMyPortfolio(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "SetMyPortfolio");
                return Result<MyPortfolio>.Failure(e);
            }
        }

        public async Task<Result<IList<DialogPreview>>> GetDialogs(string sessionId)
        {
            this.logger.LogDebug("GetDialogs");
            try
            {
                var res = await client.GetDialogs(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<DialogPreview>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<DialogPreview>>.Success(converter.ParseDialogs(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetDialogs");
                return Result<IList<DialogPreview>>.Failure(e);
            }
        }

        public async Task<Result<IList<AccountMessage>>> GetDialog(string sessionId, string dialogKey)
        {
            this.logger.LogDebug("GetDialog");
            try
            {
                var res = await client.GetDialog(sessionId, dialogKey);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<AccountMessage>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<AccountMessage>>.Success(converter.ParseDialog(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "GetDialog");
                return Result<IList<AccountMessage>>.Failure(e);
            }
        }

        public async Task<Result<IList<AccountMessage>>> SendMessage(
            string sessionId,
            string dialogKey,
            string message,
            IList<string> fileNames
            )
        {
            this.logger.LogDebug("SendMessage");
            try
            {
                var res = await this.client.SendMessage(sessionId, dialogKey, message, fileNames);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<AccountMessage>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<AccountMessage>>.Success(converter.ParseDialog(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "SendMessage");
                return Result<IList<AccountMessage>>.Failure(e);
            }
        }

        public async Task<Result<IList<AccountMessage>>> RemoveMessage(
            string sessionId,
            string dialogAndMessage
        )
        {
            this.logger.LogDebug("RemoveMessage");
            try
            {
                var res = await this.client.RemoveMessage(sessionId, dialogAndMessage);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<AccountMessage>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<AccountMessage>>.Success(converter.ParseDialog(res));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "RemoveMessage");
                return Result<IList<AccountMessage>>.Failure(e);
            }
        }
    }
}
