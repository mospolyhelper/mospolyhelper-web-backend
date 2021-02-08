namespace Mospolyhelper.Features.Controllers.Account.V0_2
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Mospolyhelper.Domain.Account.Model.V0_2;
    using Mospolyhelper.Domain.Account.UseCase.V0_2;

    [Produces("application/json")]
    [ApiVersion("0.2")]
    [ApiController, Route("v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly AccountUseCase useCase;
        private readonly Domain.Account.UseCase.AccountUseCase oldUseCase;

        public AccountController(
            ILogger<AccountController> logger,
            AccountUseCase useCase,
            Domain.Account.UseCase.AccountUseCase oldUseCase
            )
        {
            this.logger = logger;
            this.useCase = useCase;
            this.oldUseCase = oldUseCase;
        }

        public class AuthRequest
        {
            public string Login { get; set; }
            public string Password { get; set; }
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

        private string GenerateJSONWebToken(UserAuth userAuth)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretqqq!123"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var now = DateTimeOffset.UtcNow;
            
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload
            {
                { "sessionId", userAuth.SessionId },
                { "name", userAuth.Name },
                { "avatarUrl", userAuth.AvatarUrl },
                { "permissions", userAuth.Permissions },
                { "exp", now.AddHours(1).AddMinutes(20).ToUnixTimeSeconds() }
            };
            
            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefeshToken(AuthRequest authRequest)
        {
            var text = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(authRequest));
            var key = Encoding.UTF8.GetBytes("kdjriflt83,50dk6");
            var iv = Encoding.UTF8.GetBytes("k8kg5h7g5d3em7ik");

            using var aes = Aes.Create();
            aes.IV = iv;
            aes.Key = key;
            var cryptTransform = aes.CreateEncryptor();

            var cipherText = cryptTransform.TransformFinalBlock(text, 0, text.Length);

            return Convert.ToBase64String(cipherText);
        }

        private AuthRequest DecryptRefreshToken(string refreshToken)
        {
            var cipherText = Convert.FromBase64String(refreshToken);
            var key = Encoding.UTF8.GetBytes("kdjriflt83,50dk6");
            var iv = Encoding.UTF8.GetBytes("k8kg5h7g5d3em7ik");

            using var aes = Aes.Create();
            aes.IV = iv;
            aes.Key = key;

            var cryptTransform = aes.CreateDecryptor();

            var text = cryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return JsonSerializer.Deserialize<AuthRequest>(Encoding.UTF8.GetString(text));
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthResponse>> Authenticate(
            [FromBody] AuthRequest authRequest
            )
        {
            this.logger.LogInformation("POST request /account/authenticate");
            var res = await useCase.Auth(authRequest.Login, authRequest.Password);
            if (res.IsSuccess)
            {
                var accessToken = GenerateJSONWebToken(res.GetOrNull()!);
                var resfreshToken = GenerateRefeshToken(authRequest);
                return Ok(new AuthResponse(accessToken, resfreshToken));
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
            AuthRequest authRequest;
            try
            {
                authRequest = DecryptRefreshToken(refreshToken);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
            var sessionId = User.Claims.FirstOrDefault(it => it.Type == "sessionId")?.Value;
            if (sessionId == null)
            {
                return Unauthorized();
            }
            var res = await useCase.Auth(authRequest.Login, authRequest.Password, sessionId);
            if (res.IsSuccess)
            {
                var accessToken = GenerateJSONWebToken(res.GetOrNull()!);
                var resfreshToken = GenerateRefeshToken(authRequest);
                return Ok(accessToken);
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
        public async Task<ActionResult<Domain.Account.Model.Info?>> GetInfo()
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
