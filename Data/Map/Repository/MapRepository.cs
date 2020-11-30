using System.Threading.Tasks;
using Mospolyhelper.Data.Map.Remote;
using Mospolyhelper.Domain.Map.Repository;

namespace Mospolyhelper.Data.Map.Repository
{
    public class MapRepository : IMapRepository
    {
        private MapRemoteDataSource remoteDataSource;

        public MapRepository(MapRemoteDataSource remoteDataSource)
        {
            this.remoteDataSource = remoteDataSource;
        }

        public async Task<string> GetMap()
        {
            return await remoteDataSource.Get();
        }

    }
}
