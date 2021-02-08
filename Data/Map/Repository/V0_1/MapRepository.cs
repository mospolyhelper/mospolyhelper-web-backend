namespace Mospolyhelper.Data.Map.Repository.V0_1
{
    using System.Threading.Tasks;
    using Domain.Map.Repository;
    using Domain.Map.Repository.V0_1;
    using Microsoft.Extensions.Logging;
    using Remote.V0_1;
    using Utils;

    public class MapRepository : IMapRepository
    {
        private readonly ILogger logger;
        private readonly MapRemoteDataSource remoteDataSource;

        public MapRepository(
            ILogger<MapRepository> logger, 
            MapRemoteDataSource remoteDataSource
            )
        {
            this.logger = logger;
            this.remoteDataSource = remoteDataSource;
        }

        public async Task<Result<string>> GetMap()
        {
            this.logger.LogDebug("GetMap");
            return await remoteDataSource.Get();
        }
    }
}
