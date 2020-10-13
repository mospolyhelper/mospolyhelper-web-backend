using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mospolyhelper.Data.Schedule.Api;
using Mospolyhelper.Data.Schedule.Converters;
using Mospolyhelper.Data.Schedule.Remote;
using Mospolyhelper.Data.Schedule.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mospolyhelper.Features.Controllers.Schedule
{
    [Route("[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        // GET: api/<ScheduleController>
        [HttpGet]
        public Domain.Schedule.Models.Schedule Get()
        {
            var repo = new ScheduleRepository(
                new ScheduleRemoteDataSource(
                    new ScheduleClient(),
                    new ScheduleRemoteConverter()
                )
            );
            return repo.GetSchedule("181-721").GetAwaiter().GetResult();
        }

        // GET api/<ScheduleController>/5
        [HttpGet("{id}")]
        public Domain.Schedule.Models.Schedule Get(string id)
        {
            var repo = new ScheduleRepository(
                new ScheduleRemoteDataSource(
                    new ScheduleClient(),
                    new ScheduleRemoteConverter()
                )
            );
            return repo.GetSchedule(id).GetAwaiter().GetResult();
        }

        // POST api/<ScheduleController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ScheduleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ScheduleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
