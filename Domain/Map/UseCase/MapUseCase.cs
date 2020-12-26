using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mospolyhelper.Domain.Map.Repository;
using Mospolyhelper.Utils;

namespace Mospolyhelper.Domain.Map.UseCase
{
    public class MapUseCase
    {
        private readonly ILogger logger;
        private readonly IMapRepository mapRepository;

        public MapUseCase(
            ILogger<MapUseCase> logger, 
            IMapRepository mapRepository
            )
        {
            this.logger = logger;
            this.mapRepository = mapRepository;
        }

        public Task<Result<string>> GetMap()
        {
            return mapRepository.GetMap();
        }
    }
}
