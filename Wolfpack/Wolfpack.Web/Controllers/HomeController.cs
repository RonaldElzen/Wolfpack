using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Web.Models.Home;

namespace Wolfpack.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }

        public ActionResult Test(string name)
        {
            var model = new HomeVM { Test = name };

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult FormTest(HomeVM vm)
        {
            return RedirectToAction("Test", new { name = vm.Test });
        }
    }
}