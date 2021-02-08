namespace Mospolyhelper.Data.Map.Remote.V0_1
{
    using System;
    using System.Threading.Tasks;
    using Api.V0_1;
    using Microsoft.Extensions.Logging;
    using Utils;

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
