using Mospolyhelper.Data.Account.Api;
using Mospolyhelper.Data.Account.Converters;
using Mospolyhelper.Domain.Account.Model;
using Mospolyhelper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Data.Account.Remote
{
    public class AccountRemoteDataSource
    {
        private readonly AccountClient client;
        private readonly AccountConverter converter;

        public AccountRemoteDataSource(AccountClient client, AccountConverter converter)
        {
            this.client = client;
            this.converter = converter;
        }

        private bool CheckAuthorization(string html)
        {
            return !html.Contains("upassword");
        }

        public async Task<Result<string>> GetSessionId(string login, string password, string? sessionId = null)
        {
            try
            {
                return Result<string>.Success(await client.GetSessionId(login, password, sessionId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<string>.Failure(e);
            }
        }

        public async Task<Result<IList<string>>> GetPermissions(string sessionId)
        {
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
                Console.WriteLine(e);
                return Result<IList<string>>.Failure(e);
            }
        }

        public async Task<Result<Students>> GetPortfolios(string searchQuery, int page)
        {
            try
            {
                var res = await client.GetPortfolio(searchQuery, page);
                return Result<Students>.Success(converter.ParsePortfolios(res, page));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<Students>.Failure(e);
            }
        }

        public async Task<Result<AccountTeachers>> GetTeachers(string sessionId, string searchQuery, int page)
        {
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
                Console.WriteLine(e);
                return Result<AccountTeachers>.Failure(e);
            }
        }

        public async Task<Result<Info>> GetInfo(string sessionId)
        {
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
                Console.WriteLine(e);
                return Result<Info>.Failure(e);
            }
        }

        public async Task<Result<AccountMarks>> GetMarks(string sessionId)
        {
            try
            {
                var res = await client.GetMarks(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<AccountMarks>.Failure(new UnauthorizedAccessException());
                }
                return Result<AccountMarks>.Success(converter.ParseMarks(res));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<AccountMarks>.Failure(e);
            }
        }

        public async Task<Result<IList<Application>>> GetApplications(string sessionId)
        {
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
                Console.WriteLine(e);
                return Result<IList<Application>>.Failure(e);
            }
        }

        public async Task<Result<IList<Classmate>>> GetClassmates(string sessionId)
        {
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
                Console.WriteLine(e);
                return Result<IList<Classmate>>.Failure(e);
            }
        }

        public async Task<Result<MyPortfolio>> GetMyPortfolio(string sessionId)
        {
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
                Console.WriteLine(e);
                return Result<MyPortfolio>.Failure(e);
            }
        }

        public async Task<Result<MyPortfolio>> SetMyPortfolio(string sessionId, string otherInfo, bool isPublic)
        {
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
                Console.WriteLine(e);
                return Result<MyPortfolio>.Failure(e);
            }
        }

        public async Task<Result<IList<DialogPreview>>> GetDialogs(string sessionId)
        {
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
                Console.WriteLine(e);
                return Result<IList<DialogPreview>>.Failure(e);
            }
        }

        public async Task<Result<IList<AccountMessage>>> GetDialog(string sessionId, string dialogKey)
        {
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
                Console.WriteLine(e);
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
                Console.WriteLine(e);
                return Result<IList<AccountMessage>>.Failure(e);
            }
        }
    }
}
