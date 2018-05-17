using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Data;

namespace Wolfpack.Web.Controllers
{
    public class EventController : BaseController
    {
        public EventController(Context context) : base(context) { }

        // GET: Event
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateTeams(int id)
        {
            var currentEvent = Context.Events.SingleOrDefault(e => e.Id == id);

            if(currentEvent != null)
            {
                return View("Index");
            }

            return HttpNotFound();
        }
    }
}