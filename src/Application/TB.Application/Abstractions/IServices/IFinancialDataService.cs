using TB.Shared.Requests.FinancialData;
using TB.Shared.Responses.FinancialData;

namespace TB.Application.Abstractions.IServices
{
    public interface IFinancialDataService
    {
        Task<List<GetFinancialDataResponse>> FindAll();
        Task<List<GetFinancialDataResponse>> FetchFromUrl(GetFinancialDataRequest getFinancialDataRequest);
        Task<GetReturnsResponse> CalculateReturns(GetReturnsRequest getReturnsRequest);
        Task<GetVolatilityResponse> CalculateVolatility(GetVolatilityRequest getVolatilityRequest);
        Task<GetCorrelationResponse> CalculateCorrelation(GetCorrelationRequest getCorrelationRequest);
    }
}
