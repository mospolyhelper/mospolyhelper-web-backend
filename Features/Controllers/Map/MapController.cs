namespace Mospolyhelper.Features.Controllers.Map
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Mospolyhelper.Domain.Map.UseCase;

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class MapController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly MapUseCase useCase;

        public MapController(
            ILogger<MapController> logger, 
            MapUseCase useCase
            )
        {
            this.logger = logger;
            this.useCase = useCase;
        }

        [HttpGet("map")]
        public async Task<ActionResult<Domain.Map.Model.Map>> Get()
        {
            this.logger.LogInformation("GET request /map/map");
            var res = await useCase.GetMap();
            if (res.IsSuccess)
            {
                return Content(res.GetOrNull(), "application/json");
            }
            else if (res.IsFailure)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }
    }
}
