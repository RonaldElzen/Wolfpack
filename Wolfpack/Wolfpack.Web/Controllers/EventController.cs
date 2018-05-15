using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Models.Event;

namespace Wolfpack.Web.Controllers
{
    public class EventController : BaseController
    {
        private string _message;
        public EventController(Context context) : base(context) { }

        public ActionResult New(string message = "")
        {
            return View(new EventVM() { Message = message });
        }
    }
}
