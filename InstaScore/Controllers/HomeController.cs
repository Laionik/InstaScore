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

        [Authorize(Roles = "user")]
        public ActionResult Photo()
        {
            ViewBag.PhotoMessage = "Zdjęcia dostępne w tym tygodniu";
            return View();
        }

        [Authorize(Roles = "user")]
        public ActionResult Compare()
        {
            try
            {
                ViewBag.ResultCMessage = "Porównaj zdjęcia";
                var ph1x = "/Images/ex/";
                var ph2x = "/Images/ex/";
                Random rnd = new Random();
                int ph1, ph2;
                ph1 = rnd.Next(1, 8);
                while (true)
                {
                    ph2 = rnd.Next(1, 8);
                    if (ph2 != ph1)
                    {
                        break;
                    }

                }
                ph1x += "ex" + ph1 + ".jpg";
                ph2x += "ex" + ph2 + ".jpg";

                List<SelectListItem> photos = new List<SelectListItem>();
                photos.Add(new SelectListItem
                {
                    Text = "Zdjęcie " + ph1,
                    Value = ph1.ToString()
                });
                photos.Add(new SelectListItem
                {
                    Text = "Zdjęcie " + ph2,
                    Value = ph2.ToString()
                });

                ViewBag.Photos = photos;
                ViewBag.ph1x = ph1x;
                ViewBag.ph2x = ph2x;
                ViewBag.ph1 = ph1;
                ViewBag.ph2 = ph2;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return RedirectToAction("CompareError", "Error");
            }
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Compare(string win)
        {
            try
            {
                win = Request.Form["Photos"];
                var lost = "";

                if (win != Request.Form["ph1"])
                    lost = Request.Form["ph1"];
                else
                    lost = Request.Form["ph2"];

                ViewBag.ResultCMessage = "Wybrałeś zdjęcie numer " + win;

                /*
                * DODAJ +1 do total dla win i lost oraz +1 do score dla win
                */
                var ph1x = "/Images/ex/";
                var ph2x = "/Images/ex/";
                Random rnd = new Random();
                int ph1, ph2;
                ph1 = int.Parse(win);
                while (true)
                {
                    ph2 = rnd.Next(1, 8);
                    if (ph2 != ph1)
                    {
                        break;
                    }

                }
                ph1x += "ex" + ph1 + ".jpg";
                ph2x += "ex" + ph2 + ".jpg";

                List<SelectListItem> photos = new List<SelectListItem>();
                photos.Add(new SelectListItem
                {
                    Text = "Zdjęcie " + ph1,
                    Value = ph1.ToString()
                });
                photos.Add(new SelectListItem
                {
                    Text = "Zdjęcie " + ph2,
                    Value = ph2.ToString()
                });

                ViewBag.Photos = photos;
                ViewBag.ph1x = ph1x;
                ViewBag.ph2x = ph2x;
                ViewBag.ph1 = ph1;
                ViewBag.ph2 = ph2;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return RedirectToAction("CompareError", "Error");
            }
        }
    }
}
