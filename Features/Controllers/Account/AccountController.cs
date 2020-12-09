using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mospolyhelper.Data.Account.Api;
using Mospolyhelper.Data.Account.Remote;
using Mospolyhelper.Domain.Account.Model;
using Mospolyhelper.Utils;

namespace Mospolyhelper.Features.Controllers.Account
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly AccountClient api;
        private readonly AccountRemoteDataSource dataSource;

        public AccountController(AccountClient useCase, AccountRemoteDataSource dataSource)
        {
            this.api = useCase;
            this.dataSource = dataSource;
        }

        public class AuthObj
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("auth")]
        public async Task<ActionResult<string>> GetSessionId(
            [FromBody] AuthObj authObj,
            [FromHeader] string? sessionId = ""
            )
        {
            var (result, sessionIdRes) = await dataSource.GetSessionId(authObj.Login, authObj.Password, sessionId);
            if (result)
            {
                return Ok(sessionIdRes);
            }
            else
            {
                return this.Problem(sessionIdRes);
            }
        }

        [HttpGet("portfolios")]
        public async Task<ActionResult<AccountStudents>> GetPortfolios(
            [FromQuery] string? searchQuery = "", 
            [FromQuery] int page = 1
            )
        {
            return Ok(await dataSource.GetPortfolios(searchQuery ?? string.Empty, page));
        }

        [HttpGet("teachers")]
        public async Task<ActionResult<IList<AccountTeacher>>> GetTeachers(
            [FromHeader] string sessionId,
            [FromQuery] string? searchQuery = "",
            [FromQuery] int page = 0
            )
        {
            var res = await dataSource.GetTeachers(sessionId, searchQuery ?? string.Empty, page);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                switch (res.ExceptionOrNull())
                {
                    case UnauthorizedAccessException e:
                        return Unauthorized();
                    default: return StatusCode(500);
                }
            }
            return StatusCode(500);
        }

        [HttpGet("info")]
        public async Task<ActionResult<AccountInfo?>> GetInfo([FromHeader] string sessionId)
        {
            return Ok(await dataSource.GetInfo(sessionId));
        }

        [HttpGet("marks")]
        public async Task<ActionResult<AccountMarks>> GetMarks([FromHeader] string sessionId)
        {
            return Ok(await dataSource.GetMarks(sessionId));
        }

        [HttpGet("applications")]
        public async Task<ActionResult<IList<AccountApplication>>> GetApplications([FromHeader] string sessionId)
        {
            return Ok(await dataSource.GetApplications(sessionId));
        }

        [HttpGet("classmates")]
        public async Task<ActionResult<IList<AccountClassmate>>> GetClassmates([FromHeader] string sessionId)
        {
            return Ok(await dataSource.GetClassmates(sessionId));
        }

        [HttpGet("dialogs")]
        public async Task<ActionResult<IList<AccountDialogPreview>>> GetMessages([FromHeader] string sessionId)
        {
            return Ok(await dataSource.GetDialogs(sessionId));
        }

        [HttpGet("dialog")]
        public async Task<ActionResult<IList<AccountMessage>>> GetMessages(
            [FromHeader] string sessionId,
            [FromQuery] string dialogKey
            )
        {
            return Ok(await dataSource.GetDialog(sessionId, dialogKey));
        }
    }
}
