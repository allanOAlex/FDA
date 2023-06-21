using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TB.Mvc.Session;

namespace TB.Mvc.Controllers
{
    public class BaseController : Controller
    {
        private readonly SessionDictionary<string> sessionDictionary;

        public BaseController(SessionDictionary<string> SessionDictionary)
        {
             sessionDictionary = SessionDictionary;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (!string.IsNullOrEmpty(sessionDictionary["UserId"]))
                {
                    ViewBag.Name = sessionDictionary["Name"];
                    ViewBag.UserId = int.Parse(sessionDictionary["UserId"]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            

            //SessionExpired();

            base.OnActionExecuting(filterContext);
        }

        public IActionResult SessionExpired()
        {
            return RedirectToAction("Logout", "Auth");
        }
    }
}
