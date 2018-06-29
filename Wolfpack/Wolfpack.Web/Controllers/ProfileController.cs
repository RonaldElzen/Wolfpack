using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.BusinessLayer.Extensions;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Helpers.Interfaces;
using Wolfpack.Web.Models.Profile;

namespace Wolfpack.Web.Controllers
{
    public class ProfileController : BaseController
    {
        public ProfileController(Context context, IUserHelper userHelper = null, ISessionHelper sessionHelper = null)
            : base(context, userHelper, sessionHelper) { }

        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                id = UserHelper.GetCurrentUser().Id;
            }
            var profileVM = new ProfileVM { };
            var user = Context.Users.GetById(id.Value);
            profileVM.Id = user.Id;
            profileVM.UserName = user.UserName;
            profileVM.MemberSince = user.RegisterDate;
            profileVM.Skills = user.UserSkills
                .Select(s => new SkillVM
                {
                    Id = s.Skill.Id,
                    Name = s.Skill.Name,
                    NumberOfRatings = s.Ratings.Count,
                    AverageRating = s.Ratings.Count > 0 ? s.Ratings.Average(r => r.Mark) : 0,
                    Description = s.Skill.Description
                });
            return View(profileVM);
        }

        public ActionResult SearchProfile()
        {
            return View(new SearchVM());
        }

        [HttpPost]
        public ActionResult SearchProfile(SearchVM vm)
        {
            var user = Context.Users.FirstOrDefault(g => g.UserName == vm.UserName);
            //Return a list with possible users if the username is not found.
            if (user == null)
            {
                var possibleUsers = Context.Users.Select(g => new UserVM
                {
                    Id = g.Id,
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    UserName = g.UserName
                }).Where(g => g.UserName.Contains(vm.UserName));
                return View(new SearchVM { PossibleUsers = possibleUsers });
            }
            else
            {
                return RedirectToAction("Index", new { id = user.Id });
            }
        }

        public ActionResult ViewSkill(int id, int userId)
        {
            var skill = Context.Skills.SingleOrDefault(x => x.Id == id);
            var ratings = Context.Ratings.Where(u => u.UserSkill.User.Id == userId && u.UserSkill.Skill.Id == id).Select(u => new SkillRatingVM
            {
                Rating = u.Mark,
                RatedAt = u.RatedAt,
                Comment = u.Comment
            });

            var finalRatings = new List<SkillRatingVM>();
            var count = 0;
            var totalMark = 0.0;
            foreach(var rating in ratings)
            {
                count++;
                totalMark += rating.Rating;
                finalRatings.Add(new SkillRatingVM
                {
                    AverageMark = Math.Round((totalMark / count), 2),
                    Comment = rating.Comment,
                    Rating = rating.Rating,
                    RatedAt = rating.RatedAt
                });
            }

            var vm = new SingleSkillVM
            {
                Name = skill.Name,
                Description = skill.Description,
                Ratings = finalRatings
            };

            return View(vm);
        }
    }
}