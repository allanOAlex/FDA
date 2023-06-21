using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Auth
{
    public record CreateJwtAuthTokenResponse : Response
    {
        public string? JwtAuthToken { get; set; }
    }
}
