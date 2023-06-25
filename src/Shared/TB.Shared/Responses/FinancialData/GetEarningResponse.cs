using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.FinancialData
{
    public record GetEarningResponse : Response
    {
        public string? Symbol { get; set; }
        public DateTime Date { get; set; }
        public string? Quater { get; set; }
        public decimal EpsEst { get; set; }
        public decimal Eps { get; set; }
        public string? ReleaseTime { get; set; }
    }
}
