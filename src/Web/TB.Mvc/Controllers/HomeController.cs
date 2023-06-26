using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TB.Application.Abstractions.Interfaces;
using TB.Mvc.Models;
using TB.Shared.Dtos;
using TB.Shared.Responses.FinancialData;

namespace TB.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IServiceManager serviceManager;

        public HomeController(ILogger<HomeController> Logger, IServiceManager ServiceManager)
        {
            logger = Logger;
            serviceManager = ServiceManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> DashBoard()
        {
            try
            {
                var dashResponse = new FinancialDataDto
                {
                    Dividends = new List<GetDividendResponse>
                    {
                        new GetDividendResponse { Id = 1, Symbol = "AAPL", Dividends = 0.5m },
                        new GetDividendResponse { Id = 2, Symbol = "GOOGL", Dividends = 1.2m },
                        new GetDividendResponse { Id = 3, Symbol = "MSFT", Dividends = 0.8m }
                    },

                    Earnings = new List<GetEarningResponse>
                    {
                        new GetEarningResponse { Id = 1, Symbol = "AAPL", Date = new DateTime(2022, 1, 1), Quater = "Q1", EpsEst = 1.2m, Eps = 1.5m, ReleaseTime = "8:00 AM" },

                        new GetEarningResponse { Id = 2, Symbol = "GOOGL", Date = new DateTime(2022, 1, 1), Quater = "Q1", EpsEst = 2.0m, Eps = 2.5m, ReleaseTime = "9:00 AM" },

                        new GetEarningResponse { Id = 3, Symbol = "MSFT", Date = new DateTime(2022, 1, 1), Quater = "Q1", EpsEst = 1.8m, Eps = 2.2m, ReleaseTime = "10:00 AM" }
                    },

                    StockPrices = new List<GetStockPriceResponse>
                    {
                        new GetStockPriceResponse { Id = 1, Symbol = "AAPL", Date = new DateTime(2022, 1, 1), Open = 150m, High = 160m, Low = 145m, Close = 155m, CloseAdjusted = 152m, Volume = 10000, SplitCoefficient = 1m },

                        new GetStockPriceResponse { Id = 2, Symbol = "GOOGL", Date = new DateTime(2022, 1, 1), Open = 2500m, High = 2550m, Low = 2450m, Close = 2520m, CloseAdjusted = 2505m, Volume = 5000, SplitCoefficient = 1m },

                        new GetStockPriceResponse { Id = 3, Symbol = "MSFT", Date = new DateTime(2022, 1, 1), Open = 300m, High = 310m, Low = 290m, Close = 305m, CloseAdjusted = 303m, Volume = 8000, SplitCoefficient = 1m }
                    }


                };

                var financialData = await serviceManager.FinancialDataService.FindAll();

                ViewBag.Show = true;
                ViewBag.DividendsCaption = "Dividends Data";
                ViewBag.EarningsCaption = "Earnings Data";
                ViewBag.StockPriceCaption = "Stock Price Data";

                return View(financialData);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            try
            {
                if (statusCode.HasValue)
                {
                    if (statusCode.Value == 302)
                    {
                        var viewName = statusCode.ToString();
                        return View(viewName);
                    }
                    else if (statusCode.Value == 400)
                    {

                    }
                    else if (statusCode.Value == 500)
                    {

                    }
                }

                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
            catch (Exception)
            {

                throw;
            }
        }

        

    }
}