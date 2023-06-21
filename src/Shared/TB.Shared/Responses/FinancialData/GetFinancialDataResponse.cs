using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.FinancialData
{
    public record GetFinancialDataResponse : Response
    {
        public DateTime Date { get; set; }
        public string? Asset { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal CloseAdjusted { get; set; }
        public decimal SplitCoefficient { get; set; }
    }
}
