namespace Mospolyhelper.Data.Map.Api.V0_1
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class MapClient
    {
        private const string Url = "https://raw.githubusercontent.com/mospolyhelper/up-to-date-information/addresses/addresses.json";

        private readonly ILogger logger;
        private readonly HttpClient client;

        public MapClient(ILogger<MapClient> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public async Task<string> GetMap()
        {
            this.logger.LogDebug("GetMap");
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(Url),
                Method = HttpMethod.Get
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}