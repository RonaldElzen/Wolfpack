using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Data;

namespace Wolfpack.Web.Controllers
{
    public class ProfileController : BaseController
    {
        public ProfileController(Context context) : base(context) { }

        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
    }
}