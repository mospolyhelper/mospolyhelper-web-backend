using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mospolyhelper.Data.Map.Api
{
    public class MapClient
    {
        private readonly HttpClient client;

        public MapClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> GetMap()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://raw.githubusercontent.com/mospolyhelper/up-to-date-information/addresses/addresses.json"),
                Method = HttpMethod.Get
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}