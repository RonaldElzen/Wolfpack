using System;
using System.Linq;
using System.Web.Mvc;
using Wolfpack.Data;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Web.Controllers
{
    public class SkillController : BaseController
    {
        public SkillController(Context context, IUserHelper userHelper = null, ISessionHelper sessionHelper = null) 
            : base(context, null, null) { }

        /// <summary>
        /// Skills in json format for autocomplete
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetSkills")]
        public ActionResult GetSkills(string prefix)
        {
            var skills = Context.Skills.Where(g => g.Name.StartsWith(prefix)).Select(g => g.Name).ToList();
            return Json(skills);
        }
    }
}