using InstaScore.Filters;
using InstaScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;

namespace InstaScore.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        PhotosContext db = new PhotosContext();

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

        [Authorize]
        public ActionResult Photo(int? page)
        {
            ViewBag.PhotoMessage = "Zdjęcia dostępne w tym tygodniu";
            var photos = db.dbphoto.ToList();
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(photos.ToPagedList(currentPageIndex, 50));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Photo()
        {
            int? page = int.Parse(Request.QueryString["page"]);
            ViewBag.PhotoMessage = "Zdjęcia dostępne w tym tygodniu";
            var photos = db.dbphoto.ToList();
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(photos.ToPagedList(currentPageIndex, 50));
        }
        [Authorize]
        public ActionResult Ranking()
        {
            ViewBag.PhotoMessage = "Te zdjęcia były najczęsciej wybierane";
            var photos = db.dbphoto.ToList();
            return View(photos);
        }

        [Authorize(Roles = "user")]
        public ActionResult Compare()
        {
            try
            {
                ViewBag.ResultCMessage = "Porównaj zdjęcia";
                var photoslist = db.dbphoto.ToList();
                Random rnd = new Random();
                int ph1, ph2;
                while (true)
                {
                    ph1 = rnd.Next(1, photoslist.Count());
                    if (photoslist[ph1].photoVisible)
                        break;
                }

                while (true)
                {
                    ph2 = rnd.Next(1, photoslist.Count());
                    if (ph2 != ph1 && photoslist[ph2].photoVisible)
                    {
                        break;
                    }
                }
                List<photos> CompareList = new List<photos>();
                CompareList.Add(photoslist[ph1]);
                CompareList.Add(photoslist[ph2]);
                return View(CompareList);
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
                Random rnd = new Random();
                int ph1, ph2;
                var photoslist = db.dbphoto.ToList();

                /*odczytywanie wyników*/
                win = Request.Form["image"];
                var lost = "";

                if (win != Request.Form["ph1"])
                    lost = Request.Form["ph1"];
                else
                    lost = Request.Form["ph2"];

                /*Update zdjęć*/
                var photoUpdate = photoslist.Find(x => x.photoID == int.Parse(win));
                photoUpdate.photoScore = photoUpdate.photoScore + 1;
                photoUpdate.photoTotal = photoUpdate.photoTotal + 1;

                if (TryUpdateModel(photoUpdate))
                {
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception x)
                    {
                        ViewBag.ErrorMessage = x;
                        return RedirectToAction("DatabaseError", "Error");
                    }
                }
                photoUpdate = photoslist.Find(x => x.photoID == int.Parse(lost));
                photoUpdate.photoTotal = photoUpdate.photoTotal + 1;
                if (TryUpdateModel(photoUpdate))
                {
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception x)
                    {
                        ViewBag.ErrorMessage = x;
                        return RedirectToAction("DatabaseError", "Error");
                    }
                }

                /*losowanie nowych zdjęć*/
                ph1 = photoslist.FindIndex(x => x.photoID == int.Parse(win));
                while (true)
                {
                    ph2 = rnd.Next(0, photoslist.Count() - 1);
                    if (ph2 != ph1 && photoslist[ph2].photoVisible)
                    {
                        break;
                    }
                }
                List<photos> CompareList = new List<photos>();
                CompareList.Add(photoslist[ph1]);
                CompareList.Add(photoslist[ph2]);
                ViewBag.ResultCMessage = "Wybrałeś zdjęcie numer " + win;
                return View(CompareList);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e;
                return RedirectToAction("CompareError", "Error");
            }
        }
    }
}
