using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Data;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Web.Controllers
{
    public class ProfileController : BaseController
    {
        public ProfileController(Context context, IUserHelper userHelper = null, ISessionHelper sessionHelper = null)
            : base(context, userHelper, sessionHelper) { }

        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
    }
}