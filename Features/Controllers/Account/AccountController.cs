using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mospolyhelper.Data.Account.Api;

namespace Mospolyhelper.Features.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountClient useCase = new AccountClient(new HttpClient());

        [HttpGet("portfolio")]
        public async Task<IActionResult> Get()
        {
            return Ok(await useCase.GetPortfolio());
        }

        [HttpGet("auth")]
        public async Task<IActionResult> GetSessionId([FromQuery] string login, [FromQuery] string password)
        {
            return Ok(await useCase.GetSessionId(login, password));
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetInfo([FromHeader] string sessionId)
        {
            return Ok(await useCase.GetInfo(sessionId));
        }
    }
}
