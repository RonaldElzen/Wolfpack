using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Models.Home;

namespace Wolfpack.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This is a comment to test whether the wiki auto-updates with it
        /// </summary>
        /// <returns></returns>
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
            using(var context = new Context())
            {
                context.Users.Add(new User
                {
                    Mail = $"{vm.Test}@gmail.com",
                    Password = Hashing.Hash("test"),
                    RegisterDate = DateTime.Now,
                    UserName = "Test"
                });

                context.SaveChanges();
            }

            return RedirectToAction("Test", new { name = vm.Test });
        }
    }
}