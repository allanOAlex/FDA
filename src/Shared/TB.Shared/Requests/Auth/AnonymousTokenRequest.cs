using System.Security.Claims;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Auth
{
    public record AnonymousTokenRequest : Request
    {
        public List<Claim>? Claims { get; set; } 
    }
}
