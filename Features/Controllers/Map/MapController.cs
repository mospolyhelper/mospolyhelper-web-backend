using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mospolyhelper.Domain.Map.UseCase;


namespace Mospolyhelper.Features.Controllers.Map
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class MapController : ControllerBase
    {
        private readonly MapUseCase useCase;

        public MapController(MapUseCase useCase)
        {
            this.useCase = useCase;
        }

        [HttpGet("map")]
        public async Task<ActionResult<Domain.Map.Model.Map>> Get()
        {
            return Content(await useCase.GetMap(), "application/json");
        }
    }
}
