using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Models.Dummy;

namespace Wolfpack.Web.Controllers
{
    public class DummyController : BaseController
    {
        public User currentUser;
        public DummyController(Context context) : base(context) {
            currentUser = UserHelper.GetCurrentDbUser(context);
        }

        // GET: Dummy
        public ActionResult Index()
        {
            return View("Dummy", new DummyVM());
        }

        public ActionResult Dummy(DummyVM vm)
        {
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateUsersWithSkills()
        {
            string[] names = { "Roxy", "Brenton", "Carmina", "Ricarda", "Cesar", "Aundrea", "Randi", "Errol", "Clarinda", "Phyliss", "Julianne", "Dagmar", "Mervin", "Erminia", "Lovetta", "Tamisha", "Henk", "Cherryl" };
            string[] skills = { "Advising", "Coaching", "Conflict resolution", "Decision making", "Delegating", "Diplomacy", "Interviewing", "Motivation", "People management", "Problem solving", "Strategic thinking" };

            //create users
            foreach (string name in names)
            {
                if (Context.Users.Any(x => x.FirstName == name))
                {
                    return View("Dummy", new DummyVM { Message = "Dummy's have already been created" });
                }
                string username = Guid.NewGuid().ToString();
                User u = new User
                {
                    FirstName = name,
                    LastName = "Placeholder",
                    UserName = username,
                    Mail = username + "@nhlwolfpack.com",
                    Password = Hashing.Hash("Pass!234"),
                    RegisterDate = DateTime.Now,
                    LastLoginAttempt = DateTime.Now
                };
                Context.Users.Add(u);
            }
            Context.SaveChanges();

            //create skills
            foreach (string skillName in skills)
            {
                if(currentUser != null)
                {
                    Skill s;

                    if (Context.Skills.Any(x => x.Name == skillName)) s = Context.Skills.FirstOrDefault(x => x.Name == skillName);
                    else
                    {
                        s = new Skill
                        {
                            Name = skillName,
                            Description = "A skill description",
                            CreatedAt = DateTime.Now,
                            CreatedBy = currentUser
                        };
                        Context.Skills.Add(s);
                    }
                }
                else return View("Dummy", new DummyVM { Message = "No currently logged in user." });              
            }
            Context.SaveChanges();

            //create user ratings for each user and skill
            Random random = new Random();
            foreach (string name in names)
            {
                User u = Context.Users.FirstOrDefault(x => x.FirstName == name);
                if (u != null)
                {
                    foreach (string skillName in skills)
                    {
                        Skill s = Context.Skills.FirstOrDefault(x => x.Name == skillName);
                        if (s != null)
                        {
                            UserRating ur = new UserRating
                            {
                                Rating = Math.Round((random.NextDouble() * 9 + 1), 1),
                                RatedUser = u,
                                RatedBy = u,
                                RatedQuality = s,
                                RatedAt = DateTime.Now
                            };
                            Context.UserRatings.Add(ur);
                        }
                        else return View("Dummy", new DummyVM { Message = "Could not find skill: " + skillName });
                    }
                }
                else return View("Dummy", new DummyVM { Message = "Could not find user: " + name });
            }
            Context.SaveChanges();

            return View("Dummy", new DummyVM { Message = "Users have been added" });
        }
    }
}