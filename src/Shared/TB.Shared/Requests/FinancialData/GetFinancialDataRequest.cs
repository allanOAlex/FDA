using TB.Shared.Requests.Common;

namespace TB.Shared.Requests.FinancialData
{
    public record GetFinancialDataRequest : Request
    {
        public string? DataUrl { get; set; }
        


    }
}
