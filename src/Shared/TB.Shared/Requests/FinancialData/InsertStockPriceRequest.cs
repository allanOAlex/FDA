using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.FinancialData
{
    public record InsertStockPriceRequest : Request
    {
        public string? Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal CloseAdjusted { get; set; }
        public int Volume { get; set; }
        public decimal SplitCoefficient { get; set; }
    }
}
