using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.FinancialData
{
    public record GetCorrelationRequest : Request
    {
        public dynamic? Returns1 { get; set; }
        public dynamic? Returns2 { get; set; }
    }
}
