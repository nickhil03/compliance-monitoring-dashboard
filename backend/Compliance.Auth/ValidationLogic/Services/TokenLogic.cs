using Compliance.Auth.ValidationLogic.Contracts;
using Compliance.Domain.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Compliance.Auth.ValidationLogic.Services
{
    public class TokenLogic(IConfiguration _configuration, IAuthenticationService _authService) : ITokenLogic
    {
        public async Task<TokenModel> GenerateTokens(string userName, string? userAgent, string? IpAddress, bool isRefresh)
        {
            User user = await _authService.GetUserByUsernameAsync(userName ?? "") ?? throw new InvalidOperationException("User not found.");
            var refreshTokenEntity = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(GetRefreshTokenExpiryDays()),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = IpAddress ?? "",
                UserAgent = userAgent,
                UserName = user.Username,
                UserId = user._id.ToString()
            };

            if (!isRefresh)
                await _authService.SaveRefreshTokenAsync(refreshTokenEntity);

            var accessTokenExpiry = DateTime.UtcNow.AddMinutes(GetExpiryMinutes());

            return new TokenModel
            {
                AccessToken = GenerateAccessToken(user, accessTokenExpiry),
                RefreshToken = refreshTokenEntity.Token,
                AccessTokenExpires = accessTokenExpiry,
                RefreshTokenExpires = refreshTokenEntity.ExpiresAt
            };
        }

        public ClaimsPrincipal? ValidateJwtToken(string token)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT key is not configured.");

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Optional: helps account for server time differences
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    bool isHmac = jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!isHmac)
                    {
                        return null; // Or throw an exception for unsupported algorithm
                    }
                }
                return principal;
            }
            catch
            {
                return null; // Token validation failed
            }
        }

        public async Task RevokeRefreshTokenAsync(string token, string? replacedByToken = null)
        {
            var activeRefreshTokens = await _authService.GetRefreshTokensAsync();

            foreach (var activeToken in activeRefreshTokens)
            {
                if (BCrypt.Net.BCrypt.Verify(token, activeToken.Token))
                {
                    activeToken.IsRevoked = true;
                    activeToken.RevokedAt = DateTime.UtcNow;
                    activeToken.ReplacedByToken = replacedByToken;

                    await _authService.UpdateRefreshTokenAsync(activeToken);
                    break;
                }
            }
        }

        public async Task RevokeAllUserTokensAsync(string userId)
        {
            var activeRefreshTokens = await _authService.GetRefreshTokensAsync(f => f.UserId == userId);
            foreach (var activeToken in activeRefreshTokens)
            {
                activeToken.IsRevoked = true;
                activeToken.RevokedAt = DateTime.UtcNow;
                await _authService.SaveRefreshTokenAsync(activeToken);
            }
        }

        public async Task<string?> GetUserNameFromRefreshTokenAsync(string token)
        {
            var activeRefreshTokens = await _authService.GetRefreshTokensAsync(x => x.ExpiresAt > DateTime.UtcNow);
            foreach (var activeToken in activeRefreshTokens)
            {
                if (BCrypt.Net.BCrypt.Verify(token, activeToken.Token) && !string.IsNullOrEmpty(activeToken.UserName))
                {
                    return activeToken.UserName;
                }
            }
            
            return null;
        }

        private int GetExpiryMinutes()
        {
            if (int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var expiry))
            {
                return expiry;
            }
            return 30; // Default to 30 minutes if not configured
        }

        private int GetRefreshTokenExpiryDays()
        {
            if (int.TryParse(_configuration["RefreshToken:ExpiryDays"], out var refreshExpiry))
            {
                return refreshExpiry;
            }
            return 7; // Default to 7 days if not configured
        }

        private static string GenerateRefreshToken()
        {
            var random = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);
            return Convert.ToBase64String(random);
        }

        private string GenerateAccessToken(User user, DateTime expires)
        {
            string? jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT key is not configured.");

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new (ClaimTypes.NameIdentifier, user.Username),
                new (ClaimTypes.Name, user.Name),
                new (ClaimTypes.Email, user.Email ?? ""),
                new (ClaimTypes.Sid, user._id.ToString()),
                new (ClaimTypes.Role, user.Roles ?? "User"),
            ];

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}