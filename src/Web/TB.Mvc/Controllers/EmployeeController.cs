using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using TB.Application.Abstractions.Interfaces;
using TB.Mvc.Session;
using TB.Shared.Requests.Employee;
using TB.Shared.Responses.Employee;

namespace TB.Mvc.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IServiceManager serviceManager;
        private readonly SessionDictionary<string> sessionDictionary;
        private IConfiguration configuration;
        private readonly HttpClient client;


        public EmployeeController(IServiceManager ServiceManager, AuthenticationStateProvider AuthStateProvider, SessionDictionary<string> SessionDictionary, IConfiguration Configuration, IHttpClientFactory httpClientFactory) : base(SessionDictionary)
        {
            serviceManager = ServiceManager;
            sessionDictionary = SessionDictionary;
            configuration = Configuration;
            client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(configuration["ApiConfig:BaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult<List<GetAllEmployeesResponse>>> Index()
        {
            ViewBag.Caption = "Employees";

            var response = await serviceManager.EmployeeService.FindAll();
            if (response.Count <= 0)
            {
                return View(response);
            }

            return View(response);

        }

        [HttpPost]
        public async Task<ActionResult<UpdateEmployeeSalaryResponse>> UpdateEmployeeSalary(int employeeId, int salary)
        {
            UpdateEmployeeSalaryRequest updateEmployeeSalaryRequest = new()
            {
                Id = employeeId,
                Salary = salary
            };

            var response = await serviceManager.EmployeeService.MySQL_Dapper_UpdateEmployeeSalaryAsync(updateEmployeeSalaryRequest);

            if (!response.Successful == true)
            {
                return Json(response);
            }

            return Json(response);
        }



    }
}
