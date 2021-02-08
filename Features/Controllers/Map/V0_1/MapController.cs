namespace Mospolyhelper.Features.Controllers.Map.V0_1
{
    using System.Threading.Tasks;
    using Domain.Map.Model.V0_1;
    using Domain.Map.UseCase.V0_1;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Produces("application/json")]
    [ApiVersion("0.1")]
    [ApiController, Route("[controller]")]
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
        public async Task<ActionResult<Map>> Get()
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
