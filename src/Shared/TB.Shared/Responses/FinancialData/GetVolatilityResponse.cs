using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.FinancialData
{
    public record GetVolatilityResponse : Response
    {
        public decimal Volatility { get; set; }
    }
}
