using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using TB.Application.Abstractions.Interfaces;
using TB.Mvc.Session;
using TB.Shared.Constants;
using TB.Shared.Dtos;
using TB.Shared.Requests.FinancialData;
using TB.Shared.Responses.FinancialData;

namespace TB.Mvc.Controllers
{
    public class FinancialDataController : BaseController
    {
        private IConfiguration config;
        private readonly SessionDictionary<string> sessionDictionary;
        private readonly HttpClient client;
        private readonly IServiceManager serviceManager;
        private readonly string DataUrl;


        public FinancialDataController(IConfiguration Configuration, SessionDictionary<string> SessionDictionary, IHttpClientFactory httpClientFactory, IServiceManager ServiceManager) : base(SessionDictionary)
        {
            config = Configuration;
            serviceManager = ServiceManager;
            sessionDictionary = SessionDictionary;
            client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(config["ApiConfig:BaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppConstants.AuthToken);

            DataUrl = config["AppConfig:FDDataUrl"]!;
        }


        public async Task<ActionResult<FinancialDataDto>> FetchAll()
        {
            ViewBag.Caption = "Financial Data";
            var response = await serviceManager.FinancialDataService.FindAll();
            if (!response.Succesful == true)
            {
                return View(response);
            }

            return View(response);

        }

        public async Task<ActionResult<List<GetDividendResponse>>> Dividends()
        {
            ViewBag.Caption = "Dividends";
            var response = await serviceManager.FinancialDataService.Dividends();
            if (response.Count() <= 0)
            {
                return View(response);
            }

            return View(response);

        }

        public async Task<ActionResult<List<GetEarningResponse>>> Earnings()
        {
            ViewBag.Caption = "Earnings";
            var response = await serviceManager.FinancialDataService.Earnings();
            if (response.Count() <= 0)
            {
                return View(response);
            }

            return View(response);

        }

        public async Task<ActionResult<List<GetStockPriceResponse>>> StockPrices()
        {
            ViewBag.Caption = "Stock Prices";
            var response = await serviceManager.FinancialDataService.StockPrices();
            if (response.Count() <= 0)
            {
                return View(response);
            }

            return View(response);

        }



        public async Task<ActionResult<List<GetDividendResponse>>> FetchDividends(GetFinancialDataRequest getFinancialDataRequest)
        {
            ViewBag.Caption = "Dividends";
            var response = await serviceManager.FinancialDataService.FetchDividends(getFinancialDataRequest);
            if (response!.Count <= 0)
            {
                return Json(response);
            }

            return Json(response);

        }

        public async Task<ActionResult<List<GetEarningResponse>>> FetchEarnings(GetFinancialDataRequest getFinancialDataRequest)
        {
            ViewBag.Caption = "Earnings";
            var response = await serviceManager.FinancialDataService.FetchEarnings(getFinancialDataRequest);
            if (response!.Count <= 0)
            {
                return Json(response);
            }

            return Json(response);

        }

        public async Task<ActionResult<List<GetStockPriceResponse>>> FetchStockPrices(GetFinancialDataRequest getFinancialDataRequest)
        {
            ViewBag.Caption = "Stock Prices";
            var response = await serviceManager.FinancialDataService.FetchStockPrices(getFinancialDataRequest);
            if (response!.Count <= 0)
            {
                return Json(response);
            }

            return Json(response);

        }

        

        public async Task<ActionResult<GetReturnsResponse>> GetReturns(GetReturnsRequest getReturnsRequest)
        {
            ViewBag.Caption = "Returns";
            var response = await serviceManager.FinancialDataService.CalculateReturns(getReturnsRequest);
            if (!response.Successful == true)
            {
                return View(response);
            }

            return response!;

        }

        public async Task<ActionResult<GetVolatilityResponse>> GetVolatility(GetVolatilityRequest getVolatilityRequest)
        {
            ViewBag.Caption = "Volatility";
            var response = await serviceManager.FinancialDataService.CalculateVolatility(getVolatilityRequest);
            if (!response.Successful == true)
            {
                return View(response);
            }

            return response;

        }

        [HttpPost]
        public async Task<ActionResult<GetCorrelationResponse>> GetCorrelation(GetCorrelationRequest getCorrelationRequest)
        {
            ViewBag.Caption = "Correlation";
            var response = await serviceManager.FinancialDataService.CalculateCorrelation(getCorrelationRequest);
            if (!response.Successful == true)
            {
                
                return View(response);
            }

            return response;

        }



    }
}
