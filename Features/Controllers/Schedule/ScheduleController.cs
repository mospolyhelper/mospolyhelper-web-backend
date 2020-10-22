using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mospolyhelper.Data.Schedule.Api;
using Mospolyhelper.Data.Schedule.Converters;
using Mospolyhelper.Data.Schedule.Remote;
using Mospolyhelper.Data.Schedule.Repository;
using Mospolyhelper.Domain.Schedule.UseCase;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mospolyhelper.Features.Controllers.Schedule
{
    [Route("[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        // TODO: Make injecting
        private readonly ScheduleUseCase useCase;

        public ScheduleController(ScheduleUseCase useCase)
        {
            this.useCase = useCase;
        }

        [HttpGet("schedule")]
        public async Task<IActionResult> Get([FromQuery] string id)
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
        public async Task<IActionResult> GetFilteredSchedule([FromBody] ScheduleFilterQuery query)
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
        public async Task<IActionResult> GetGroupList()
        {
            return Ok(await useCase.GetGroupList());
        }

        [HttpGet("teacher-list")]
        public async Task<IActionResult> GetTeacherList()
        {
            return Ok(await useCase.GetTeacherList());
        }

        [HttpGet("auditorium-list")]
        public async Task<IActionResult> GetAuditoriumList()
        {
            return Ok(await useCase.GetAuditoriumList());
        }

        [HttpGet("title-list")]
        public async Task<IActionResult> GetTitleList()
        {
            return Ok(await useCase.GetTitleList());
        }

        [HttpGet("type-list")]
        public async Task<IActionResult> GetTypeList()
        {
            return Ok(await useCase.GetTypeList());
        }
    }
}
