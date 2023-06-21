using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TB.Shared.Requests.Auth;

namespace TB.Application.Abstractions.IServices
{
    public interface IJwtTokenService
    {
        JwtSecurityToken GetAnonJwtToken();
        JwtSecurityToken GetJwtToken(List<Claim> userClaims);
        JwtSecurityToken GetJwtTokenWithNewExpiry(List<Claim> userClaims, DateTime expirationDate);
        JwtSecurityToken GenerateRefreshToken(List<Claim> userClaims, GetRefreshTokenRequest refreshTokenRequest);
        SecurityToken GetToken(List<Claim> userClaims);
        string JwtToken(List<Claim> userClaims);
    }
}
