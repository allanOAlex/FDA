using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.User
{
    public record UpdateUserRequest : Request
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public string? PasswordResetToken { get; set; }
    }
}
