using System;
using System.Threading.Tasks;
using Mospolyhelper.Data.Map.Api;

namespace Mospolyhelper.Data.Map.Remote
{
    public class MapRemoteDataSource
    {
        private readonly MapClient client;

        public MapRemoteDataSource(MapClient client)
        {
            this.client = client;
        }

        public async Task<string> Get()
        {
            try
            {
                return await client.GetMap();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "{}";
            }
        }
    }
}
