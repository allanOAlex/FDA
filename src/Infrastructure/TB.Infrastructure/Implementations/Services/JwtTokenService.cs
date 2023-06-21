using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TB.Application.Abstractions.IServices;
using TB.Infrastructure.Extensions;
using TB.Shared.Requests.Auth;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration config;

        public JwtTokenService(IConfiguration Config)
        {
            config = Config;
        }

        public JwtSecurityToken GetAnonJwtToken()
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, "Anonymous"),
                new Claim(ClaimTypes.Role, "User"),
            };

            try
            {
                AuthExtensions.SecurityKey(out string key);
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Auth:Jwt:JwtSecurityKey"]!));
                var expiryInMin = Convert.ToInt32(config["Auth:Jwt:JwtExpiryInMin"]);

                var token = new JwtSecurityToken(
                    issuer: config["Auth:Jwt:JwtIssuer"],
                    audience: config["Auth:Jwt:JwtAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(expiryInMin),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                token.Header.Add("kid", authSigningKey.Key);
                token.SigningKey = authSigningKey;
                AuthExtensions.signKey = authSigningKey;
                return token;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching JwtToken | {ex.InnerException}");
            }
        }

        public JwtSecurityToken GetJwtToken(List<Claim> userClaims)
        {
            try
            {
                AuthExtensions.SecurityKey(out string key);
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Auth:Jwt:JwtSecurityKey"]!));
                var expiryInMin = Convert.ToInt32(config["Auth:Jwt:JwtExpiryInMin"]);

                var token = new JwtSecurityToken(
                    issuer: config["Auth:Jwt:JwtIssuer"],
                    audience: config["Auth:Jwt:JwtAudience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(expiryInMin),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                token.Header.Add("kid", authSigningKey.Key);
                token.SigningKey = authSigningKey;
                AuthExtensions.signKey = authSigningKey;
                return token;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching JwtToken | {ex.InnerException}");
            }
        }

        public JwtSecurityToken GetJwtTokenWithNewExpiry(List<Claim> userClaims, DateTime expirationDate)
        {
            try
            {
                AuthExtensions.SecurityKey(out string key);
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Auth:Jwt:JwtSecurityKey"]!));
                var expiryInMin = Convert.ToInt32(config["Auth:Jwt:JwtExpiryInMin"]);

                var token = new JwtSecurityToken(
                    issuer: config["Auth:Jwt:JwtIssuer"],
                    audience: config["Auth:Jwt:JwtAudience"],
                    claims: userClaims,
                    expires: expirationDate,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                token.Header.Add("kid", authSigningKey.Key);
                token.SigningKey = authSigningKey;
                AuthExtensions.signKey = authSigningKey;
                return token;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching JwtToken | {ex.InnerException}");
            }
        }

        public JwtSecurityToken GenerateRefreshToken(List<Claim> userClaims, GetRefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                AuthExtensions.SecurityKey(out string key);
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Auth:Jwt:JwtSecurityKey"]!));

                var token = new JwtSecurityToken(
                    issuer: config["Auth:Jwt:JwtIssuer"],
                    audience: config["Auth:Jwt:JwtAudience"],
                    claims: userClaims,
                    expires: refreshTokenRequest.ExpirationDate,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                token.Header.Add("kid", authSigningKey.Key);
                token.SigningKey = authSigningKey;
                AuthExtensions.signKey = authSigningKey;
                return token;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching JwtToken | {ex.InnerException}");
            }
        }

        public SecurityToken GetToken(List<Claim> userClaims)
        {
            try
            {
                Dictionary<string, object> claims = new Dictionary<string, object>
                {
                    { "claims", userClaims}
                };

                AuthExtensions.SecurityKey(out string key);
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var keyId = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                authSigningKey.KeyId = keyId.KeyId;
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, userClaims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(ClaimTypes.NameIdentifier, userClaims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userClaims?.FirstOrDefault(c => c.Type.Equals("Username", StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(JwtRegisteredClaimNames.Name, userClaims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(JwtRegisteredClaimNames.Email, userClaims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email, StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),

                    Claims = claims,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    Issuer = config["Auth:Jwt:JwtIssuer"],
                    Audience = config["Auth:Jwt:JwtAudience"],
                    SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512),
                };

                var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(tokenDescriptor);
                token.Header.Add("kid", new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)).Key.ToString());

                var userToken = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
                userToken.SigningKey.KeyId = authSigningKey.ToString();

                return userToken;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching JwtToken | {ex.InnerException}");
            }
        }

        public string JwtToken(List<Claim> userClaims)
        {
            try
            {
                Dictionary<string, object> claims = new Dictionary<string, object>
                {
                    { "claims", userClaims }
                };

                var authSigningKey = AuthExtensions.GetSymmetricKey();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, userClaims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userClaims?.FirstOrDefault(c => c.Type.Equals("Username", StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(JwtRegisteredClaimNames.Name, userClaims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(JwtRegisteredClaimNames.Email, userClaims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email, StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(ClaimTypes.Role, userClaims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role, StringComparison.OrdinalIgnoreCase))?.Value!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),

                    Expires = DateTime.UtcNow.AddMinutes(30),
                    Issuer = config["Auth:Jwt:JwtIssuer"],
                    Audience = config["Auth:Jwt:JwtAudience"],
                    SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512),
                };

                var userToken = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(userToken);

                return jwtToken;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching JwtToken | {ex.InnerException}");
            }
        }
    }
}
