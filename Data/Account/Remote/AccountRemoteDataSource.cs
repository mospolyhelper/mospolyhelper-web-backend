using Mospolyhelper.Data.Account.Api;
using Mospolyhelper.Data.Account.Converters;
using Mospolyhelper.Domain.Account.Model;
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

        public async Task<IList<AccountPortfolio>> GetPortfolios(string searchQuery, int page)
        {
            try
            {
                var portfoliosString = await client.GetPortfolio(searchQuery, page);
                return converter.ParsePortfolios(portfoliosString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Array.Empty<AccountPortfolio>();
            }
        }

        public async Task<IList<AccountTeacher>> GetTeachers(string sessionId, string searchQuery, int page)
        {
            try
            {
                var teachersString = await client.GetTeachers(sessionId, searchQuery, page);
                return converter.ParseTeachers(teachersString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Array.Empty<AccountTeacher>();
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
