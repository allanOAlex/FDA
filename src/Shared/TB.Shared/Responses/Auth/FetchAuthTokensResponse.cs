using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Auth
{
    public record FetchAuthTokensResponse : Response
    {
        public int UserId { get; set; }
        public string? TokenName { get; set; }
        public string? TokenValue { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }


    }
}
