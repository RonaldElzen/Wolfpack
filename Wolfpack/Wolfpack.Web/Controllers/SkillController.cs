using System;
using System.Linq;
using System.Web.Mvc;
using Wolfpack.Data;

namespace Wolfpack.Web.Controllers
{
    public class SkillController : BaseController
    {
        public SkillController(Context context) : base(context) { }

        /// <summary>
        /// Skills in json format for autocomplete
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetSkills")]
        public ActionResult GetSkills(String prefix)
        {
            var skills = Context.Skills.Where(g => g.Name.StartsWith(prefix)).Select(g => g.Name).ToList();
            return Json(skills);
        }
    }
}