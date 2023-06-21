using TB.Shared.Responses.Email;

namespace TB.Application.Abstractions.IServices
{
    public interface IEmailService
    {
        Task<PasswordResetEmailResponse> SendPasswordResetEmail(string emailAddress, string token, string resetUrl);
    }
}
