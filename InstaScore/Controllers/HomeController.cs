using InstaScore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaScore.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
           // ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            //ViewBag.Message = "Your app description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Photo()
        {
            ViewBag.PhotoMessage = "Zdjęcia dostępne w tym tygodniu";
            return View();
        }
    }
}
