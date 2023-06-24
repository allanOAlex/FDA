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
using TB.Shared.Dtos;
using TB.Persistence.SQLServer;

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


        public Task<FinancialDataDto> FindAll()
        {
            throw new NotImplementedException();
        }


        public async Task<GetReturnsResponse> CalculateReturns(GetReturnsRequest getReturnsRequest)
        {
            try
            {
                var data = await unitOfWork.StockPrice.FindAll();

                if (!string.IsNullOrEmpty(getReturnsRequest.DateFrom.ToString()) && !string.IsNullOrEmpty(getReturnsRequest.DateTo.ToString()))
                {
                    data = data.Where(d => d.Date >= getReturnsRequest.DateFrom && d.Date <= getReturnsRequest.DateTo);
                }
                else if(!string.IsNullOrEmpty(getReturnsRequest.DateFrom.ToString()) && string.IsNullOrEmpty(getReturnsRequest.DateTo.ToString()))
                {
                    data = data.Where(d => d.Date >= getReturnsRequest.DateFrom);
                }
                else if(string.IsNullOrEmpty(getReturnsRequest.DateFrom.ToString()) && !string.IsNullOrEmpty(getReturnsRequest.DateTo.ToString()))
                {
                    data = data.Where(d => d.Date <= getReturnsRequest.DateTo);
                }

                var assetDatas = data.Where(data => data.Symbol == getReturnsRequest.Symbol).Distinct().OrderBy(data => data.Date).ToList();

                var returnsList = new List<decimal>();
                decimal previousClose = 0m;

                foreach (var assetData in assetDatas)
                {
                    if (previousClose != 0m)
                    {
                        var returnPercentage = (assetData.Close - previousClose) / previousClose;
                        returnsList.Add(returnPercentage);
                    }

                    previousClose = assetData.Close;
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

        public async Task<GetCorrelationResponse> CalculateCorrelation(GetCorrelationRequest getCorrelationRequest)
        {
            try
            {
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




        public async Task<List<GetDividendResponse>> FetchDividends(GetFinancialDataRequest getFinancialDataRequest)
        {
            try
            {
                using var httpClient = new HttpClient();
                using var stream = await httpClient.GetStreamAsync(getFinancialDataRequest.DataUrl);
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecordsAsync<Dividend>();

                var dividendsList = new List<GetDividendResponse>();
                if (records != null)
                {
                    await foreach (var record in records)
                    {
                        var earning = new InsertDividendRequest
                        {
                            Symbol = record.Symbol,
                            Dividends = record.Dividends,
                            
                        };

                        var request = new MapperConfiguration(cfg => cfg.CreateMap<InsertDividendRequest, Earning>());
                        var response = new MapperConfiguration(cfg => cfg.CreateMap<Earning, GetDividendResponse>());

                        IMapper requestMap = request.CreateMapper();
                        IMapper responseMap = response.CreateMapper();

                        var destination = requestMap.Map<InsertDividendRequest, Earning>(earning);
                        var itemToCreate = await unitOfWork.Earning.Create(destination);
                        var result = responseMap.Map<Earning, GetDividendResponse>(itemToCreate);

                        dividendsList.Add(result);
                    }

                }

                await unitOfWork.CompleteAsync();
                bool Successful = true;

                return Successful == true ? dividendsList : dividendsList;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<GetEarningResponse>> FetchEarnings(GetFinancialDataRequest getFinancialDataRequest)
        {
            try
            {
                using var httpClient = new HttpClient();
                using var stream = await httpClient.GetStreamAsync(getFinancialDataRequest.DataUrl);
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecordsAsync<Earning>();

                var earningsList = new List<GetEarningResponse>();
                if (records != null)
                {
                    await foreach (var record in records)
                    {
                        var earning = new InsertEarningRequest
                        {
                            Symbol = record.Symbol,
                            Date = record.Date,
                            Quater = record.Quater,
                            EpsEst = record.EpsEst,
                            Eps = record.Eps,
                            ReleaseTime = record.ReleaseTime,
                            

                        };

                        var request = new MapperConfiguration(cfg => cfg.CreateMap<InsertEarningRequest, Earning>());
                        var response = new MapperConfiguration(cfg => cfg.CreateMap<Earning, GetEarningResponse>());

                        IMapper requestMap = request.CreateMapper();
                        IMapper responseMap = response.CreateMapper();

                        var destination = requestMap.Map<InsertEarningRequest, Earning>(earning);
                        var itemToCreate = await unitOfWork.Earning.Create(destination);
                        var result = responseMap.Map<Earning, GetEarningResponse>(itemToCreate);

                        earningsList.Add(result);
                    }

                }

                await unitOfWork.CompleteAsync();
                bool Successful = true;

                return Successful == true ? earningsList : earningsList;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<GetStockPriceResponse>> FetchStockPrices(GetFinancialDataRequest getFinancialDataRequest)
        {
            try
            {
                using var httpClient = new HttpClient();
                using var stream = await httpClient.GetStreamAsync(getFinancialDataRequest.DataUrl);
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecordsAsync<StockPrice>();

                var stockPriceList = new List<GetStockPriceResponse>();
                if (records != null)
                {
                    await foreach (var record in records)
                    {
                        var stockPrice = new InsertStockPriceRequest
                        {
                            Symbol = record.Symbol,
                            Date = record.Date,
                            Open = record.Open,
                            High = record.High,
                            Low = record.Low,
                            Close = record.Close,
                            CloseAdjusted = record.CloseAdjusted,
                            Volume = record.Volume,
                            SplitCoefficient = record.SplitCoefficient,

                        };

                        var request = new MapperConfiguration(cfg => cfg.CreateMap<InsertStockPriceRequest, StockPrice>());
                        var response = new MapperConfiguration(cfg => cfg.CreateMap<StockPrice, GetStockPriceResponse>());

                        IMapper requestMap = request.CreateMapper();
                        IMapper responseMap = response.CreateMapper();

                        var destination = requestMap.Map<InsertStockPriceRequest, StockPrice>(stockPrice);
                        var itemToCreate = await unitOfWork.StockPrice.Create(destination);
                        var result = responseMap.Map<StockPrice, GetStockPriceResponse>(itemToCreate);

                        stockPriceList.Add(result);
                    }

                }

                await unitOfWork.CompleteAsync();
                bool Successful = true;

                return Successful == true ? stockPriceList : stockPriceList;

            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
