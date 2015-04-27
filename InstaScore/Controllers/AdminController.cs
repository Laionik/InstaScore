using InstaScore.Filters;
using InstaScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcPaging;

namespace InstaScore.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AdminController : Controller
    {
        PhotosContext db = new PhotosContext();
        UsersContext dbUser = new UsersContext();
        //
        // GET: /Admin/
        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult RoleCreate()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleCreate(string RoleName)
        {
            Roles.CreateRole(Request.Form["RoleName"]);
            ViewBag.ResultMessage = "Rula utworzona!";
            return RedirectToAction("RoleIndex", "Admin");
        }

        [Authorize(Roles = "admin")]
        public ActionResult RoleIndex()
        {
            var roles = Roles.GetAllRoles();
            return View(roles);
        }

        [Authorize(Roles = "admin")]
        public ActionResult RoleDelete(string RoleName)
        {
            try
            {
                Roles.DeleteRole(RoleName);
                ViewBag.ResultMessage = "Rola usunięta!";
                return RedirectToAction("RoleIndex", "Admin");
            }
            catch (Exception)
            {
                ViewBag.ResultMessage = "Rola jest przypisana do użytkownika/ów!";
                return RedirectToAction("RoleIndex", "Admin");
            }

        }

        /// <summary>
        /// Create a new role to the user
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult RoleAddToUser()
        {
            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;

            return View();
        }

        /// <summary>
        /// Add role to the user
        /// </summary>
        /// <param name="RoleName"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string RoleName, string UserName)
        {

            if (Roles.IsUserInRole(UserName, RoleName))
            {
                ViewBag.ResultMessage = "Użytkownik ma już przypisaną rolę!";
            }
            else
            {
                Roles.AddUserToRole(UserName, RoleName);
                ViewBag.ResultMessage = "Użytkownik został przypisany do roli";
            }

            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;
            return View();
        }

        /// <summary>
        /// Get all the roles for a particular user
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
                SelectList list = new SelectList(Roles.GetAllRoles());
                ViewBag.Roles = list;
            }
            return View("RoleAddToUser");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {

            if (Roles.IsUserInRole(UserName, RoleName))
            {
                Roles.RemoveUserFromRole(UserName, RoleName);
                ViewBag.ResultMessage = "Udało się usunąć rolę użytkownika!";
            }
            else
            {
                ViewBag.ResultMessage = "Użytkownik nie miał przypisanej tej roli.";
            }
            ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
            SelectList list = new SelectList(Roles.GetAllRoles());
            ViewBag.Roles = list;


            return View("RoleAddToUser");
        }

        //
        // GET: /Admin/PhotoManage
        [Authorize(Roles = "admin")]
        public ActionResult PhotoManage(int? page)
        {
            ViewBag.PhotoManage = "Tu możesz zedytować listę dostępnych zdjęć";
            var photos = db.dbphoto.ToList();
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(photos.ToPagedList(currentPageIndex, 20));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult PhotoManage()
        {
            int? page = int.Parse(Request.QueryString["page"]);
            ViewBag.PhotoManage = "Tu możesz zedytować listę dostępnych zdjęć";
            var photos = db.dbphoto.ToList();
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(photos.ToPagedList(currentPageIndex, 20));
        }

        [Authorize(Roles = "admin")]
        public ActionResult PhotoChange()
        {
            int id = int.Parse(Request.QueryString["x"]);
            var page = Request.QueryString["page"];
            var photolist = db.dbphoto.ToList();
            var x = photolist.Find(r => r.photoID == id);
            x.photoVisible = !x.photoVisible;
            if (TryUpdateModel(x))
            {
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = e;
                    return RedirectToAction("DatabaseError", "Error");
                }
            }
            return RedirectToAction("PhotoManage", new { page = page });
        }

        [Authorize(Roles = "admin")]
        public ActionResult UserManage(int? page)
        {
            ViewBag.UserManage = "Zarządzanie użytkownikami";
            var UsersList = dbUser.UserProfiles.ToList();
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(UsersList.ToPagedList(currentPageIndex, 20));
        }

        [Authorize(Roles = "admin")]
        public ActionResult UserEdit()
        {
            int id = int.Parse(Request.QueryString["x"]);
            var page = Request.QueryString["page"];
            var Userslist = dbUser.UserProfiles.ToList();
            ViewBag.Page = page;
            var x = Userslist.Find(r => r.UserId == id);
            return View(x);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult UserEdit(string userid, string username, string usermail)
        {
            UserProfile x = new UserProfile();
            x.UserId = int.Parse(userid);
            x.UserName = username;
            x.UserMail = usermail;
            if (TryUpdateModel(x))
            {
                try
                {
                    dbUser.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = e;
                    return RedirectToAction("DatabaseError", "Error");
                }
            }
            return RedirectToAction("UserManage");
        }

        [Authorize(Roles = "admin")]
        public ActionResult UserDelete()
        {
            int id = int.Parse(Request.QueryString["x"]);
            var page = ViewBag.page;
            var userlist = dbUser.UserProfiles.ToList();
            var x = userlist.Find(r => r.UserId == id);
            foreach (var role in Roles.GetRolesForUser(x.UserName))
                Roles.RemoveUserFromRole(x.UserName, role);
            try
            {
                dbUser.UserProfiles.Remove(x);
                dbUser.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e;
                return RedirectToAction("DatabaseError", "Error");
            }

            return RedirectToAction("UserManage", new { page = page });
        }
    }
}
