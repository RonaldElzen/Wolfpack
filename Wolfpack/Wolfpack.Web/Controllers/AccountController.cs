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

        /// <summary>
        /// Check for a key in the params. If it exists in the database redirect to the reset password form.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult Recovery(string key)
        {
            if(key != null)
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
                            return View("Recovery", new RecoveryVM());
                        }
                    }
                    else
                    {
                        return RedirectToAction("Recovery", new { Status = "invalid" });
                    }
                }
            }
            return View();
        }

        /// <summary>
        /// New password form. Once posted and passwords are the same + the key is still correct.
        /// The password will be changed and the current recovery code removed
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RecoveryForm(RecoveryVM vm)
        {
            string key = (string) Session["recoveryKey"];
            if (vm.Password == vm.ConfirmPassword && key != null)
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

        /// <summary>
        /// Send email for recovery if form on /Account/Recovery is posted
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RecoveryNew(RecoveryVM vm)
        {
            if (vm.Email != null)
            {
                using (var context = new Context())
                {
                    User user = context.Users.FirstOrDefault(u => u.Mail == vm.Email);
                    if (user != null) ResetPassword(vm.Email);
                }
            }
            return RedirectToAction("Recovery", new { Status = "sent" });
        }

        /// <summary>
        /// Send email to the email provided with a link for the user to reset his/her password
        /// </summary>
        /// <param name="email"></param>
        public void ResetPassword(string email)
        {
            using (var context = new Context())
            {
                User user = context.Users.SingleOrDefault(u => u.Mail == email);
                if(user != null)
                {
                    string key = Guid.NewGuid().ToString();
                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    string link = baseUrl + "Account/Recovery?key=" + key;
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