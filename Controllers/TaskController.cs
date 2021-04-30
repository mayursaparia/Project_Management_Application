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
            return View(db.Tasks.Where(x => x.taskName.StartsWith(search) || search == null).ToList());
        }

    }
}