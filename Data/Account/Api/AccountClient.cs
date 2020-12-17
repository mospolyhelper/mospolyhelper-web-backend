using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mospolyhelper.Data.Account.Api
{
    public static class NameValueCollectionExtension
    {
        public static string ToWindows1251UrlEncodedQuery(this NameValueCollection nv)
        {
            StringBuilder sb = new StringBuilder();
            bool firstIteration = true;
            foreach (var key in nv.AllKeys)
            {
                if (!firstIteration)
                    sb.Append("&");
                sb.Append(HttpUtility.UrlEncode(key, Encoding.GetEncoding(1251)))
                    .Append("=")
                    .Append(HttpUtility.UrlEncode(nv[key], Encoding.GetEncoding(1251)));
                firstIteration = false;
            }
            return sb.ToString();
        }
    }

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

        private async Task<string> GetResponseString(Uri url, HttpMethod method, string sessionId = "", HttpContent? content = null)
        {
            var response = await GetResponse(url, method, sessionId, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private Task<HttpResponseMessage> GetResponse(Uri url, HttpMethod method, string sessionId = "", HttpContent? content = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = url,
                Method = method,
                Headers = { { "Cookie", $"PHPSESSID={sessionId}" } },
                Content = content
            };
            return client.SendAsync(request);
        }

        public async Task<(bool, string?)> GetSessionId(string login, string password, string? sessionId = null)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("ulogin", login),
                new KeyValuePair<string, string>("upassword", password),
                new KeyValuePair<string, string>("auth_action", "userlogin")
            });

            var response = await GetResponse(new Uri(UrlAuth), HttpMethod.Post, sessionId ?? string.Empty, content);
            response.EnsureSuccessStatusCode();
            var resString = await response.Content.ReadAsStringAsync();
            var resSessionId = GetCookie(response) ?? string.Empty;
            if (resString.Contains("upassword"))
            {
                return (false, resSessionId);
            }
            return (true, resSessionId);
        }

        private string? GetCookie(HttpResponseMessage message)
        {
            try
            {
                message.Headers.TryGetValues("Set-Cookie", out var setCookie);
                var setCookieString = setCookie.Single();
                var cookieTokens = setCookieString.Split(';');
                var firstCookie = cookieTokens.FirstOrDefault();
                var keyValueTokens = firstCookie.Split('=');
                var valueString = keyValueTokens[1];
                var cookieValue = HttpUtility.UrlDecode(valueString);
                return cookieValue;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<string> GetInfo(string sessionId)
        {
            return GetResponseString(new Uri(UrlInfo), HttpMethod.Get, sessionId);
        }

        public Task<string> GetPortfolio(string searchQuery, int page)
        {
            var builder = new UriBuilder(UrlPortfolio);
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (searchQuery != string.Empty)
            {
                query["objsearch"] = searchQuery;
            }
            if (page != 0)
            {
                query["pg"] = page.ToString();
            }
            builder.Query = query.ToWindows1251UrlEncodedQuery();
            var url = builder.Uri;
            return GetResponseString(url, HttpMethod.Get);
        }

        public Task<string> GetTeachers(string sessionId, string searchQuery, int page)
        {
            var builder = new UriBuilder(UrlTeachers);
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (searchQuery != string.Empty)
            {
                query["objsearch"] = searchQuery;
            }
            if (page != 0)
            {
                query["pg"] = page.ToString();
            }
            builder.Query = query.ToWindows1251UrlEncodedQuery();
            var url = builder.Uri;
            return GetResponseString(url, HttpMethod.Get, sessionId);
        }

        public Task<string> GetMarks(string sessionId)
        {
            return GetResponseString(new Uri(UrlMarks), HttpMethod.Get, sessionId);
        }

        public Task<string> GetApplications(string sessionId)
        {
            return GetResponseString(new Uri(UrlApplications), HttpMethod.Get, sessionId);
        }

        public Task<string> GetClassmates(string sessionId)
        {
            return GetResponseString(new Uri(UrlClassmates), HttpMethod.Get, sessionId);
        }

        public Task<string> GetDialogs(string sessionId)
        {
            return GetResponseString(new Uri(UrlMessages), HttpMethod.Get, sessionId);
        }

        public Task<string> GetDialog(string sessionId, string dialogKey)
        {
            var builder = new UriBuilder(UrlMessages);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["dlg"] = dialogKey;
            builder.Query = query.ToString();
            var url = builder.Uri;
            return GetResponseString(url, HttpMethod.Get, sessionId);
        }

        public void SendMessage(string sessionId, string dialogKey, string message)
        {

        }

        public Task<string> GetMyPortfolio(string sessionId)
        {
            return GetResponseString(new Uri(UrlMyPortfolio), HttpMethod.Get, sessionId);
        }

        public Task<string> SetMyPortfolio(string sessionId, string otherInfo, bool isPublic)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("otherinfo", Convert.ToBase64String(Encoding.UTF8.GetBytes(otherInfo))),
                new KeyValuePair<string, string>("acces_otherinfo", isPublic ? "1" : "0"),
                new KeyValuePair<string, string>("action", "save_portfolio")
            });

            return GetResponseString(new Uri(UrlMyPortfolio), HttpMethod.Post, sessionId, formContent);
        }
    }
}
