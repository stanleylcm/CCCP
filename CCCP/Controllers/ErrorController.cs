using System.Web.Mvc;

namespace CCCP.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string errorMessage)
        {
            ViewBag.Message = errorMessage;
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
