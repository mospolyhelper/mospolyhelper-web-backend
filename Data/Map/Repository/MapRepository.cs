using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mospolyhelper.Data.Map.Remote;
using Mospolyhelper.Domain.Map.Repository;
using Mospolyhelper.Utils;

namespace Mospolyhelper.Data.Map.Repository
{
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
