using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.Logging
{
    public record QueryLogFileRequest : Request
    {
        public string? LogFile { get; set; }
        public string? Level { get; set; }
        public string? Method { get; set; }
        public string? Time { get; set; }
        public string? RequestId { get; set; }
        public string? RequestPath { get; set; }
        public string? ConnectionId { get; set; }
        public string? CorrelationId { get; set; }



    }
}
