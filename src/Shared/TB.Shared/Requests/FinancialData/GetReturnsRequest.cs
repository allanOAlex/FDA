using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.FinancialData
{
    public record GetReturnsRequest : Request
    {
        public string? Symbol { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }


    }
}
