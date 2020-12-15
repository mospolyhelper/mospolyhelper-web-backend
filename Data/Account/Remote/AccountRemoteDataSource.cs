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
        private AccountClient client;
        private AccountConverter converter;

        public AccountRemoteDataSource(AccountClient client, AccountConverter converter)
        {
            this.client = client;
            this.converter = converter;
        }

        private bool CheckAuthorization(string html)
        {
            return !html.Contains("upassword");
        }

        public async Task<(bool, string?)> GetSessionId(string login, string password, string? sessionId = null)
        {
            try
            {
                return await client.GetSessionId(login, password, sessionId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, null);
            }
        }

        public async Task<AccountStudents> GetPortfolios(string searchQuery, int page)
        {
            try
            {
                var res = await client.GetPortfolio(searchQuery, page);
                return converter.ParsePortfolios(res, page);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new AccountStudents(1, 1, Array.Empty<AccountPortfolio>());
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

        public async Task<Result<AccountInfo>> GetInfo(string sessionId)
        {
            try
            {
                var res = await client.GetInfo(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<AccountInfo>.Failure(new UnauthorizedAccessException());
                }
                return Result<AccountInfo>.Success(converter.ParseInfo(res));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<AccountInfo>.Failure(e);
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

        public async Task<Result<IList<AccountApplication>>> GetApplications(string sessionId)
        {
            try
            {
                var res = await client.GetApplications(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<AccountApplication>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<AccountApplication>>.Success(converter.ParseApplications(res));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<IList<AccountApplication>>.Failure(e);
            }
        }

        public async Task<Result<IList<AccountClassmate>>> GetClassmates(string sessionId)
        {
            try
            {
                var res = await client.GetClassmates(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<AccountClassmate>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<AccountClassmate>>.Success(converter.ParseClassmates(res));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<IList<AccountClassmate>>.Failure(e);
            }
        }

        public async Task<Result<IList<AccountDialogPreview>>> GetDialogs(string sessionId)
        {
            try
            {
                var res = await client.GetDialogs(sessionId);
                var isAuthorized = CheckAuthorization(res);
                if (!isAuthorized)
                {
                    return Result<IList<AccountDialogPreview>>.Failure(new UnauthorizedAccessException());
                }
                return Result<IList<AccountDialogPreview>>.Success(converter.ParseDialogs(res));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<IList<AccountDialogPreview>>.Failure(e);
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
    }
}
