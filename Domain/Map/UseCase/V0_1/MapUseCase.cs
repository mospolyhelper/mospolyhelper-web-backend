namespace Mospolyhelper.Domain.Map.UseCase.V0_1
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Repository.V0_1;
    using Utils;

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
            this.logger.LogDebug("GetMap");
            return mapRepository.GetMap();
        }
    }
}
