using System.Security.Claims;
using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.User
{
    public record GetUserClaimsResponse : Response
    {
        public virtual List<Claim>? Claims { get; set; }

    }
}
