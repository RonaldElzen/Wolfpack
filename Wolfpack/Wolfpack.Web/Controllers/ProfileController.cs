using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Data;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Models.Profile;
using Wolfpack.Data.Models;

namespace Wolfpack.Web.Controllers
{
    public class ProfileController : BaseController
    {
        public ProfileController(Context context) : base(context) { }

        public ActionResult Index()
        {
            var userId = UserHelper.GetCurrentUser().Id;
            var profileVM = new ProfileVM { };

            profileVM.Skills = Context.UserRatings
                .Where(x => x.RatedUser.Id == userId)
                .GroupBy(u => u.RatedQuality)
                .Select(s => new SkillVM
                {
                    Name = s.Key.Name,
                    NumberOfRatings = s.Count(),
                    AverageRating = s.Average(y => y.Rating),
                    Description = s.Key.Description
                });

            return View(profileVM);
        }
    }
}