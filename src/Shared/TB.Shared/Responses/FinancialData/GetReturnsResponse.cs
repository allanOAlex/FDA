using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.FinancialData
{
    public record GetReturnsResponse : Response
    {
        public List<decimal>? Returns { get; set; }
    }
}
