using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Responses.FinancialData;

namespace TB.Shared.Dtos
{
    public record FinancialDataDto : Dto
    {
        public ICollection<GetDividendResponse>? Dividends { get; set; }
        public ICollection<GetEarningResponse>? Earnings { get; set; }
        public ICollection<GetStockPriceResponse>? StockPrices { get; set; }
    }
}
