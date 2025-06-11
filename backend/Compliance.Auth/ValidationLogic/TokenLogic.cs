using Compliance.Domain.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Compliance.Auth.ValidationLogic
{
    public class TokenLogic(IConfiguration _configuration) : ITokenLogic
    {
        public IEnumerable<Claim> ReadJwtTokenClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
            {
                throw new ArgumentException("Invalid JWT format.");
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.Claims;
        }

        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new (ClaimTypes.NameIdentifier, user.Username),
                new (ClaimTypes.Name, user.Name),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Sid, user._id.ToString()),
                new (ClaimTypes.Role, user.Roles ?? "User"), // Default to "User" if no roles are specified
            ];

            // Use the expiry time from configuration or default to 30 minutes
            int expiryMinutes = int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var mins) ? mins : 30;

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
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
    }
}
