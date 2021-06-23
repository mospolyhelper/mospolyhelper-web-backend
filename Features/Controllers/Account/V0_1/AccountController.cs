namespace Mospolyhelper.Features.Controllers.Account.V0_1
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Account.Model.V0_1;
    using Domain.Account.UseCase.V0_1;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Produces("application/json")]
    [ApiVersion("0.1")]
    [ApiController, Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly AccountUseCase useCase;

        public AccountController(
            ILogger<AccountController> logger, 
            AccountUseCase useCase
            )
        {
            this.logger = logger;
            this.useCase = useCase;
        }

        public class AuthQuery
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string SessionId { get; set; } = string.Empty;
        }

        public class MyPortfolioQuery
        {
            public string OtherInformation { get; set; } = string.Empty;
            public bool IsPublic { get; set; } = false;
        }

        public class SendMessageRequest
        {
            public string DialogKey { get; set; }
            public string Message { get; set; }
            public IList<string> FileNames { get; set; } = Array.Empty<string>();
        }

        [HttpPost("auth")]
        public async Task<ActionResult<string>> GetSessionId(
            [FromBody] AuthQuery authObj
            )
        {
            this.logger.LogInformation("POST request /account/auth");
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

        [HttpGet("permissions")]
        public async Task<ActionResult<string>> GetPermissions(
            [FromHeader] string? sessionId = ""
            )
        {
            this.logger.LogInformation("POST request /account/permissions");
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
            this.logger.LogInformation($"GET request /account/portfolios query = {searchQuery}; page = {page}");
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
            this.logger.LogInformation($"GET request /account/teachers query = {searchQuery}; page = {page}");
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
            this.logger.LogInformation("GET request /account/classmates");
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
            this.logger.LogInformation("GET request /account/info");
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
            this.logger.LogInformation("GET request /account/marks");
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

        [HttpGet("grade-sheets")]
        public async Task<ActionResult<GradeSheets>> GetGradeSheets(
            [FromQuery] string? semester = "",
            [FromHeader] string? sessionId = ""
            )
        {
            this.logger.LogInformation("GET request /account/grade-sheets");
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.GetGradeSheets(sessionId, semester ?? string.Empty);
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

        [HttpGet("grade-sheet-marks")]
        public async Task<ActionResult<IList<GradeSheetMark>>> GetGradeSheetMarks(
            [FromQuery] string guid = "",
            [FromHeader] string? sessionId = ""
        )
        {
            this.logger.LogInformation("GET request /account/grade-sheets");
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.GetGradeSheetAllMarks(sessionId, guid);
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
            this.logger.LogInformation("GET request /account/applications");
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

        [HttpGet("payments")]
        public async Task<ActionResult<Payments>> GetPayments(
            [FromHeader] string? sessionId = ""
            )
        {
            this.logger.LogInformation("GET request /account/payments");
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.GetPayments(sessionId);
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
            this.logger.LogInformation("GET request /account/dialogs");
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
            this.logger.LogInformation("GET request /account/dialog");
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
            [FromBody] SendMessageRequest sendMessage,
            [FromHeader] string? sessionId = ""
            )
        {
            this.logger.LogInformation("POST request /account/message");
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.SendMessage(sessionId, sendMessage.DialogKey, sendMessage.Message, sendMessage.FileNames);
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

        [HttpDelete("message")]
        public async Task<ActionResult<IList<AccountMessage>>> RemoveMessage(
            [FromQuery] string removeKey,
            [FromHeader] string? sessionId = ""
        )
        {
            this.logger.LogInformation("DELETE request /account/message");
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.RemoveMessage(sessionId, removeKey);
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
        public async Task<ActionResult<MyPortfolioQuery>> GetMyPortfolio(
            [FromHeader] string? sessionId = ""
            )
        {
            this.logger.LogInformation("GET request /account/myportfolio");
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
        public async Task<ActionResult<MyPortfolioQuery>> SetMyPortfolio(
            [FromBody] MyPortfolioQuery portfolio,
            [FromHeader] string? sessionId = ""
            )
        {
            this.logger.LogInformation("POST request /account/myportfolio");
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
