using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.FinancialData
{
    public record GetReturnsRequest : Request
    {
        public string? Asset { get; set; }
    }
}
