using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.Shared.Dtos
{
    public record LogEntryDto
    {
        public DateTime Timestamp { get; set; }
        public string? Level { get; set; }
        public string? MessageTemplate { get; set; }
        public LogEntryPropertiesDto? Properties { get; set; }


    }
}
