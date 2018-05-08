using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Models.Account;

namespace Wolfpack.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewUser()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult NewUserPost(NewUserVM vm)
        {
            using (var context = new Context())
            {
                context.Users.Add(new User
                {

                    UserName = vm.UserName,
                    Mail = vm.MailAdress,
                    Password = vm.Password,
                    RegisterDate = DateTime.Now
                   
                });

                context.SaveChanges();
            }
            return RedirectToAction("NewUser");
            // return RedirectToAction("Test", new { name = vm.Test });
        }
    }

}
