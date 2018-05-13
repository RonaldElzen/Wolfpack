using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;

namespace Wolfpack.Web.Controllers
{
    public class BaseController : Controller
    {
        protected Context Context { get; set; }

        protected UserHelper CurrentUser { get; set; }

        public BaseController(Context context)
        {
            Context = context;
            CurrentUser = UserHelper.GetCurrentUser();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Context != null)
                Context.Dispose();

            base.Dispose(disposing);
        }
    }
}