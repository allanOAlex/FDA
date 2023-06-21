using TB.Domain.Models;
using TB.Shared.Responses.Auth;

namespace TB.Application.Abstractions.IRepositories
{
    public interface IAuthRepository
    {
        Task LoginWithSignInManager(string UserName, string Password, bool isPersistent, bool lockOutOnFailure);
        Task LogoutWithSignInManager();
        Task<ResetPasswordResponse> ResetPassword(AppUser entity);
        Task<int> InvalidatePasswordResetToken(AppUser entity);
        Task<int> ManageAuthTokenAsync(AuthToken entity, int commandId);
        Task<List<FetchAuthTokensResponse>> FetchAuthTokens();
        Task<IEnumerable<AuthToken>> FindAuthTokens();
        Task<int> CreateAuthTokenAsync(AuthToken entity);
        Task<string> FetchRefreshTokenAsync(int UserId);
        Task<int> UpdateAuthToken(AuthToken entity);
    }
}
