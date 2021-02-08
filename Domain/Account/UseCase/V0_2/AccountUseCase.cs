namespace Mospolyhelper.Domain.Account.UseCase.V0_2
{
    using Microsoft.Extensions.Logging;
    using Mospolyhelper.Domain.Account.Model.V0_2;
    using Mospolyhelper.Domain.Account.Repository.V0_2;
    using Mospolyhelper.Utils;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Tokens;

    public class AccountUseCase
    {
        private readonly ILogger logger;
        private readonly IAccountRepository accountRepository;

        public AccountUseCase(
            ILogger<AccountUseCase> logger,
            IAccountRepository accountRepository
            )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
        }

        public async Task<Result<AuthResponse>> Auth(AuthRequest authRequest, string? sessionId = null)
        {
            this.logger.LogDebug("Auth");
            var res = await this.accountRepository.Auth(authRequest.Login, authRequest.Password, sessionId);
            return res.MapCatching(it =>
            {
                var accessToken = GenerateJSONWebToken(it);
                var refreshToken = GenerateRefreshToken(authRequest);
                return new AuthResponse(accessToken, refreshToken);
            });
        }

        public async Task<Result<string>> Refresh(string refreshToken, string sessionId)
        {
            this.logger.LogDebug("Refresh");
            AuthRequest authRequest;
            try
            {
                authRequest = DecryptRefreshToken(refreshToken);
            }
            catch (Exception e)
            {
                return Result<string>.Failure(new UnauthorizedAccessException());
            }
            var res = await this.accountRepository.Auth(authRequest.Login, authRequest.Password, sessionId);
            return res.MapCatching(GenerateJSONWebToken);
        }

        private string GenerateRefreshToken(AuthRequest authRequest)
        {
            var text = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(authRequest));
            var key = Encoding.UTF8.GetBytes(Secrets.AuthAesKey);
            var iv = Encoding.UTF8.GetBytes(Secrets.AuthAesIv);

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
            var key = Encoding.UTF8.GetBytes(Secrets.AuthAesKey);
            var iv = Encoding.UTF8.GetBytes(Secrets.AuthAesIv);

            using var aes = Aes.Create();
            aes.IV = iv;
            aes.Key = key;

            var cryptTransform = aes.CreateDecryptor();

            var text = cryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return JsonSerializer.Deserialize<AuthRequest>(Encoding.UTF8.GetString(text));
        }

        private string GenerateJSONWebToken(UserAuth userAuth)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secrets.AuthJwtKey));
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
    }
}
