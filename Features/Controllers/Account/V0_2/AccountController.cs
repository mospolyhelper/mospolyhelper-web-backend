namespace Mospolyhelper.Features.Controllers.Account.V0_2
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Mospolyhelper.Domain.Account.Model;
    using Mospolyhelper.Domain.Account.UseCase;

    [Produces("application/json")]
    [ApiVersion("0.2")]
    [ApiController, Route("v{version:apiVersion}/[controller]")]
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

        private string GenerateJSONWebToken(string sessionId)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, sessionId),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "student")
                };
            var w = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretqqq!123"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                claims: w.Claims,
                notBefore: now,
                expires: now.AddHours(1).AddMinutes(20),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("auth")]
        public async Task<ActionResult> Auth(
            [FromBody] AuthQuery authObj
            )
        {
            this.logger.LogInformation("POST request /account/auth");
            var res = await useCase.GetSessionId(authObj.Login, authObj.Password, authObj.SessionId);
            if (res.IsSuccess)
            {
                return Ok(new { Token = GenerateJSONWebToken(res.GetOrNull()!) });
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
            var sessionId = User.Identity.Name;
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
    }
}
