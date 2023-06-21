using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using TB.Application.Abstractions.Interfaces;
using TB.Mvc.Session;
using TB.Shared.Constants;
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
        }


        public async Task<ActionResult<List<GetFinancialDataResponse>>> Index()
        {
            ViewBag.Caption = "Financial Data";
            var response = await serviceManager.FinancialDataService.FindAll();
            if (response!.Count <= 0)
            {
                return View(response);
            }

            return View(response.AsEnumerable());

        }

        public async Task<ActionResult<List<GetFinancialDataResponse>>> FetchFromUrl(GetFinancialDataRequest getFinancialDataRequest)
        {
            ViewBag.Caption = "Financial Data";
            getFinancialDataRequest.DataUrl = config["AppConfig:FDDataUrl"];
            var response = await serviceManager.FinancialDataService.FetchFromUrl(getFinancialDataRequest);
            if (response.Count <= 0)
            {
                
                return View(response);
            }

            return response!;

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
