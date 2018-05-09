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
                    if (user != null)
                    {
                        Session["recoveryKey"] = key;
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
            string key = (string) Session["recoveryKey"];
            if (vm.Password == vm.PasswordSame && key != null)
            {
                using (var context = new Context())
                {
                    Recovery recovery = context.Recoveries.FirstOrDefault(r => r.Key == key);
                    if (recovery != null)
                    {
                        User user = context.Users.FirstOrDefault(u => u.Id == recovery.User.Id);
                        if (user != null)
                        {
                            user.Password = vm.Password;
                            context.Recoveries.Remove(recovery);
                            context.SaveChanges();
                            return RedirectToAction("Recovery", new { Status = "changed" });
                        }
                    }
                }
            }
            return RedirectToAction("Recovery", new { Status = "failed" });
        }

        [HttpPost]
        public ActionResult RecoveryNew(RecoveryVM vm)
        {
            if (vm.Email != null)
            {
                using (var context = new Context())
                {
                    User user = context.Users.FirstOrDefault(u => u.Mail == vm.Email);
                    if (user != null)
                    {
                        ResetPassword(vm.Email);
                    }
                }
            }
            return RedirectToAction("Recovery", new { Status = "sent" });
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