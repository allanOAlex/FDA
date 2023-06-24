using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.FinancialData
{
    public record InsertDividendRequest : Request
    {
        public string? Symbol { get; set; }
        public decimal Dividends { get; set; }
    }
}
