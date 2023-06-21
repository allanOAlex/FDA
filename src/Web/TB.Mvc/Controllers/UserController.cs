using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using TB.Application.Abstractions.Interfaces;
using TB.Mvc.Session;
using TB.Shared.Constants;
using TB.Shared.Requests.Auth;
using TB.Shared.Requests.User;
using TB.Shared.Responses.Auth;
using TB.Shared.Responses.User;

namespace TB.Mvc.Controllers
{
    public class UserController : BaseController
    {
        private IConfiguration config;
        private readonly HttpClient client;
        private readonly SessionDictionary<string> sessionDictionary;
        private readonly IServiceManager serviceManager;



        public UserController(IConfiguration Configuration, IHttpClientFactory httpClientFactory, SessionDictionary<string> SessionDictionary, IServiceManager ServiceManager) : base(SessionDictionary)
        {
            config = Configuration;
            sessionDictionary = SessionDictionary;
            serviceManager = ServiceManager;
            client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(config["ApiConfig:BaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppConstants.AuthToken);
        }

        public async Task<ActionResult<List<GetAllUsersResponse>>> Index()
        {
            ViewBag.Caption = "Users";

            var response = await serviceManager.AppUserService.FindAll();
            if (response.Count <= 0)
            {
                return View(response);
            }

            return View(response.AsEnumerable());

        }

        public async Task<ActionResult<List<CreateUserResponse>>> Create(CreateUserRequest createUserRequest)
        {
            ViewBag.Caption = "Created User";

            List<CreateUserResponse> createUserResponses = new();

            var response = await serviceManager.AppUserService.Create(createUserRequest);
            if (!response.Successful == true)
            {
                return View(response);

            }

            createUserResponses.Add(response);
            return View("CreatedUser", createUserResponses);


        }

        public IActionResult RegisterView() { return View(); }

        public async Task<ActionResult<CreateUserResponse>> Register(CreateUserRequest createUserRequest)
        {
            ViewBag.Caption = "Created User";

            var response = await serviceManager.AppUserService.Create(createUserRequest);
            if (!response.Successful == true)
            {

                return Json(response, new { success = false });

            }

            return Json(response, new { success = true });

        }

        public IActionResult ForgotPasswordView() { return View(); }

        public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest request)
        {
            request.ResetUrl = $"https://localhost:7208/User/ResetPasswordView/";
            var response = await serviceManager.AuthService.ForgotUserPassword(request);
            if (!response.Successful == true)
            {
                var failedAjaxResponse = new
                {
                    success = true,
                    response
                };

                return Json(failedAjaxResponse);
            }

            var ajaxResponse = new
            {
                success = true,
                response
            };

            return Json(ajaxResponse);

        }

        public IActionResult ResetPasswordView(int userId, string token)
        {
            ViewBag.UserId = userId;
            ViewBag.Token = token;
            return View();
        }

        public async Task<ActionResult<ResetPasswordResponse>> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var response = await serviceManager.AuthService.UserPasswordReset(resetPasswordRequest);
            if (!response.Successful == true)
            {
                var failedAjaxResponse = new
                {
                    success = false,
                    response
                };

                return Json(failedAjaxResponse);

            }

            var ajaxResponse = new
            {
                success = true,
                response
            };

            return Json(ajaxResponse);

        }

        



    }
}
