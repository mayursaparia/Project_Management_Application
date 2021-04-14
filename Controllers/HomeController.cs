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
    public class HomeController : Controller
    {
        ProjectManagementEntities db = new ProjectManagementEntities();

        //Home Page
        public ActionResult Index()
        {
            return View();
        }



        //Add project
        public ActionResult AddProject()
        {
            
            var man = db.Managers.Select(r => r.name);
            Project p = new Project {
                managerlist = new SelectList(man)
            };
            
            //p.manager = "Not Selected";
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProject(Project project)
        {


            if (ModelState.IsValid)
            {
                if (project.startDate == null)
                {
                    project.startDate = DateTime.Now;
                    project.endDate = null;
                }
                
                project.status = "In-Process";
                project.taskNo = 0;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("AddProject");
            }
            var man = db.Managers.Select(r => r.name);
            Project p = new Project
            {
                managerlist = new SelectList(man)
            };
            ViewData["Manager"] = p.managerlist;

            return View(project);
        }
        public ActionResult SearchProject(string sortby)
        {
            var project = db.Projects.AsQueryable();
            ViewBag.SortByStartDate = sortby == "startDate" ? "startDate desc" : "startDate";
            ViewBag.SortByEndDate = sortby == "endDate" ? "endDate desc" : "endDate";
            ViewBag.SortByPriority = sortby == "priority" ? "priority desc" : "priority";
            ViewBag.SortByStatus = sortby == "status" ? "status desc" : "status";

            switch(sortby)
            {
                case "startDate desc":
                    project = project.OrderByDescending(x => x.startDate);
                    break;
                case "startDate":
                    project = project.OrderBy(x => x.startDate);
                    break;
                case "endDate desc":
                    project = project.OrderByDescending(x => x.endDate);
                    break;
                case "endDate":
                    project = project.OrderBy(x => x.endDate);
                    break;
                case "priority desc":
                    project = project.OrderByDescending(x => x.priority);
                    break;
                case "priority":
                    project = project.OrderBy(x => x.priority);
                    break;
                case "status desc":
                    project = project.OrderByDescending(x => x.status);
                    break;
                case "status":
                    project = project.OrderBy(x => x.status);
                    break;
                default :
                    project = project.OrderBy(x => x.name);
                    break;

            }

            return View(project.ToList());
        }


        //Update Project

        public ActionResult UpdateProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);

            if (project.status == "Completed")
            {
                project.startDate = DateTime.Now;
                project.endDate = null;
            }

            if (project == null)
            {
                return HttpNotFound();
            }

            
            return View(project);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProject([Bind(Include = "Id,name,startDate,endDate,priority,taskNo,status,manager")] Project project)
        {
            project.status = "In-Process";

            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddProject");
            }
            return View(project);
        }

        //Suspend 

        public ActionResult SuspendProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuspendProject([Bind(Include = "Id,name,startDate,endDate,priority,taskNo,status,manager")] Project project)
        {
            project.endDate = DateTime.Now;
            project.status = "Completed";
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddProject");
            }
            return View(project);
        }


        //Add User
        public ActionResult AddUser(string sortby)
        {
            var user = db.Users.AsQueryable();
            ViewBag.SortByFirstName = sortby == null ? "firstName desc" : "";
            ViewBag.SortByLastName = sortby == "lastName" ? "lastName desc" : "lastName";
            ViewBag.SortByEmpId = sortby == "employeeId" ? "employeeId desc" : "employeeId";
           

            switch (sortby)
            {
                case "firstName desc":
                    user = user.OrderByDescending(x => x.firstName);
                    break;
                case "lastName desc":
                    user = user.OrderByDescending(x => x.lastName);
                    break;
                case "lastName":
                    user = user.OrderBy(x => x.lastName);
                    break;
                case "employeeId desc":
                    user = user.OrderByDescending(x => x.employeeId);
                    break;
                case "employeeId":
                    user = user.OrderBy(x => x.employeeId);
                    break;
                default :
                    user = user.OrderBy(x => x.firstName);
                    break;
            }
            User u = new User();
            u.userlist = user.ToList();
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser([Bind(Include = "firstName,lastName,employeeId")] User user)
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


        //Add Task

        public ActionResult AddTask()
        {

            var proj = db.Projects.Select(r => r.name);
            var pt = db.Tasks.Select(r => r.parentTask).Distinct();
            var u = db.Users.Select(r => r.firstName);
            Task p = new Task
            {
                projectlist = new SelectList(proj),
                parenttasklist = new SelectList(pt),
                userlist = new SelectList(u)
            };

            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTask(Task task)
        {


            if (ModelState.IsValid)
            {
                if(task.parentTask == null)
                {
                    task.parentTask = task.taskName;
                }


                var id = db.Projects.Where(x => x.name == task.projectName).Select(x => x.Id).FirstOrDefault();
                Project pro = db.Projects.Find(id);
                db.Projects.Remove(pro);
                db.SaveChanges();
                var taskn = Convert.ToInt32(pro.taskNo);
                var tasknu = taskn + 1;
                pro.taskNo = tasknu;
                db.Projects.Add(pro);
                db.SaveChanges();

                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("AddTask");
            }
            var proj = db.Projects.Select(r => r.name);
            var pt = db.Tasks.Select(r => r.parentTask).Distinct();
            var u = db.Users.Select(r => r.firstName);
            Task p = new Task
            {
                projectlist = new SelectList(proj),
                parenttasklist = new SelectList(pt),
                userlist = new SelectList(u)
            };
            ViewData["projectName"] = p.projectlist;

            ViewData["parentTask"] = p.parenttasklist;

            ViewData["userName"] = p.userlist;

            return View(task);
        }


        //View Task

        public ActionResult ViewTask(string search)
        {
            return View(db.Tasks.Where(x => x.taskName.StartsWith(search) || search == null).ToList());
        }



    }
}