using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TB.Mvc.Session;
using TB.Shared.Constants;

namespace TB.Mvc.Controllers
{
    public class ErrorController : BaseController
    {
        private readonly SessionDictionary<string> sessionDictionary;

        public ErrorController(SessionDictionary<string> SessionDictionary) : base(SessionDictionary)
        {
                sessionDictionary = SessionDictionary;
        }

        [Route("Error/HandleStatusCode/{statusCode:int}")]
        public IActionResult HandleStatusCode(int? statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var sessionStatusCode = AppConstants.StatusCode;

            if (statusCode.HasValue)
            {
                switch (statusCode)
                {
                    case 1001:
                        ViewBag.ErrorMessage = $"Sorry, your session has expired. Login to continue.";
                        return RedirectToAction("HandleSessionExpired", "Auth");

                    case 1002:
                        ViewBag.ErrorMessage = $"Token Expired";
                        //return RedirectToAction("HandleTokenExpired", "Auth");
                        return View("HandleTokenExpired");

                    case 1003:
                        ViewBag.ErrorMessage = $"Error creating refresh token";
                        return View("FailedLogin");

                    case 1004:
                        ViewBag.ErrorMessage = $"RefreshToken Expired";
                        return RedirectToAction("HandleSessionExpired", "Auth");

                    case 401:
                        ViewBag.ErrorMessage = $"Sorry, the resource you're looking for was not found.";
                        ViewBag.RouteOfException = statusCodeData!.OriginalPath;
                        return View("401");

                    case 403:
                        ViewBag.ErrorMessage = $"Sorry, the resource you're looking for was not found.";
                        return View("403");

                    case 404:
                        ViewBag.ErrorMessage = $"Sorry, the resource you're looking for was not found.";
                        ViewBag.RouteOfException = statusCodeData!.OriginalPath;
                        return View("404");

                    case 450:
                        ViewBag.ErrorMessage = $"Sorry, the resource you're looking for was not found.";
                        return View("450");

                    case 500:
                        ViewBag.ErrorMessage = $"Something went wrong \n " +
                            $"Please contact system administrator.";
                        return View("500");


                    default:
                        return View("500");

                }
            }
            else
            {
                switch (sessionStatusCode)
                {
                    case 302:
                        ViewBag.ErrorMessage = $"Sorry, your session timed out. Login to continue.";
                        ViewBag.RouteOfException = statusCodeData!.OriginalPath;
                        return View("302");

                    case 401:
                        ViewBag.ErrorMessage = $"Sorry, the resource you're looking for was not found.";
                        ViewBag.RouteOfException = statusCodeData!.OriginalPath;
                        return View("401");

                    case 403:
                        ViewBag.ErrorMessage = $"Sorry, the resource you're looking for was not found.";
                        return View("403");

                    case 404:
                        ViewBag.ErrorMessage = $"Sorry, the resource you're looking for was not found.";
                        ViewBag.RouteOfException = statusCodeData!.OriginalPath;
                        return View("404");

                    case 450:
                        ViewBag.ErrorMessage = $"Sorry, the resource you're looking for was not found.";
                        return View("450");

                    case 500:
                        ViewBag.ErrorMessage = $"Something went wrong \n " +
                            $"Please contact system administrator.";
                        return View("500");
                    default:
                        return View("500");

                }
            }


        }

        [Route("Error/200")]
        public IActionResult HandleSuccess()
        {
            return View("200");
        }

        [Route("Error/201")]
        public IActionResult HandleCreated()
        {
            return View("201");
        }

        [Route("Error/204")]
        public IActionResult HandleNoContent()
        {
            return View("204");
        }

        [Route("Error/302")]
        public IActionResult HandleRedirect()
        {
            return RedirectToAction("HandleSessionTimeOut", "Auth");
        }

        [Route("Error/400")]
        public IActionResult HandleBadRequest()
        {
            return View("400");
        }

        [Route("Error/401")]
        public IActionResult HandleUnAuthorized()
        {
            return View("401");
        }

        [Route("Error/403")]
        public IActionResult HandleForbidden()
        {
            return View("403");
        }

        [Route("Error/404")]
        public IActionResult HandleNotFound()
        {
            return View("404");
        }

        [Route("Error/408")]
        public IActionResult HandleRequestTimeOut() 
        {
            return View("408");
        }

        [Route("Error/409")]
        public IActionResult HandleConflict()
        {
            return View("409");
        }

        [Route("Error/500")]
        public IActionResult HandleInternalServerError()
        {
            return View("500");
        }

        [Route("Error/501")]
        public IActionResult HandleNotImplemented()
        {
            return View("501");
        }

        [Route("Error/503")]
        public IActionResult HandleServiceUnavailable()
        {
            return View("503");
        }

        [Route("Error/504")]
        public IActionResult HandleGatewayTimeOut()
        {
            return View("504");
        }

        


    }
}
