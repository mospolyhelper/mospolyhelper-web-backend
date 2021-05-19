namespace Mospolyhelper.Data.Account.Api.V0_1
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Extensions.Logging;

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
        private const string UrlGradeSheets = UrlBase + "/?p=stud_stats";
        private const string UrlProjects = UrlBase + "/?p=projects";
        private const string UrlPhysed = UrlBase + "/?p=phys";
        private const string UrlClassmates = UrlBase + "/?p=group";
        private const string UrlTeachers = UrlBase + "/?p=teachers";
        private const string UrlApplications = UrlBase + "/?p=sprav";
        private const string UrlMyPortfolio = UrlBase + "/?p=myportfolio";
        private const string UrlPortfolio = UrlBase + "/?p=portfolio";

        private readonly HttpClient client;
        private readonly ILogger logger;

        public AccountClient(ILogger<AccountClient> logger, HttpClient client)
        {
            this.client = client;
            this.logger = logger;
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
                Content = content
            };
            var headers = new Dictionary<string, string>();
            if (sessionId != string.Empty)
            {
                headers["Cookie"] = $"PHPSESSID={sessionId}";
            }
            foreach (var (key, value) in headers)
            {
                request.Headers.Add(key, value);
            }
            return client.SendAsync(request);
        }

        private ByteArrayContent ToUrlEncodedForm(NameValueCollection contentList)
        {
            var data = Encoding.GetEncoding(1251).GetBytes(contentList.ToWindows1251UrlEncodedQuery());
            var content = new ByteArrayContent(data);
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            return content;
        }

        public async Task<(string, string)> GetSessionId(string login, string password, string? sessionId = null)
        {
            this.logger.LogDebug("GetSessionId");
            var postData = new NameValueCollection()
            {
                { "ulogin", login },
                { "upassword", password },
                { "auth_action", "userlogin" }
            };
            var data = Encoding.GetEncoding(1251).GetBytes(postData.ToWindows1251UrlEncodedQuery());
            var content = new ByteArrayContent(data);
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            var response = await GetResponse(new Uri(UrlAuth), HttpMethod.Post, sessionId ?? string.Empty, content);
            response.EnsureSuccessStatusCode();
            var resString = await response.Content.ReadAsStringAsync();
            var resSessionId = GetCookie(response) ?? sessionId ?? string.Empty;
            return (resSessionId, resString);
        }

        private string? GetCookie(HttpResponseMessage message)
        {
            this.logger.LogDebug("GetCookie");
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

        public Task<string> GetPermissions(string sessionId)
        {
            this.logger.LogDebug("GetPermissions");
            return GetResponseString(new Uri(UrlInfo), HttpMethod.Get, sessionId);
        }

        public Task<string> GetInfo(string sessionId)
        {
            this.logger.LogDebug("GetInfo");
            return GetResponseString(new Uri(UrlInfo), HttpMethod.Get, sessionId);
        }

        public Task<string> GetPortfolio(string searchQuery, int page)
        {
            this.logger.LogDebug("GetPortfolio");
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
            this.logger.LogDebug("GetTeachers");
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
            this.logger.LogDebug("GetMarks");
            return GetResponseString(new Uri(UrlMarks), HttpMethod.Get, sessionId);
        }

        public Task<string> GetGradeSheets(string sessionId, string semester)
        {
            this.logger.LogDebug("GetGradeSheets");
            if (semester == string.Empty)
            {
                return GetResponseString(new Uri(UrlGradeSheets), HttpMethod.Get, sessionId);
            }
            else
            {
                var contentList = new NameValueCollection()
                {
                    { "kvartal", semester }
                };
                var content = ToUrlEncodedForm(contentList);

                return GetResponseString(new Uri(UrlGradeSheets), HttpMethod.Post, sessionId, content);
            }
        }

        public Task<string> GetApplications(string sessionId)
        {
            this.logger.LogDebug("GetApplications");
            return GetResponseString(new Uri(UrlApplications), HttpMethod.Get, sessionId);
        }

        public Task<string> GetPayments(string sessionId)
        {
            this.logger.LogDebug("GetPayments");
            return GetResponseString(new Uri(UrlPayments), HttpMethod.Get, sessionId);
        }

        public Task<string> GetClassmates(string sessionId)
        {
            this.logger.LogDebug("GetClassmates");
            return GetResponseString(new Uri(UrlClassmates), HttpMethod.Get, sessionId);
        }

        public Task<string> GetDialogs(string sessionId)
        {
            this.logger.LogDebug("GetDialogs");
            return GetResponseString(new Uri(UrlMessages), HttpMethod.Get, sessionId);
        }

        public Task<string> GetDialog(string sessionId, string dialogKey)
        {
            this.logger.LogDebug("GetDialog");
            var builder = new UriBuilder(UrlMessages);
            var query = HttpUtility.ParseQueryString(builder.Query);
            var dialogParams = dialogKey.Split("&s=");
            query["dlg"] = dialogParams[0];
            if (dialogParams.Length > 1)
            {
                query["s"] = dialogParams[1];
            }
            builder.Query = query.ToString();
            var url = builder.Uri;
            return GetResponseString(url, HttpMethod.Get, sessionId);
        }

        public Task<string> SendMessage(string sessionId, string dialogKey, string message, IList<string> fileNames)
        {
            this.logger.LogDebug("SendMessage");
            var builder = new UriBuilder(UrlMessages);
            var query = HttpUtility.ParseQueryString(builder.Query);
            var dialogParams = dialogKey.Split("&s=");
            query["dlg"] = dialogParams[0];
            if (dialogParams.Length > 1)
            {
                query["s"] = dialogParams[1];
            }
            builder.Query = query.ToString();
            var url = builder.Uri;

            var contentList = new NameValueCollection()
            {
                { "to", dialogKey },
                { "action", "writeto" },
                { "answer", message }
            };
            foreach (var fileName in fileNames)
            {
                contentList.Add("ufile[]", fileName);
            }

            var content = ToUrlEncodedForm(contentList);
            return GetResponseString(url, HttpMethod.Post, sessionId, content);
        }

        public Task<string> RemoveMessage(string sessionId, string dialogAndMessage)
        {
            this.logger.LogDebug("RemoveMessage");
            var builder = new UriBuilder(UrlMessages);
            var query = HttpUtility.ParseQueryString(builder.Query);
            var dialogKeyAndMessageParams = dialogAndMessage.Split("&m=");
            if (dialogKeyAndMessageParams.Length > 1)
            {
                query["dlg"] = dialogKeyAndMessageParams[0];
                query["m"] = dialogKeyAndMessageParams[1];
            }
            builder.Query = query.ToString();
            var url = builder.Uri;
            return GetResponseString(url, HttpMethod.Get, sessionId);
        }

        public Task<string> GetMyPortfolio(string sessionId)
        {
            this.logger.LogDebug("GetMyPortfolio");
            return GetResponseString(new Uri(UrlMyPortfolio), HttpMethod.Get, sessionId);
        }

        public Task<string> SetMyPortfolio(string sessionId, string otherInfo, bool isPublic)
        {
            this.logger.LogDebug("SetMyPortfolio");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("otherinfo", Convert.ToBase64String(Encoding.UTF8.GetBytes(otherInfo))),
                new KeyValuePair<string, string>("acces_otherinfo", isPublic ? "1" : "0"),
                new KeyValuePair<string, string>("action", "save_portfolio")
            });

            return GetResponseString(new Uri(UrlMyPortfolio), HttpMethod.Post, sessionId, content);
        }
    }
}
