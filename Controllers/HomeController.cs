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
        //Home Page
        public ActionResult Index()
        {
            return View();
        }


    }
}