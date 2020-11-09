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
                var infoString = await client.GetMarks(sessionId);
                return converter.ParseMarks(infoString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
