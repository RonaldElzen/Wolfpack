using BusinessLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // GET: Account2
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Recovery(string key)
        {
            using (var context = new Context())
            {
                Recovery recovery = context.Recoveries.FirstOrDefault(r => r.Key == key);
                if (recovery != null)
                {
                    User user = context.Users.FirstOrDefault(u => u.Id == recovery.User.Id);
                    if(user != null)
                    {
                        var model = new RecoveryVM();
                        return View("Recovery", model);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult RecoveryForm(RecoveryVM vm)
        {
            if(vm.Password == vm.PasswordDouble)
            {
                using (var context = new Context())
                {
                    //int id = getidfromsomewhere?
                    int id = 2;
                    User user = context.Users.FirstOrDefault(u => u.Id == id);
                    if(user != null)
                    {
                        user.Password = vm.Password;
                        context.SaveChanges();
                    }
                }
                return RedirectToAction("Recovery");
            }
            return RedirectToAction("Recovery");
        }

        public void ResetPassword(string email)
        {
            using (var context = new Context())
            {
                User user = context.Users.SingleOrDefault(u => u.Mail == email);
                if(user != null)
                {
                    string key = Guid.NewGuid().ToString();
                    //string link = Url.Action("Recovery", "Account", null);
                    string link = "http://localhost:56401/Account/Recovery?key=" + key;
                    context.Recoveries.Add(new Recovery
                    {
                        Key = key,
                        User = user
                    });
                    context.SaveChanges();
                    MailService ms = new MailService();
                    ms.SendRecoveryMail(email, link);
                }
            }
        }
    }
}