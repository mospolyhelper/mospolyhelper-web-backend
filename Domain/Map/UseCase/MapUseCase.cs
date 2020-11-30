using System.Threading.Tasks;
using Mospolyhelper.Domain.Map.Repository;

namespace Mospolyhelper.Domain.Map.UseCase
{
    public class MapUseCase
    {
        private IMapRepository mapRepository;

        public MapUseCase(IMapRepository mapRepository)
        {
            this.mapRepository = mapRepository;
        }

        public Task<string> GetMap()
        {
            return mapRepository.GetMap();
        }

    }
}
