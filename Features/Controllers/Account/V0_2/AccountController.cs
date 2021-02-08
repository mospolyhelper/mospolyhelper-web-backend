namespace Mospolyhelper.Features.Controllers.Account.V0_2
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Account.Model.V0_1;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Domain.Account.Model.V0_2;
    using Domain.Account.UseCase.V0_2;

    [Produces("application/json")]
    [ApiVersion("0.2")]
    [ApiController, Route("v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly AccountUseCase useCase;
        private readonly Domain.Account.UseCase.V0_1.AccountUseCase oldUseCase;

        public AccountController(
            ILogger<AccountController> logger,
            AccountUseCase useCase,
            Domain.Account.UseCase.V0_1.AccountUseCase oldUseCase
            )
        {
            this.logger = logger;
            this.useCase = useCase;
            this.oldUseCase = oldUseCase;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthResponse>> Authenticate(
            [FromBody] AuthRequest authRequest
            )
        {
            this.logger.LogInformation("POST request /account/authenticate");
            var res = await useCase.Auth(authRequest);
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

        [Authorize]
        [HttpPost("refresh")]
        public async Task<ActionResult<string>> Refresh(
            [FromBody] string refreshToken
            )
        {
            this.logger.LogInformation("POST request /account/refresh");
            var sessionId = User.Claims.FirstOrDefault(it => it.Type == "sessionId")?.Value;

            if (sessionId == null)
            {
                return Unauthorized();
            }
            var res = await useCase.Refresh(refreshToken, sessionId);
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

        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult<Info?>> GetInfo()
        {
            this.logger.LogInformation("GET request /account/info");
            var sessionId = User.Claims.FirstOrDefault(it => it.Type == "sessionId")?.Value;
            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }
            var res = await oldUseCase.GetInfo(sessionId);
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
