using BusinessLayer.Controllers;
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
    public class Account2Controller : Controller
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
                    User user = context.Users.FirstOrDefault(u => u == recovery.User);
                    return RedirectToAction("RecoveryForm", new { User = user });
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult RecoveryForm(RecoveryVM vm)
        {
            using (var context = new Context())
            {
                //dostuff
            }
            return View();
        }

        public void ResetPassword(string email)
        {
            using (var context = new Context())
            {
                User user = context.Users.SingleOrDefault(u => u.Mail == email);
                if(user != null)
                {
                    string key = Guid.NewGuid().ToString();
                    context.Recoveries.Add(new Recovery
                    {
                        Key = key,
                        User = user
                    });
                    context.SaveChanges();

                    string link = Url.Action("Recovery", "Home", new { Key = key }).ToString();
                    MailService ms = new MailService();
                    ms.SendRecoveryMail(email, link);
                }
            }
        }
    }
}