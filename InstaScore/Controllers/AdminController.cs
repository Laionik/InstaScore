﻿using InstaScore.Filters;
using InstaScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InstaScore.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AdminController : Controller
    {
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
            catch(Exception)
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
    }
}
