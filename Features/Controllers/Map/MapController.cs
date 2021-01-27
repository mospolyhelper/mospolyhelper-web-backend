namespace Mospolyhelper.Features.Controllers.Map
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Mospolyhelper.Domain.Map.UseCase;

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
