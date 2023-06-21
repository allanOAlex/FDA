using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using TB.Shared.Requests.Auth;
using TB.Shared.Responses.Auth;
using TB.Shared.Responses.Autrh;

namespace TB.Application.Abstractions.IServices
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginWithSignInManager(LoginRequest loginRequest);
        Task<LogoutResponse> LogoutWithSignInManager(LogoutRequest logoutRequest);
        Task<ForgotPasswordResponse> ForgotUserPassword(ForgotPasswordRequest request);
        Task<ResetPasswordResponse> UserPasswordReset(ResetPasswordRequest resetPasswordRequest);
        Task<string> ValidatePasswordResetToken(string token);
        Task<AnonymousTokenResponse> GenerateAnonymousToken();
        Task<CreateJwtAuthTokenResponse> CreateJwtAuthToken(int UserId);
        Task<string> FetchRefreshToken(int UserId);
        Task<GetRefreshTokenResponse> GenerateRefreshToken(List<Claim> userClaims, GetRefreshTokenRequest refreshTokenRequest);
        Task<PassResetTokenValidationResponse> ValidateJwtPasswordResetToken(string token);
        bool IsTokenValid(SecurityToken token);
        Task<UnlockScreenResponse> UnlockScreen(UnlockScreenRequest unlockScreenRequest);

    }

}
