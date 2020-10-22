using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mospolyhelper.Data.Account.Api
{
    public class AccountClient
    {
        private const string UrlBase = "https://e.mospolytech.ru";
        private const string UrlAuth = UrlBase + "/?p=login";
        private const string UrlInfo = UrlBase + "/?";
        private const string UrlProfile = UrlBase + "/?p=about";
        private const string UrlNotifications = UrlBase + "/?p=alerts";
        private const string UrlMessages = UrlBase + "/?p=messages";
        private const string UrlPayments = UrlBase + "/?p=payments";
        private const string UrlSchedules = UrlBase + "/?p=rasp";
        private const string UrlMarks = UrlBase + "/?p=marks";
        private const string UrlStatement = UrlBase + "/?p=stud_stats";
        private const string UrlProjects = UrlBase + "/?p=projects";
        private const string UrlPhysed = UrlBase + "/?p=phys";
        private const string UrlClassmates = UrlBase + "/?p=group";
        private const string UrlTeachers = UrlBase + "/?p=teachers";
        private const string UrlApplications = UrlBase + "/?p=sprav";
        private const string UrlMyPortfolio = UrlBase + "/?p=myportfolio";
        private const string UrlPortfolio = UrlBase + "/?p=portfolio";

        private readonly HttpClient client;

        public AccountClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> GetSessionId(string login, string password)
        {
            var content = new[]
            {
                new KeyValuePair<string, string>("ulogin", login),
                new KeyValuePair<string, string>("upassword", password),
                new KeyValuePair<string, string>("auth_action", "userlogin")
            };
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(UrlAuth),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(content)
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return string.Join(',',
                response.Headers
                    .SingleOrDefault(header => header.Key == "Set-Cookie").Value
            );
        }

        public async Task<string> GetInfo(string sessionId)
        {
            var url = new Uri(UrlInfo);
            var request = new HttpRequestMessage
            {
                RequestUri = url,
                Method = HttpMethod.Get,
                Headers = { { "Cookie", sessionId } }
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPortfolio()
        {
            var url = new Uri(UrlPortfolio);
            var request = new HttpRequestMessage
            {
                RequestUri = url,
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
