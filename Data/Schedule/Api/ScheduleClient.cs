using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Mospolyhelper.Utils;

namespace Mospolyhelper.Data.Schedule.Api
{
    public class ScheduleClient
    {
        private const string UrlBase = "https://rasp.dmami.ru";
        private const string UrlGetSchedule = UrlBase + "/site/group";
        private readonly string UrlGetAllSchedules = 
            UrlBase + Secrets.URL_SCHEDULE_ALL;
        private readonly string UrlGetAllSchedulesSession = 
            UrlBase + Secrets.URL_SCHEDULE_SESSION_ALL;
        private const string UrlGetScheduleByTeacher = "https://kaf.dmami.ru/lessons/teacher-html";

        private readonly ILogger logger;
        private readonly HttpClient client;

        public ScheduleClient(ILogger<ScheduleClient> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public async Task<string> GetSchedule(string groupTitle, bool isSession)
        {
            this.logger.LogDebug($"GetSchedule groupTitle = {groupTitle}, isSession = {isSession}");
            var builder = new UriBuilder(UrlGetSchedule);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["group"] = groupTitle;
            query["session"] = isSession ? "1" : "0";
            builder.Query = query.ToString();
            var url = builder.Uri;
            var request = new HttpRequestMessage
            {
                RequestUri = url,
                Method = HttpMethod.Get,
                Headers =
                {
                    { nameof(HttpRequestHeader.Referer), UrlBase },
                    { "X-Requested-With", "XMLHttpRequest" }
                }
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAllSchedules(bool isSession)
        {
            this.logger.LogDebug($"GetAllSchedules isSession = {isSession}");
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(isSession ? UrlGetAllSchedulesSession : UrlGetAllSchedules),
                Method = HttpMethod.Get,
                Headers =
                {
                    { nameof(HttpRequestHeader.Referer), UrlBase },
                    { "X-Requested-With", "XMLHttpRequest" }
                }
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetScheduleByTeacher(string teacherId)
        {
            this.logger.LogDebug($"GetScheduleByTeacher teacherId = {teacherId}");
            var builder = new UriBuilder(UrlGetScheduleByTeacher);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["id"] = teacherId;
            builder.Query = query.ToString();
            var url = builder.Uri;
            var request = new HttpRequestMessage
            {
                RequestUri = url,
                Method = HttpMethod.Get,
                Headers =
                {
                    { nameof(HttpRequestHeader.Referer), UrlBase },
                    { "X-Requested-With", "XMLHttpRequest" }
                }
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
