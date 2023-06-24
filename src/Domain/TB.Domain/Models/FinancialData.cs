using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.Domain.Models
{
    public class FinancialData
    {
        public ICollection<Dividend>? Dividends { get; set; }
        public ICollection<Earning>? Earnings { get; set; }
        public ICollection<StockPrice>? StockPrices { get; set; }


    }
}
