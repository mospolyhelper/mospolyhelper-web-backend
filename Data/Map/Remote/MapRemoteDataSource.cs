using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mospolyhelper.Data.Map.Api;
using Mospolyhelper.Utils;

namespace Mospolyhelper.Data.Map.Remote
{
    public class MapRemoteDataSource
    {
        private readonly ILogger logger;
        private readonly MapClient client;

        public MapRemoteDataSource(
            ILogger<MapRemoteDataSource> logger, 
            MapClient client
            )
        {
            this.logger = logger;
            this.client = client;
        }

        public async Task<Result<string>> Get()
        {
            this.logger.LogDebug("Get");
            try
            {
                return Result<string>.Success(await client.GetMap());
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Get");
                return Result<string>.Failure(e);
            }
        }
    }
}
