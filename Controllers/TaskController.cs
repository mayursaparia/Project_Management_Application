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
    public class TaskController : Controller
    {

        ProjectManagementEntities db = new ProjectManagementEntities();


        //Add Task

        public ActionResult AddTask()
        {

            var proj = db.Projects.Where(x => x.status == "In-Process").Select(r => r.name);
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
                if (db.Projects.Any(x => x.name.ToLower() == task.projectName.ToLower()))
                {
                    if (db.Users.Any(y => y.firstName.ToLower() == task.userName.ToLower()))
                    {
                        if (db.Tasks.Any(z => z.taskName.ToLower() == task.taskName.ToLower() && z.projectName.ToLower() == task.projectName.ToLower() && z.startDate == task.startDate && z.endDate == task.endDate))
                        {
                            ModelState.AddModelError("taskName", "Task Allready Present.");
                        }
                        else
                        {
                            if (task.parentTask == null)
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
                    }
                    else
                    {
                        ModelState.AddModelError("userName", "User name not Present.");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("projectName", "Incorrect Project Name.");
                }
            }
            var proj = db.Projects.Where(x => x.status == "In-Process").Select(r => r.name);
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
            return View(db.SP_View_Task(search).ToList());
        }


        //autocomplete project


        [HttpPost]
        public JsonResult GetSearchValue(string Prefix)
        {
            //Note : you can bind same list from database  
            var name = (from c in db.Projects
                             where (c.name.Contains(Prefix) && c.status.Equals("In-Process")) 
                             select new { c.name}).ToList();
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        //autocomplete user

        public JsonResult GetUser(string Prefix)
        {
            //Note : you can bind same list from database  
            var firstName = (from c in db.Users
                        where c.firstName.Contains(Prefix)
                        select new { c.firstName }).ToList();
            return Json(firstName, JsonRequestBehavior.AllowGet);
        }
    }
}