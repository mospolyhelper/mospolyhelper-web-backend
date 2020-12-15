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
            public string SessionId { get; set; } = string.Empty;
        }

        [HttpPost("auth")]
        public async Task<ActionResult<string>> GetSessionId(
            [FromBody] AuthObj authObj
            )
        {
            var (result, sessionIdRes) = await dataSource.GetSessionId(authObj.Login, authObj.Password, authObj.SessionId);
            if (result)
            {
                return Ok(sessionIdRes);
            }
            else
            {
                return Unauthorized();
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
        public async Task<ActionResult<AccountTeachers>> GetTeachers(
            [FromQuery] string? searchQuery = "",
            [FromQuery] int page = 1,
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await dataSource.GetTeachers(sessionId, searchQuery ?? string.Empty, page);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return (res.ExceptionOrNull()) switch
                {
                    UnauthorizedAccessException e => Unauthorized(),
                    _ => StatusCode(500),
                };
            }
            return StatusCode(500);
        }

        [HttpGet("classmates")]
        public async Task<ActionResult<IList<AccountClassmate>>> GetClassmates(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await dataSource.GetClassmates(sessionId);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return (res.ExceptionOrNull()) switch
                {
                    UnauthorizedAccessException e => Unauthorized(),
                    _ => StatusCode(500),
                };
            }
            return StatusCode(500);
        }

        [HttpGet("info")]
        public async Task<ActionResult<AccountInfo?>> GetInfo(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await dataSource.GetInfo(sessionId);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return (res.ExceptionOrNull()) switch
                {
                    UnauthorizedAccessException e => Unauthorized(),
                    _ => StatusCode(500),
                };
            }
            return StatusCode(500);
        }

        [HttpGet("marks")]
        public async Task<ActionResult<AccountMarks>> GetMarks(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await dataSource.GetMarks(sessionId);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return (res.ExceptionOrNull()) switch
                {
                    UnauthorizedAccessException e => Unauthorized(),
                    _ => StatusCode(500),
                };
            }
            return StatusCode(500);
        }

        [HttpGet("applications")]
        public async Task<ActionResult<IList<AccountApplication>>> GetApplications(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await dataSource.GetApplications(sessionId);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return (res.ExceptionOrNull()) switch
                {
                    UnauthorizedAccessException e => Unauthorized(),
                    _ => StatusCode(500),
                };
            }
            return StatusCode(500);
        }

        [HttpGet("dialogs")]
        public async Task<ActionResult<IList<AccountDialogPreview>>> GetMessages(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await dataSource.GetDialogs(sessionId);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return (res.ExceptionOrNull()) switch
                {
                    UnauthorizedAccessException e => Unauthorized(),
                    _ => StatusCode(500),
                };
            }
            return StatusCode(500);
        }

        [HttpGet("dialog")]
        public async Task<ActionResult<IList<AccountMessage>>> GetMessages(
            [FromQuery] string dialogKey,
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await dataSource.GetDialog(sessionId, dialogKey);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return (res.ExceptionOrNull()) switch
                {
                    UnauthorizedAccessException e => Unauthorized(),
                    _ => StatusCode(500),
                };
            }
            return StatusCode(500);
        }

        [HttpGet("myportfolio")]
        public async Task<ActionResult<MyPortfolio>> GetMyPortfolio(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await dataSource.GetMyPortfolio(sessionId);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return (res.ExceptionOrNull()) switch
                {
                    UnauthorizedAccessException e => Unauthorized(),
                    _ => StatusCode(500),
                };
            }
            return StatusCode(500);
        }

        [HttpPost("myportfolio")]
        public async Task<ActionResult<MyPortfolio>> SetMyPortfolio(
            [FromBody] MyPortfolio portfolio,
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await dataSource.SetMyPortfolio(sessionId, portfolio.OtherInformation, portfolio.IsPublic);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return (res.ExceptionOrNull()) switch
                {
                    UnauthorizedAccessException e => Unauthorized(),
                    _ => StatusCode(500),
                };
            }
            return StatusCode(500);
        }

        public class MyPortfolio
        {
            public string OtherInformation { get; set; } = string.Empty;
            public bool IsPublic { get; set; }
        }
    }
}
