using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.FinancialData
{
    public record GetCorrelationResponse : Response
    {
        public decimal Correlation { get; set; }
    }
}
