using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.FinancialData
{
    public record GetDividendResponse : Response
    {
        public string? Symbol { get; set; }
        public decimal Dividends { get; set; }
    }
}
