using AutoMapper;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IServices;
using MathNet.Numerics.Statistics;
using Microsoft.EntityFrameworkCore;
using TB.Shared.Responses.FinancialData;
using TB.Shared.Requests.FinancialData;
using CsvHelper;
using System.Globalization;
using TB.Domain.Models;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class FinancialDataService : IFinancialDataService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public FinancialDataService(IUnitOfWork UnitOfWork, IMapper Mapper)
        {
            unitOfWork = UnitOfWork;
            mapper = Mapper;

        }

        public async Task<List<GetFinancialDataResponse>> FindAll()
        {
            try
            {
                List<GetFinancialDataResponse> financialDataList = new();
                var finanncialData = await unitOfWork.FinancialData.FindAll();
                if (finanncialData.Any())
                {
                    foreach (var item in finanncialData)
                    {
                        var listItem = new GetFinancialDataResponse
                        {
                            Id = item.Id,
                            Date = item.Date,
                            Asset = item.Asset,
                            Open = item.Open,
                            High = item.High,
                            Low = item.Low,
                            Close = item.Close,
                            CloseAdjusted = item.CloseAdjusted,
                            SplitCoefficient = item.SplitCoefficient,
                        };

                        financialDataList.Add(listItem);
                    }

                    return financialDataList;
                }

                return financialDataList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GetFinancialDataResponse>> FetchFromUrl(GetFinancialDataRequest getFinancialDataRequest)
        {
            try
            {
                using var httpClient = new HttpClient();
                using var stream = await httpClient.GetStreamAsync(getFinancialDataRequest.DataUrl);
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecordsAsync<StockPrice>();

                var financialDataResponse = new List<GetFinancialDataResponse>();
                if (records != null)
                {
                    await foreach (var record in records)
                    {
                        var financialData = new GetFinancialDataRequest
                        {
                            Date = record.Date,
                            Asset = record.Asset,
                            Open = record.Open,
                            High = record.High,
                            Low = record.Low,
                            Close = record.Close,
                            CloseAdjusted = record.CloseAdjusted,
                            SplitCoefficient = record.SplitCoefficient,

                        };

                        var request = new MapperConfiguration(cfg => cfg.CreateMap<GetFinancialDataRequest, StockPrice>());
                        var response = new MapperConfiguration(cfg => cfg.CreateMap<StockPrice, GetFinancialDataResponse>());

                        IMapper requestMap = request.CreateMapper();
                        IMapper responseMap = response.CreateMapper();

                        var destination = requestMap.Map<GetFinancialDataRequest, StockPrice>(financialData);
                        var itemToCreate = await unitOfWork.FinancialData.Create(destination);
                        var result = responseMap.Map<StockPrice, GetFinancialDataResponse>(itemToCreate);

                        financialDataResponse.Add(result);
                    }

                }

                await unitOfWork.CompleteAsync();
                bool Successful = true;

                return Successful == true ? financialDataResponse : financialDataResponse;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GetCorrelationResponse> CalculateCorrelation(GetCorrelationRequest getCorrelationRequest)
        {
            try
            {
                //var returnArray1 = await getCorrelationRequest.Returns1!.Select(x => (double)x).ToArrayAsync();
                //var returnArray2 = await getCorrelationRequest.Returns2!.Select(x => (double)x).ToArrayAsync();

                var returnArray1 = await ((IQueryable<decimal>)getCorrelationRequest.Returns1!).Select(x => (double)x).ToArrayAsync();
                var returnArray2 = await ((IQueryable<decimal>)getCorrelationRequest.Returns2!).Select(x => (double)x).ToArrayAsync();

                decimal correlationValue = (decimal)Correlation.Pearson(returnArray1, returnArray2);

                var correlation = new GetCorrelationResponse
                {
                    Correlation = correlationValue
                };

                return correlation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetReturnsResponse> CalculateReturns(GetReturnsRequest getReturnsRequest)
        {
            try
            {
                var financialData = await FindAll();
                var assetData = financialData.Where(data => data.Asset == getReturnsRequest.Asset).OrderBy(data => data.Date).ToList();
                var returnsList = new List<decimal>();
                decimal previousClose = 0m;

                foreach (var data in assetData)
                {
                    if (previousClose != 0m)
                    {
                        var returnPercentage = (data.Close - previousClose) / previousClose;
                        returnsList.Add(returnPercentage);
                    }

                    previousClose = data.Close;
                }

                var returns = new GetReturnsResponse
                {
                    Returns = returnsList
                };

                return returns;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<GetVolatilityResponse> CalculateVolatility(GetVolatilityRequest getVolatilityRequest)
        {
            try
            {
                List<decimal> returnList = await getVolatilityRequest.Returns!.ToListAsync();
                int count = returnList.Count;
                decimal sum = returnList.Sum();
                decimal mean = sum / count;
                decimal sumOfSquaredDifferences = returnList.Sum(value => (value - mean) * (value - mean));
                decimal variance = sumOfSquaredDifferences / count;
                decimal standardDeviation = (decimal)Math.Sqrt((double)variance);

                var volatility = new GetVolatilityResponse
                {
                    Volatility = standardDeviation
                };

                return volatility;

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
