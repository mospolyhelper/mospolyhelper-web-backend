using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mospolyhelper.Data.Account.Api;
using Mospolyhelper.Data.Account.Remote;
using Mospolyhelper.Domain.Account.Model;
using Mospolyhelper.Domain.Account.UseCase;
using Mospolyhelper.Utils;

namespace Mospolyhelper.Features.Controllers.Account
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly AccountUseCase useCase;

        public AccountController(AccountUseCase useCase)
        {
            this.useCase = useCase;
        }

        public class AuthObj
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string SessionId { get; set; } = string.Empty;
        }

        public class MyPortfolio
        {
            public string OtherInformation { get; set; } = string.Empty;
            public bool IsPublic { get; set; } = false;
        }

        public class MessageObj
        {
            public string Message { get; set; }
            public IList<string> FileNames { get; set; } = Array.Empty<string>();
        }

        [HttpPost("auth")]
        public async Task<ActionResult<string>> GetSessionId(
            [FromBody] AuthObj authObj
            )
        {
            var res = await useCase.GetSessionId(authObj.Login, authObj.Password, authObj.SessionId);
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

        [HttpPost("permissions")]
        public async Task<ActionResult<string>> GetPermissions(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.GetPermissions(sessionId);
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

        [HttpGet("portfolios")]
        public async Task<ActionResult<Students>> GetPortfolios(
            [FromQuery] string? searchQuery = "", 
            [FromQuery] int page = 1
            )
        {
            var res = await useCase.GetPortfolios(searchQuery ?? string.Empty, page);
            if (res.IsSuccess)
            {
                return Ok(res.GetOrNull());
            }
            else if (res.IsFailure)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
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
            var res = await useCase.GetTeachers(sessionId, searchQuery ?? string.Empty, page);
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
        public async Task<ActionResult<IList<Classmate>>> GetClassmates(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.GetClassmates(sessionId);
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
        public async Task<ActionResult<Info?>> GetInfo(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.GetInfo(sessionId);
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
            var res = await useCase.GetMarks(sessionId);
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
        public async Task<ActionResult<IList<Application>>> GetApplications(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.GetApplications(sessionId);
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
        public async Task<ActionResult<IList<DialogPreview>>> GetDialogs(
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.GetDialogs(sessionId);
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
        public async Task<ActionResult<IList<AccountMessage>>> GetDialog(
            [FromQuery] string dialogKey,
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.GetDialog(sessionId, dialogKey);
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

        [HttpPost("message")]
        public async Task<ActionResult<IList<AccountMessage>>> SendMessage(
            [FromQuery] string dialogKey,
            [FromBody] MessageObj message,
            [FromHeader] string? sessionId = ""
            )
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.SendMessage(sessionId, dialogKey, message.Message, message.FileNames);
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
            var res = await useCase.GetMyPortfolio(sessionId);
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
            var res = await useCase.SetMyPortfolio(sessionId, portfolio.OtherInformation, portfolio.IsPublic);
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
    }
}
