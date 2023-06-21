using Microsoft.AspNetCore.Authorization;
using TB.Mvc.Enums;

namespace TB.Mvc.Helpers.AuthHelpers
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        public AuthorizeRoles(params Role[] roles)
        {
            Roles = string.Join(",", roles.Select(r => r.ToString()));
        }
    }



}
