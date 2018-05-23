using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Web.Controllers
{
    public class BaseController : Controller
    {
        protected Context Context { get; set; }

        protected IUserHelper UserHelper { get; set; }

        public BaseController(Context context, IUserHelper userHelper = null)
        {
            Context = context;
            UserHelper = userHelper ?? new UserHelper();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UserHelper.GetCurrentUser() == null && filterContext.Controller as AccountController == null)
            {
                filterContext.Result = new RedirectResult(Url.Action("Login", "Account"));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Context != null)
                Context.Dispose();

            base.Dispose(disposing);
        }
    }
}