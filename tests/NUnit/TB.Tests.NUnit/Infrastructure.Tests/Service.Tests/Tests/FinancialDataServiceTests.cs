using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IServices;
using TB.Domain.Models;
using TB.Infrastructure.Implementations.Interfaces;
using TB.Persistence.MySQL.MySQL;
using TB.Persistence.SQLServer;
using TB.Shared.Requests.FinancialData;
using TB.Tests.NUnit.Infrastructure.Tests.Repository.Tests.MockRepositories;
using TB.Tests.NUnit.Infrastructure.Tests.Service.Tests.MockServices;

namespace TB.Tests.NUnit.Infrastructure.Tests.Service.Tests.Tests
{
    public class FinancialDataServiceTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IMapper> mapperMock;
        private MockFinancialDataService service;

        [SetUp]
        public void Setup()
        {

            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
            service = new MockFinancialDataService(unitOfWorkMock.Object, mapperMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            unitOfWorkMock.Reset();
            mapperMock.Reset();
        }



        [Test]
        public async Task CalculateReturns_WithValidRequest_ReturnsExpectedReturns()
        {
            // Arrange
            var getReturnsRequest = new GetReturnsRequest
            {
                DateFrom = new DateTime(2022, 1, 1),
                DateTo = new DateTime(2022, 12, 31),
                Symbol = "AAPL"
            };

            // Assuming you have relevant data in the unit of work's stock price repository
            var stockPrices = new List<StockPrice>
            {
                new StockPrice { Date = new DateTime(2022, 1, 1), Symbol = "AAPL", Close = 100 },
            };

            // Set up the mock behavior for the stock price repository
            unitOfWorkMock.Setup(u => u.StockPrice.FindAll()).ReturnsAsync(stockPrices.AsQueryable());

            // Act
            var result = await service.TestCalculateReturns(getReturnsRequest);

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new List<decimal>(), result.Returns);
        }

        [Test]
        public async Task CalculateVolatility_WithValidRequest_ReturnsExpectedVolatility()
        {
            // Arrange
            var getVolatilityRequest = new GetVolatilityRequest
            {
                Returns = new List<decimal> { 0.1m, 0.0909m, 0.05m } 
            };

            // Act
            var result = await service.TestCalculateVolatility(getVolatilityRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(decimal.Round(result.Volatility, 4), Is.EqualTo(0.0217));
        }

        [Test]
        public async Task CalculateCorrelation_WithValidRequest_ReturnsExpectedCorrelation()
        {
            // Arrange
            var getCorrelationRequest = new GetCorrelationRequest
            {
                Returns1 = new List<decimal> { 0.1m, 0.0909m, 0.05m },
                Returns2 = new List<decimal> { 0.2m, 0.1m, 0.15m }
            };


            // Act
            var result = await service.TestCalculateCorrelation(getCorrelationRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(decimal.Round(result.Correlation, 4), Is.EqualTo(0.1708));
        }




    }

}
