using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(Context context, IUserHelper userHelper = null, ISessionHelper sessionHelper = null)
            : base(context, userHelper, sessionHelper) { }

        public ActionResult Index()
        {
            return View();
        }
    }
}