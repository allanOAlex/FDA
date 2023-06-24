using TB.Shared.Dtos;
using TB.Shared.Requests.FinancialData;
using TB.Shared.Responses.FinancialData;

namespace TB.Application.Abstractions.IServices
{
    public interface IFinancialDataService
    {
        Task<FinancialDataDto> FindAll();

        Task<GetReturnsResponse> CalculateReturns(GetReturnsRequest getReturnsRequest);
        Task<GetVolatilityResponse> CalculateVolatility(GetVolatilityRequest getVolatilityRequest);
        Task<GetCorrelationResponse> CalculateCorrelation(GetCorrelationRequest getCorrelationRequest);

        Task<List<GetDividendResponse>> FetchDividends(GetFinancialDataRequest getFinancialDataRequest);
        Task<List<GetEarningResponse>> FetchEarnings(GetFinancialDataRequest getFinancialDataRequest);
        Task<List<GetStockPriceResponse>> FetchStockPrices(GetFinancialDataRequest getFinancialDataRequest);
        
    }
}
