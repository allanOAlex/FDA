using AutoMapper;
using MathNet.Numerics.Statistics;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Application.Abstractions.Interfaces;
using TB.Shared.Requests.FinancialData;
using TB.Shared.Responses.FinancialData;

namespace TB.Tests.NUnit.Infrastructure.Tests.Service.Tests.MockServices
{
    internal sealed class MockFinancialDataService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MockFinancialDataService(IUnitOfWork UnitOfWork, IMapper Mapper)
        {
            unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        public async Task<GetReturnsResponse> TestCalculateReturns(GetReturnsRequest getReturnsRequest)
        {
            try
            {
                var data = await unitOfWork.StockPrice.FindAll();

                if (!string.IsNullOrEmpty(getReturnsRequest.DateFrom.ToString()) && !string.IsNullOrEmpty(getReturnsRequest.DateTo.ToString()))
                {
                    data = data.Where(d => d.Date >= getReturnsRequest.DateFrom && d.Date <= getReturnsRequest.DateTo);
                }
                else if (!string.IsNullOrEmpty(getReturnsRequest.DateFrom.ToString()) && string.IsNullOrEmpty(getReturnsRequest.DateTo.ToString()))
                {
                    data = data.Where(d => d.Date >= getReturnsRequest.DateFrom);
                }
                else if (string.IsNullOrEmpty(getReturnsRequest.DateFrom.ToString()) && !string.IsNullOrEmpty(getReturnsRequest.DateTo.ToString()))
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

        public async Task<GetVolatilityResponse> TestCalculateVolatility(GetVolatilityRequest getVolatilityRequest)
        {
            try
            {
                List<decimal> returnList = getVolatilityRequest.Returns!;
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

        public async Task<GetCorrelationResponse> TestCalculateCorrelation(GetCorrelationRequest getCorrelationRequest)
        {
            try
            {
                var returns1 = ((IEnumerable<decimal>)getCorrelationRequest.Returns1!).Select(x => (double)x);
                var returns2 = ((IEnumerable<decimal>)getCorrelationRequest.Returns2!).Select(x => (double)x);

                decimal correlationValue = (decimal)Correlation.Pearson(returns1, returns2);

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









    }
}
