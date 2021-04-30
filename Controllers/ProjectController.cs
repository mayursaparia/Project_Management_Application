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
    public class ProjectController : Controller
    {

        ProjectManagementEntities db = new ProjectManagementEntities();


        //Add project
        public ActionResult AddProject()
        {

            var man = db.Managers.Select(r => r.name);
            Project p = new Project
            {
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

            switch (sortby)
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
                default:
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


    }
}