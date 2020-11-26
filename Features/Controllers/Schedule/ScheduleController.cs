using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mospolyhelper.Domain.Schedule.UseCase;


namespace Mospolyhelper.Features.Controllers.Schedule
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleUseCase useCase;

        public ScheduleController(ScheduleUseCase useCase)
        {
            this.useCase = useCase;
        }

        [HttpGet("schedule")]
        public async Task<ActionResult<Domain.Schedule.Model.Schedule?>> Get([FromQuery] string id)
        {
            return Ok(await useCase.GetSchedule(id));
        }

        public class ScheduleFilterQuery
        {
            public IEnumerable<string> Groups { get; set; } = Array.Empty<string>();
            public IEnumerable<string> Teachers { get; set; } = Array.Empty<string>();
            public IEnumerable<string> Auditoriums { get; set; } = Array.Empty<string>();
            public IEnumerable<string> Types { get; set; } = Array.Empty<string>();
            public IEnumerable<string> Titles { get; set; } = Array.Empty<string>();
        }

        [HttpPost("schedule")]
        public async Task<ActionResult<Domain.Schedule.Model.Schedule?>> GetFilteredSchedule([FromBody] ScheduleFilterQuery query)
        {
            return Ok(
                await useCase.GetSchedule(
                    query.Groups, 
                    query.Teachers, 
                    query.Auditoriums, 
                    query.Types, 
                    query.Titles
                    )
            );
        }


        [HttpGet("group-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetGroupList()
        {
            return Ok(await useCase.GetGroupList());
        }

        [HttpGet("teacher-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetTeacherList()
        {
            return Ok(await useCase.GetTeacherList());
        }

        [HttpGet("auditorium-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetAuditoriumList()
        {
            return Ok(await useCase.GetAuditoriumList());
        }

        [HttpGet("title-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetTitleList()
        {
            return Ok(await useCase.GetTitleList());
        }

        [HttpGet("type-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetTypeList()
        {
            return Ok(await useCase.GetTypeList());
        }
    }
}
