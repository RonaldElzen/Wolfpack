using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.BusinessLayer.Extensions;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Helpers.Interfaces;
using Wolfpack.Web.Models.Dummy;

namespace Wolfpack.Web.Controllers
{
    public class DummyController : BaseController
    {
        public DummyController(Context context, IUserHelper userHelper = null, ISessionHelper sessionHelper = null)
            : base(context, userHelper, sessionHelper) { }

        /// <summary>
        /// Get Dummy Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Dummy", new DummyVM());
        }

        /// <summary>
        /// Get Dummy Page with Dummy VM
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public ActionResult Dummy(DummyVM vm)
        {
            return View(vm);
        }

        /// <summary>
        /// Create dummy data. This will be 18 user with 11 self rated skills.
        /// Also put all these users in a Dummy group.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUsersWithSkills()
        {
            var currentUser = UserHelper.GetCurrentDbUser(Context);
            if (currentUser == null)
                return View("Dummy", new DummyVM { Message = "No currently logged in user." });

            string[] names = { "Roxy", "Brenton", "Carmina", "Ricarda", "Cesar", "Aundrea", "Randi", "Errol", "Clarinda", "Phyliss", "Julianne", "Dagmar", "Mervin", "Erminia", "Lovetta", "Tamisha", "Henk", "Cherryl" };
            string[] skills = { "Advising", "Coaching", "Conflict resolution", "Decision making", "Delegating", "Diplomacy", "Interviewing", "Motivation", "People management", "Problem solving", "Strategic thinking" };

            //Create users
            foreach (string name in names)
            {
                if (Context.Users.Any(x => x.FirstName == name))
                {
                    return View("Dummy", new DummyVM { Message = "Dummy's have already been created" });
                }
                User u = new User
                {
                    FirstName = name,
                    LastName = "Placeholder",
                    UserName = name,
                    Mail = name + "@nhlwolfpack.com",
                    Password = Hashing.Hash("Pass!234"),
                    RegisterDate = DateTime.Now,
                    LastLoginAttempt = DateTime.Now
                };
                Context.Users.Add(u);
            }
            Context.SaveChanges();

            //Create skills
            foreach (string skillName in skills)
            {
                Skill s;

                if (Context.Skills.Any(x => x.Name == skillName))
                    s = Context.Skills.GetByName(skillName);
                else
                {
                    s = new Skill
                    {
                        Name = skillName,
                        Description = "A skill description",
                        CreatedBy = currentUser
                    };
                    Context.Skills.Add(s);
                }            
            }
            Context.SaveChanges();

            //Create user ratings for each user and skill
            for(int i = 0; i < 3; i++)
            {
                Random random = new Random();
                foreach (string name in names)
                {
                    User u = Context.Users.FirstOrDefault(x => x.FirstName == name);
                    if (u != null)
                    {
                        foreach (string skillName in skills)
                        {
                            Skill s = Context.Skills.GetByName(skillName);
                            if (s != null)
                            {
                                var userSkill = u.UserSkills.FirstOrDefault(x => x.Skill.Id == s.Id);

                                if (userSkill == null)
                                {
                                    userSkill = new UserSkill
                                    {
                                        Skill = s,
                                    };
                                    u.UserSkills.Add(userSkill);
                                }

                                var date = DateTime.Now;
                                date.AddDays(i);

                                userSkill.Ratings.Add(new Rating
                                {
                                    RatedBy = u,
                                    RatedAt = DateTime.Now,
                                    Mark = Math.Round((random.NextDouble() * 9 + 1), 1)
                                });
                            }
                            else return View("Dummy", new DummyVM { Message = "Could not find skill: " + skillName });
                        }
                    }
                    else return View("Dummy", new DummyVM { Message = "Could not find user: " + name });
                }
            }
            Context.SaveChanges();

            //Put all users in a dummygroup
            Group group = new Group
            {
                GroupCreator = currentUser.Id,
                GroupName = "DummyGroup",
                Category = "Does this even matter?",
                CreatedOn = DateTime.Now
            };

            foreach (string name in names)
            {
                User u = Context.Users.FirstOrDefault(x => x.FirstName == name);
                if (u != null)
                {
                    group.Users.Add(u);
                }
                else return View("Dummy", new DummyVM { Message = "Could not find user: " + name });
            }
            Context.Groups.Add(group);
            Context.SaveChanges();

            return View("Dummy", new DummyVM { Message = "Users have been added" });
        }

        public ActionResult AlgorithmDummy(int amountOfPeople = 100)
        {
            var rand = new Random();
            var skills = new List<Skill>();
            var group = new Group
            {
                GroupName = "AlgoTest",
                CreatedOn = DateTime.Now,
                GroupCreator = UserHelper.GetCurrentDbUser(Context).Id
            };
            var newEvent = new Event
            {
                Group = group,
                EventName = "AlgoTest",
                EventCreator = UserHelper.GetCurrentDbUser(Context),
                CreatedOn = DateTime.Now
            };

            for (int i = 0; i < 10; i++)
            {
                var skill = new Skill
                {
                    Name = $"Skill {i}",
                    Description = $"Skill {i}",
                };

                group.Skills.Add(skill);
                Context.Skills.Add(skill);
                skills.Add(skill);
            }

            for (int i = 0; i < amountOfPeople; i++)
            {
                var newUser = new User
                {
                    FirstName = $"Algorithm{i}/{amountOfPeople}",
                    LastName = "Algo",
                    Mail = "Algo@algo.algo",
                    Password = Hashing.Hash("test"),
                    RegisterDate = DateTime.Now,
                    UserName = $"Algo{i}/{amountOfPeople}",
                    LastLoginAttempt = DateTime.Now
                };

                foreach(var skill in skills)
                {
                    var userSkill = new UserSkill
                    {
                        Skill = skill,
                    };

                    userSkill.Ratings.Add(new Rating
                    {
                        Mark = rand.Next(1, 11),
                        RatedBy = UserHelper.GetCurrentDbUser(Context),
                        RatedAt = DateTime.Now,
                    });

                    newUser.UserSkills.Add(userSkill);
                }

                group.Users.Add(newUser);
                Context.Users.Add(newUser);
            }

            Context.Events.Add(newEvent);
            Context.Groups.Add(group);

            Context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}