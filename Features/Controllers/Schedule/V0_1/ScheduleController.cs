namespace Mospolyhelper.Features.Controllers.Schedule.V0_1
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Schedule.Model.V0_1;
    using Domain.Schedule.UseCase.V0_1;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Produces("application/json")]
    [ApiVersion("0.1")]
    [ApiController, Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ScheduleUseCase useCase;

        public ScheduleController(
            ILogger<ScheduleController> logger, 
            ScheduleUseCase useCase
            )
        {
            this.logger = logger;
            this.useCase = useCase;
        }

        public class ScheduleFilterQuery
        {
            public IEnumerable<string> Groups { get; set; } = Array.Empty<string>();
            public IEnumerable<string> Teachers { get; set; } = Array.Empty<string>();
            public IEnumerable<string> Auditoriums { get; set; } = Array.Empty<string>();
            public IEnumerable<string> Types { get; set; } = Array.Empty<string>();
            public IEnumerable<string> Titles { get; set; } = Array.Empty<string>();

            public override string ToString()
            {
                return "Groups = " + string.Join(", ", this.Groups) + "; " +
                    "Teachers = " + string.Join(", ", this.Teachers) + "; " +
                    "Auditoriums = " + string.Join(", ", this.Auditoriums) + "; " +
                    "Types = " + string.Join(", ", this.Types) + "; " +
                    "Titles = " + string.Join(", ", this.Titles);
            }
        }

        [HttpGet("schedule")]
        public async Task<ActionResult<Schedule?>> Get([FromQuery] string id)
        {
            this.logger.LogInformation($"GET request /schedule/schedule id={id}");
            return Ok(await useCase.GetSchedule(id));
        }

        [HttpGet("schedule-teacher")]
        public async Task<ActionResult<Schedule?>> GetByTeacher([FromQuery] string id)
        {
            this.logger.LogInformation($"GetByTeacher request /schedule/schedule-teacher id={id}");
            return Ok(await useCase.GetScheduleByTeacher(id));
        }


        [HttpPost("schedule")]
        public async Task<ActionResult<Schedule?>> GetFilteredSchedule(
            [FromBody] ScheduleFilterQuery query
            )
        {
            this.logger.LogInformation($"POST request /schedule/schedule query={query}");
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
            this.logger.LogInformation("GET request /schedule/group-list");
            return Ok(await useCase.GetGroupList());
        }

        [HttpGet("teacher-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetTeacherList()
        {
            this.logger.LogInformation("GET request /schedule/teacher-list");
            return Ok(await useCase.GetTeacherList());
        }

        [HttpGet("auditorium-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetAuditoriumList()
        {
            this.logger.LogInformation("GET request /schedule/auditorium-list");
            return Ok(await useCase.GetAuditoriumList());
        }

        [HttpGet("title-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetTitleList()
        {
            this.logger.LogInformation("GET request /schedule/title-list");
            return Ok(await useCase.GetTitleList());
        }

        [HttpGet("type-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetTypeList()
        {
            this.logger.LogInformation("GET request /schedule/type-list");
            return Ok(await useCase.GetTypeList());
        }
    }
}
