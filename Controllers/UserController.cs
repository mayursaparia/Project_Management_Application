using ProjectManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementApp.Controllers
{
    public class UserController : Controller
    {

        ProjectManagementEntities db = new ProjectManagementEntities();

        //Add User
        public ActionResult AddUser(string sortby)
        {
            var user = db.Users.AsQueryable();
            ViewBag.SortByFirstName = sortby == "firstName" ? "firstName desc" : "firstName";
            ViewBag.SortByLastName = sortby == "lastName" ? "lastName desc" : "lastName";
            ViewBag.SortByEmpId = sortby == "employeeId" ? "employeeId desc" : "employeeId";


            switch (sortby)
            {
                case "firstName desc":
                    user = user.OrderByDescending(x => x.firstName);
                    break;
                case "firstName":
                    user = user.OrderBy(x => x.firstName);
                    break;
                case "lastName desc":
                    user = user.OrderByDescending(x => x.lastName);
                    break;
                case "lastName":
                    user = user.OrderBy(x => x.lastName);
                    break;
                case "employeeId desc":
                    user = user.OrderByDescending(x => x.Id);
                    break;
                case "employeeId":
                    user = user.OrderBy(x => x.Id);
                    break;
                default:
                    user = user.OrderByDescending(x => x.Id);
                    break;
            }
            User u = new User();
            u.userlist = user.ToList();
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser([Bind(Include = "firstName,lastName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("AddUser");
            }
            var users = db.Users.AsQueryable();
            User u = new User();
            u.userlist = users.ToList();
            return View(u);
        }

        //Edit User

        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "Id,firstName,lastName,employeeId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddUser");
            }
            return View(user);
        }


        //Delete User
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("AddUser");
        }
    }
}