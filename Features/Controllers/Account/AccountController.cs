using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mospolyhelper.Data.Account.Api;
using Mospolyhelper.Data.Account.Remote;
using Mospolyhelper.Domain.Account.Model;

namespace Mospolyhelper.Features.Controllers.Account
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountClient useCase;
        private readonly AccountRemoteDataSource dataSource;

        public AccountController(AccountClient useCase, AccountRemoteDataSource dataSource)
        {
            this.useCase = useCase;
            this.dataSource = dataSource;
        }

        [HttpGet("portfolios")]
        public async Task<ActionResult<IList<AccountPortfolio>>> Get([FromQuery] string? searchQuery = "", [FromQuery] int page = 0)
        {
            return Ok(await dataSource.GetPortfolios(searchQuery ?? string.Empty, page));
        }

        [HttpGet("auth")]
        public async Task<ActionResult<string>> GetSessionId([FromQuery] string login, [FromQuery] string password)
        {
            return Ok(await useCase.GetSessionId(login, password));
        }

        [HttpGet("info")]
        public async Task<ActionResult<AccountInfo?>> GetInfo([FromHeader] string sessionId)
        {
            return Ok(await dataSource.GetInfo(sessionId));
        }

        [HttpGet("marks")]
        public async Task<ActionResult<string>> GetMarks([FromHeader] string sessionId)
        {
            return Ok(await dataSource.GetMarks(sessionId));
        }

        [HttpGet("applications")]
        public async Task<ActionResult<string>> GetApplications([FromHeader] string sessionId)
        {
            return Ok(await useCase.GetApplications(sessionId));
        }
    }
}
