using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TB.Mvc.Models;

namespace TB.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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