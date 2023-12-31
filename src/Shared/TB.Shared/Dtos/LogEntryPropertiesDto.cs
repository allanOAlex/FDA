﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.Shared.Dtos
{
    public record LogEntryPropertiesDto
    {
        public string? Method { get; set; }
        public string? Path { get; set; }
        public string? Time { get; set; }
        public string? RequestId { get; set; }
        public string? RequestPath { get; set; }
        public string? ConnectionId { get; set; }
        public Guid CorrelationId { get; set; }
        public string? MachineName { get; set; }



    }
}
