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
                var portfoliosString = await client.GetPortfolio(searchQuery, page);
                return converter.ParsePortfolios(portfoliosString, page);
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
                var teachersString = await client.GetTeachers(sessionId, searchQuery, page);
                var isAuthorized = CheckAuthorization(teachersString);
                if (!isAuthorized)
                {
                    return Result<AccountTeachers>.Failure(new UnauthorizedAccessException());
                }
                return Result<AccountTeachers>.Success(converter.ParseTeachers(teachersString, page)); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<AccountTeachers>.Failure(e);
            }
        }

        public async Task<AccountInfo?> GetInfo(string sessionId)
        {
            try
            {
                var infoString = await client.GetInfo(sessionId);
                return converter.ParseInfo(infoString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<AccountMarks?> GetMarks(string sessionId)
        {
            try
            {
                var marksString = await client.GetMarks(sessionId);
                return converter.ParseMarks(marksString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IList<AccountApplication>> GetApplications(string sessionId)
        {
            try
            {
                var applicationsString = await client.GetApplications(sessionId);
                return converter.ParseApplications(applicationsString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Array.Empty<AccountApplication>();
            }
        }

        public async Task<IList<AccountClassmate>> GetClassmates(string sessionId)
        {
            try
            {
                var classmateString = await client.GetClassmates(sessionId);
                return converter.ParseClassmates(classmateString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Array.Empty<AccountClassmate>();
            }
        }

        public async Task<IList<AccountDialogPreview>> GetDialogs(string sessionId)
        {
            try
            {
                var dialogsString = await client.GetDialogs(sessionId);
                return converter.ParseDialogs(dialogsString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Array.Empty<AccountDialogPreview>();
            }
        }

        public async Task<IList<AccountMessage>> GetDialog(string sessionId, string dialogKey)
        {
            try
            {
                var dialogString = await client.GetDialog(sessionId, dialogKey);
                return converter.ParseDialog(dialogString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Array.Empty<AccountMessage>();
            }
        }
    }
}
