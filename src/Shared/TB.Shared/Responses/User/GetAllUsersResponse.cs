using System.Security.Claims;
using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.User
{
    public record GetAllUsersResponse : Response
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsActive { get; set; }
        
        public List<Claim>? UserClaims { get; set; }
    }
}
