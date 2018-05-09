using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
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

        /// <summary>
        /// Standard View
        /// </summary>
        /// <returns></returns>
        public ActionResult NewUser()
        {
           
            return View();
        }

        /// <summary>
        /// Login action method
        /// </summary>
        /// <returns>Login page</returns>
        public ActionResult Login()
        {
            if (UserHelper.GetCurrentUser() != null)
                return RedirectToAction("Index", "Home");

            return View(new LoginVM());
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
            using(var context = new Context())
            {
                var user = context.Users
                    .SingleOrDefault(x => x.UserName == vm.LoginName || x.Mail == vm.LoginName);

                if(user == null)
                {
                    ModelState.AddModelError("LoginName", "Invalid combination of username and password");

                    return View("Login", vm);
                }

                if(Hashing.Verify(vm.Password, user.Password))
                {
                    UserHelper.SetCurrentUser(user);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginName", "Invalid combination of username and password");
                    
                    return View("Login", vm);
                }
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
        /// Creates a new useraccount based on the form Account/NewUser
        /// Checks for correct password format and valid data for email
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult NewUserPost(NewUserVM vm)
        {

            if (!string.Equals(vm.Password, vm.PasswordCheck))
            {
                ModelState.AddModelError("Password", "Passwords do not match ");
            }

            if (_isEmailValid(vm.MailAdress))
            {

                using (var context = new Context())
                {
                    context.Users.Add(new User
                    {

                        UserName = vm.UserName,
                        Mail = vm.MailAdress,
                        Password = vm.Password,
                        RegisterDate = DateTime.Now,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName

                    });
                    if (ModelState.IsValid && (vm.Password == vm.PasswordCheck))
                    {
                        context.SaveChanges();
                    }
                    else
                    {


                        return View("NewUser");
                    }

                    return RedirectToAction("NewUserCreated");

                }
            }
            else {
                ModelState.AddModelError("MailAdress", "This email is not valid please try again.");
                return View("NewUser");
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
            catch (FormatException ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Redirection from creation to profile creation
        /// </summary>
        /// <returns></returns>
        public ActionResult NewUserCreated()
        {
            
          return Redirect("/");
        }

    }

}
