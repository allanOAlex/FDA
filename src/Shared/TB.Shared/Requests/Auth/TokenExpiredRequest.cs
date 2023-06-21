using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Auth
{
    public record TokenExpiredRequest : Request
    {
        public int UserId { get; set; }
    }
}
