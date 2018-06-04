﻿using System;
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

        public ActionResult Index(int? id )
        {
            if (!id.HasValue)
            {
                id = UserHelper.GetCurrentUser().Id;
            }
            var profileVM = new ProfileVM { };
            var user = Context.Users.SingleOrDefault(x => x.Id == id);
            profileVM.UserName = user.UserName;
            profileVM.MemberSince = user.RegisterDate;
            profileVM.Skills = user.UserSkills
                .Select(s => new SkillVM
                {
                    Name = s.Skill.Name,
                    NumberOfRatings = s.Ratings.Count,
                    AverageRating = s.Ratings.Average(r => r.Mark),
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
    }
}