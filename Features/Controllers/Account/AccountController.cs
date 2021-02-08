namespace Mospolyhelper.Features.Controllers.Account
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Mospolyhelper.Domain.Account.Model;
    using Mospolyhelper.Domain.Account.UseCase;

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

        public class MessageQuery
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
            this.logger.LogInformation("GET request /account/marks");
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
                //return Ok(res.GetOrNull());
                return Content("{  \"contracts\": {    \"dormitory\": {      \"name\": \"Договор № 000858 от 24 августа 2018\",      \"paidAmount\": 69000,      \"debt\": -650,      \"debtDate\": \"12.01.2021\",      \"remainingAmount\": 11700,      \"expirationDate\": \"31 августа 2022\",      \"payments\": [        {          \"date\": \"29 сентября 2020 г\",          \"amount\": 3900        },        {          \"date\": \"26 февраля 2020 г\",          \"amount\": 3900        },        {          \"date\": \"4 сентября 2019 г\",          \"amount\": 3900        },        {          \"date\": \"2 августа 2019 г\",          \"amount\": 3800        },        {          \"date\": \"2 июля 2019 г\",          \"amount\": 3800        },        {          \"date\": \"3 июня 2019 г\",          \"amount\": 3800        },        {          \"date\": \"6 мая 2019 г\",          \"amount\": 3800        },        {          \"date\": \"2 апреля 2019 г\",          \"amount\": 3800        },        {          \"date\": \"6 марта 2019 г\",          \"amount\": 3800        },        {          \"date\": \"4 февраля 2019 г\",          \"amount\": 3800        },        {          \"date\": \"21 января 2019 г\",          \"amount\": 3800        },        {          \"date\": \"10 декабря 2018 г\",          \"amount\": 3800        },        {          \"date\": \"6 ноября 2018 г\",          \"amount\": 3800        },        {          \"date\": \"26 октября 2018 г\",          \"amount\": 3800        },        {          \"date\": \"24 августа 2018 г\",          \"amount\": 3800        }      ],      \"sberQR\": \"qr.php?data=ST00012%7CName%3D%D3%D4%CA%20%EF%EE%20%E3.%CC%EE%F1%EA%E2%E5%20%28%CC%EE%F1%EA%EE%E2%F1%EA%E8%E9%20%CF%EE%EB%E8%F2%E5%F5%20%EB%2F%F1%2020736%C504980%29%7CPersonalAcc%3D40501810845252000079%7CBankName%3D%C3%D3%20%C1%E0%ED%EA%E0%20%D0%EE%F1%F1%E8%E8%20%EF%EE%20%D6%D4%CE%7CBIC%3D044525000%7CCorrespAcc%3D0%7CPayeeINN%3D7719455553%7CCategory%3DPOBSH%7CContract%3D000858%7CLastName%3D%7CFirstName%3D%7CMiddleName%3D%7CPayerAddress%3D%7CChildFio%3D%C4%FB%ED%E4%E8%ED%20%C0%EB%E5%EA%F1%E0%ED%E4%F0%20%C2%EB%E0%E4%E8%EC%E8%F0%EE%E2%E8%F7%7CCBC%3D00000000000000000130%7COKTMO%3D45314000%7CPurpose%3D%C4%EE%E3%EE%E2%EE%F0%20%EE%F2%2024.08.2018%20%B9%20000858%7CSum%3D1170000%7C&size=2&ic=1\"    },    \"tuition\": {      \"name\": \"Договор № 0271-1Б10/1-18/19 от 3 августа 2018\",      \"paidAmount\": 194000,      \"debt\": 0,      \"debtDate\": \"12.01.2021\",      \"remainingAmount\": 0,      \"expirationDate\": \"\",      \"payments\": [        {          \"date\": \"29 января 2019 г\",          \"amount\": 97000        },        {          \"date\": \"6 августа 2018 г\",          \"amount\": 97000        }      ],      \"sberQR\": \"\"    }  }}", "application/json");
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
            [FromBody] MessageQuery message,
            [FromHeader] string? sessionId = ""
            )
        {
            this.logger.LogInformation("POST request /account/message");
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await useCase.SendMessage(sessionId, message.DialogKey, message.Message, message.FileNames);
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
