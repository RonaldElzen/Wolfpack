﻿using BusinessLayer.Services;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Helpers.Enums;
using Wolfpack.Web.Helpers.Interfaces;
using Wolfpack.Web.Models.Account;

namespace Wolfpack.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(Context context, IUserHelper userHelper = null, ISessionHelper sessionHelper = null) 
            : base(context, userHelper, sessionHelper) { }

        /// <summary>
        /// Standard View
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Login action method
        /// </summary>
        /// <returns>Login page</returns>
        public ActionResult Login(string message = "")
        {
            if (UserHelper.GetCurrentUser() != null)
                return RedirectToAction("Index", "Home");

            return View(new LoginVM() { Message = message });
        }

        /// <summary>
        /// The submit action for the login page. Checks whether the user exists and 
        /// whether the password is correct. Logs in the user if successfull.
        /// </summary>
        /// <param name="vm">The model</param>
        /// <returns>Login page if login failed, else home page</returns>
        [HttpPost]
        public ActionResult Login(LoginVM vm)
        {
            var user = Context.Users
                .SingleOrDefault(x => x.UserName == vm.LoginName || x.Mail == vm.LoginName);

            if (user == null)
            {
                ModelState.AddModelError("LoginName", "Invalid combination of username and password");
                return View("Login", vm);
            }

            if (user.LastLoginAttempt != null && user.LoginAttempts > 0 && user.LoginAttempts < 4)
            {
                if (Hashing.Verify(vm.Password, user.Password))
                {
                    TimeSpan diff = DateTime.Now - user.LastLoginAttempt;
                    if (diff.TotalMinutes > 30)
                        user.LoginAttempts = 0;
                }
            }
            else if (user.LoginAttempts > 3)
            {
                ModelState.AddModelError("Locked", "This account is currently locked. To unlock your account, please check your email");

                string key = Guid.NewGuid().ToString();
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                string link = baseUrl + "Account/Unlock?key=" + key;
                Context.LockedAccounts.Add(new LockedAccount
                {
                    Key = key,
                    User = user
                });
                Context.SaveChanges();

                MailService ms = new MailService();
                ms.SendLoginAttemptMail(user.Mail, link);

                return View("Login", vm);
            }

            user.LastLoginAttempt = DateTime.Now;

            if (Hashing.Verify(vm.Password, user.Password))
            {
                user.LoginAttempts = 0;
                Context.SaveChanges();
                UserHelper.SetCurrentUser(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("LoginName", "Invalid combination of username and password");
                user.LoginAttempts += 1;
                Context.SaveChanges();

                return View("Login", vm);
            }
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        /// <returns>The login view</returns>
        public ActionResult Logout()
        {
            UserHelper.ClearCurrentUser();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Creates a new useraccount based on the form Account/Register
        /// Checks for correct password format and valid data for email
        /// </summary>
        /// <param name="vm"></param>
        /// /// <returns></returns>
        [HttpPost]
        public ActionResult RegisterPost(RegisterVM vm)
        {
            var recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (string.IsNullOrEmpty(recaptchaHelper.Response))
            {
                //This means the captcha had a empty response.
                ModelState.AddModelError("CaptchaResponse", "Please complete the captcha.");
            }

            var recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
                ModelState.AddModelError("CaptchaResponse", "Invalid captcha response.");
            }

            if (!string.Equals(vm.Password, vm.PasswordCheck))
            {
                ModelState.AddModelError("Password", "Passwords do not match ");
            }

            if (_isEmailValid(vm.MailAdress))
            {
                var userExists = Context.Users.Any(x => x.UserName == vm.UserName);
                var mailExists = Context.Users.Any(x => x.Mail == vm.MailAdress);
                if (!userExists && !mailExists)
                {
                    Context.Users.Add(new User
                    {
                        UserName = vm.UserName,
                        Mail = vm.MailAdress,
                        Password = Hashing.Hash(vm.Password),
                        RegisterDate = DateTime.Now,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        LastLoginAttempt = DateTime.Now
                    });
                }
                else
                {
                    if (userExists)
                        ModelState.AddModelError("MailAdress", "Email already in use.");

                    if (mailExists)
                        ModelState.AddModelError("UserName", "Username already in use.");
                }

                if (ModelState.IsValid && (vm.Password == vm.PasswordCheck))
                {
                    Context.SaveChanges();
                }
                else
                {
                    return View("Register", vm);
                }

                return RedirectToAction("Login", "Account", new { message = "Your account has been successfully created!" });
            }
            else
            {
                ModelState.AddModelError("MailAdress", "This email is not valid please try again.");
                return View("Register");
            }
        }

        /// <summary>
        /// Validation check for emailadress
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        private bool _isEmailValid(string emailAddress)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(emailAddress);
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check for a key in the params. If it exists in the database redirect to the reset password form.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult Recovery(RecoveryVM vm)
        {
            if(vm.Key != null)
            {
                Recovery recovery = Context.Recoveries.FirstOrDefault(r => r.Key == vm.Key);
                if (recovery != null)
                {
                    User user = recovery.User;
                    if (user != null)
                    {
                        SessionHelper.SetSessionItem("recoveryKey", vm.Key);
                        return View("Recovery", new RecoveryVM());
                    }
                }
                else
                {
                    return View("Recovery", new RecoveryVM { Status = RecoveryStatus.Invalid });
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
            string key = (string)SessionHelper.GetSessionItem("recoveryKey");
            if (vm.Password == vm.PasswordCheck && key != null)
            {
                Recovery recovery = Context.Recoveries.FirstOrDefault(r => r.Key == key);
                if (recovery != null)
                {
                    User user = recovery.User;
                    if (user != null)
                    {
                        user.Password = Hashing.Hash(vm.Password);
                        Context.Recoveries.Remove(recovery);
                        Context.SaveChanges();
                        return View("Recovery", new RecoveryVM { Status = RecoveryStatus.Changed });
                    }
                }
            }
            return View("Recovery", new RecoveryVM { Status = RecoveryStatus.Failed });
        }

        /// <summary>
        /// Send email for recovery if form on /Account/Recovery is posted
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RecoveryNew(RecoveryVM vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.Email))
            {
                User user = Context.Users.FirstOrDefault(u => u.Mail == vm.Email);
                if (user != null) _resetPassword(vm.Email);
            }
            return View("Recovery", new RecoveryVM { Status = RecoveryStatus.Sent });
        }

        public ActionResult Unlock(UnlockVM vm)
        {
            if (vm.Key != null)
            {
                LockedAccount lockedAccount = Context.LockedAccounts.FirstOrDefault(r => r.Key == vm.Key);
                if (lockedAccount != null)
                {
                    User user = lockedAccount.User;
                    if (user != null)
                    {
                        user.LoginAttempts = 0;
                        Context.LockedAccounts.Remove(lockedAccount);
                        Context.SaveChanges();
                        vm.Status = "succes";
                        return View("Unlock", vm);
                    }
                }
            }
            return View("Unlock");
        }

        /// <summary>
        /// Send email to the email provided with a link for the user to reset his/her password
        /// </summary>
        /// <param name="email"></param>
        private void _resetPassword(string email)
        {
            User user = Context.Users.SingleOrDefault(u => u.Mail == email);
            if (user != null)
            {
                string key = Guid.NewGuid().ToString();
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                string link = baseUrl + "Account/Recovery?key=" + key;
                Context.Recoveries.Add(new Recovery
                {
                    Key = key,
                    User = user
                });
                Context.SaveChanges();
                MailService ms = new MailService();
                ms.SendRecoveryMail(email, link);
            }
        }
    }
}