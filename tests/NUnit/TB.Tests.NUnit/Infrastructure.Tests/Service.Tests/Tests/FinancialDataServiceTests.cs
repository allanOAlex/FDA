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
        
        private readonly DBContext? context;
        private readonly MyDBContext? myContext;
        private readonly Dappr? daper;
        private readonly UserManager<AppUser>? userManager;
        private readonly SignInManager<AppUser>? signInManager;
        private readonly IConfiguration? config;

        private IUnitOfWork unitOfWork;
        private MockFinancialDataService service;

        [SetUp]
        public void Setup()
        {

            unitOfWork = new UnitOfWork(context, myContext, daper, userManager, signInManager, config);
            service = new MockFinancialDataService(unitOfWork);
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
                
                // Add more test data as needed
            };

            // Mock the dependencies or use a testing framework like Moq to create mocks
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            // Set up the mock behavior for the stock price repository
            var me = unitOfWorkMock.Setup(u => u.StockPrice.FindAll()).ReturnsAsync(stockPrices.AsQueryable());

            var financialDataService = new MockFinancialDataService(unitOfWorkMock.Object);


            // Act
            //var result = await service.TestCalculateReturns(getReturnsRequest);
            var result = await financialDataService.TestCalculateReturns(getReturnsRequest);

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(new List<decimal>(), result.Returns);
        }

        [Test]
        public async Task CalculateVolatility_ReturnsExpectedVolatility()
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
            Assert.That(result.Volatility, Is.EqualTo(0.0241m));
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

            var returns1 = ((IEnumerable<object>)getCorrelationRequest.Returns1).Select(Convert.ToDecimal).ToList();
            var returns2 = ((IEnumerable<object>)getCorrelationRequest.Returns2).Select(Convert.ToDecimal).ToList();

            // Convert Lists to IQueryable<decimal>
            var returns1Queryable = returns1.AsQueryable();
            var returns2Queryable = returns2.AsQueryable();

            // Act
            var result = await service.TestCalculateCorrelation(new GetCorrelationRequest
            {
                Returns1 = returns1Queryable,
                Returns2 = returns2Queryable
            });

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Correlation, Is.EqualTo(0.6546m));
        }




    }
}
