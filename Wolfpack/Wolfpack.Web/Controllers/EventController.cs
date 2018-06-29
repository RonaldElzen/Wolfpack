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
using Wolfpack.Web.Helpers.Enums;
using Wolfpack.Web.Helpers.Interfaces;
using Wolfpack.Web.Models.Event;

namespace Wolfpack.Web.Controllers
{
    public class EventController : BaseController
    {
        public EventController(Context context, IUserHelper userHelper = null, ISessionHelper sessionHelper = null)
            : base(context, userHelper, sessionHelper) { }

        /// <summary>
        /// Show all events of logged-in user
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Get events started by user
            var user = UserHelper.GetCurrentDbUser(Context);
            return View(new UserEventsVM
            {
                CreatedEvents = Context.Events
                    .Where(x => x.EventCreator.Id == user.Id)
                    .Select(e => new EventVM
                    {
                        Id = e.Id,
                        CreatedOn = e.CreatedOn,
                        EventName = e.EventName
                    }),
                ParticipatingEvents = user.EventTeams
                    .Select(e => new EventVM
                    {
                        Id = e.Event.Id,
                        CreatedOn = e.Event.CreatedOn,
                        EventName = e.Event.EventName
                    })
            });
        }

        /// <summary>
        /// View single event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var userId = UserHelper.GetCurrentUser().Id;
            var singleEvent = Context.Events.SingleOrDefault(x => x.Id == id);
            if (singleEvent != null)
            {
                var skills = singleEvent.Skills.Select(s => new SkillVM
                {
                    Description = s.Description,
                    Id = s.Id,
                    Name = s.Name
                });

                var teams = singleEvent.Teams.Select(t => new TeamVM
                {
                    Users = t.Users.Select(u => new UserVM
                    {
                        FirstName = u.FirstName,
                        Id = u.Id,
                        LastName = u.LastName,
                        UserName = u.UserName
                    }),
                    Name = t.Name,
                    Id = t.Id,
                });

                return View(new EventVM
                {
                    CreatedOn = singleEvent.CreatedOn,
                    EventCreator = singleEvent.EventCreator.Id,
                    EventName = singleEvent.EventName,
                    GroupId = singleEvent.Group.Id,
                    Id = singleEvent.Id,
                    Skills = skills,
                    Teams = teams,
                    IsEventCreator = singleEvent.EventCreator.Id == userId
                });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// View edit
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(int Id)
        {
            return View(new EditVM { Id = Id });
        }

        /// <summary>
        /// Form for deleting event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            //TODO: Add actual deletion after post [HttpDelete]? Dont forget to remove all items that depend on an event
            int loggedInUserId = UserHelper.GetCurrentUser().Id;
            var singleEvent = Context.Events.SingleOrDefault(x => x.Id == id && x.EventCreator.Id == loggedInUserId);
            if (singleEvent != null)
            {
                return View(new EventVM
                {
                    EventName = singleEvent.EventName,
                    CreatedOn = singleEvent.CreatedOn
                });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Team(int id)
        {
            var team = Context.EventTeams.GetById(id);
            var vm = new TeamVM
            {
                Name = team.Name,
                Users = team.Users.Select(u => new UserVM
                {
                    FirstName = u.FirstName,
                    Id = u.Id,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    SkillRatings = u.UserSkills.Select(us => new SkillRatingVM
                    {
                        Rating = us.Ratings.Average(r => r.Mark),
                        Name = us.Skill.Name
                    })
                }),
                //SkillNames = team.Event.Skills.Select(s => s.Name) TODO: Only get skills from event not all in users
            };

            vm.SkillNames = vm.Users.First().SkillRatings.Select(x => x.Name).ToList();

            return View(vm);
        }

        /// <summary>
        /// View for generating teams
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult GenerateTeams(int Id)
        {
            var selectListItems = new List<SelectListItem>();

            foreach (var item in Enum.GetValues(typeof(AlgorithmType)))
            {
                selectListItems.Add(new SelectListItem { Value = item.ToString(), Text = item.ToString() });
            }

            return View("GenerateTeamsForm", new GenerateTeamsVM
            {
                EventId = Id,
                AlgorithmTypes = selectListItems,
                AlgorithmType = AlgorithmType.AverageTeams
            });
        }

        public ActionResult RateTeamMembers(int id )
        {
            return View( new { Id = id });
        }

        [HttpPost]
        public ActionResult GenerateTeams(GenerateTeamsVM vm)
        {
            if (vm.TeamSize < 1)
            {
                vm.Message = "Please make sure you have filled in a max team size";
                return View("GenerateTeamsForm", vm);
            }

            var currentEvent = Context.Events.GetById(vm.EventId);

            switch (vm.AlgorithmType)
            {
                case AlgorithmType.AverageTeams:
                    _generateAverageTeams(currentEvent, vm.TeamSize);
                    break;
                case AlgorithmType.BestTeam:
                    _generateBestTeams(currentEvent, vm.TeamSize);
                    break;
                default:
                    return HttpNotFound();
            }

            foreach (var user in currentEvent.Group.Users)
            {
                user.Notifications.Add(new Notification
                {
                    Title = "Added to eventTeam for event: " + currentEvent.EventName,
                    Content = $"An event has started and you've been added to a team. " +
                            $"You can now rate your team members through the following link: " +
                            Url.Action("RateUser", "Group", new { id = currentEvent.Group.Id }, this.Request.Url.Scheme),
                    Date = DateTime.Now,
                    IsRead = false
                });
            }

            return RedirectToAction("Details", new { id = vm.EventId });
        }

        private void _generateAverageTeams(Event currentEvent, int teamSize)
        {
            currentEvent.Teams.Clear();

            if (currentEvent != null && !currentEvent.Group.Archived)
            {
                var groupUsers = currentEvent.Group.Users;

                var amountOfTeams = groupUsers.Count / teamSize; 

                for (int i = 0; i < amountOfTeams; i++)
                {
                    var team = new EventTeam { Name = $"{currentEvent.EventName}-Team {i + 1}" };

                    for (int j = 0; j < teamSize; j++)
                    {
                        if (team.Users.Count > 0)
                        {
                            var usersWithoutOpposite = team.Users.Where(u => !team.Users.Any(x => x != null && u != null && x != u && u.GetBestSkill() == x.GetWorstSkill()));

                            if (usersWithoutOpposite != null && usersWithoutOpposite.Count() > 0)
                            {
                                var user = usersWithoutOpposite.Last();
                                var oppositeUser = groupUsers.Where(u => u != null && user != null && !currentEvent.Teams.Any(t => t.Users.Contains(u))
                                    && !team.Users.Contains(u) && u.GetWorstSkill().Id == user.GetBestSkill().Id).FirstOrDefault();

                                if (oppositeUser != null)
                                {
                                    team.Users.Add(oppositeUser);
                                    continue;
                                }
                            }
                        }

                        team.Users.Add(groupUsers.FirstOrDefault(u => !currentEvent.Teams.Any(t => t.Users.Contains(u)) && !team.Users.Contains(u)));
                        continue;
                    }

                    currentEvent.Teams.Add(team);
                }

                var teamsToChange = GetTeamsToChange(currentEvent);

                while (teamsToChange != null && teamsToChange.Count() > 1)
                {
                    var skillToSwitch = teamsToChange.Select(t => new
                    {
                        Team = t,
                        Skill = t.GetTotalsPerSkill().OrderByDescending(x => x.Value).FirstOrDefault().Key
                    });

                    var orderedTeams = teamsToChange
                        .OrderByDescending(t => t.Users
                            .Sum(u => u.UserSkills
                                .Average(s => s.Ratings
                                    .Average(r => r.Mark))));

                    var bestTeam = orderedTeams.First();
                    var worstTeam = orderedTeams.Last();

                    var skillToChange = worstTeam.GetTotalsPerSkill().OrderBy(s => s.Value).FirstOrDefault().Key;

                    var worstTeamUserToChange = worstTeam.Users
                        .OrderBy(u => u.UserSkills
                            .FirstOrDefault(s => s.Skill == skillToChange)
                            .Ratings
                            .Average(r => r.Mark))
                        .FirstOrDefault();

                    var bestTeamUserToChange = bestTeam.Users
                        .OrderByDescending(u => u.UserSkills
                            .FirstOrDefault(s => s.Skill == skillToChange)
                            .Ratings
                            .Average(r => r.Mark))
                        .FirstOrDefault();

                    worstTeam.Users.Remove(worstTeamUserToChange);
                    worstTeam.Users.Add(bestTeamUserToChange);
                    bestTeam.Users.Remove(bestTeamUserToChange);
                    bestTeam.Users.Add(worstTeamUserToChange);

                    teamsToChange = GetTeamsToChange(currentEvent);
                }

                Context.SaveChanges();

                IEnumerable<EventTeam> GetTeamsToChange(Event e)
                {
                    var totalScorePerTeam = e.Teams.Select(t => new
                    {
                        Team = t,
                        Total = t.Users.Sum(u => u.TotalSkillScore())
                    });

                    var averageScore = totalScorePerTeam.Average(t => t.Total);
                    var allowedDifference = 25;
                    return totalScorePerTeam.Where(t => t.Total > averageScore + allowedDifference || t.Total < averageScore - allowedDifference).Select(x => x.Team);
                }
            }
        }

        private void _generateBestTeams(Event currentEvent, int teamSize)
        {
            currentEvent.Teams.Clear();

            if (currentEvent != null)
            {
                var groupUsers = currentEvent.Group.Users;
                var amountOfTeams = groupUsers.Count / teamSize;

                var orderedGroupUsers = groupUsers
                    .OrderByDescending(u => u.UserSkills
                        .Average(s => s.Ratings
                            .Average(r => r.Mark)));

                for (int i = 0; i < amountOfTeams; i++)
                {
                    var team = orderedGroupUsers
                        .Skip(i * teamSize)
                        .Take(teamSize);

                    currentEvent.Teams.Add(new EventTeam
                    {
                        Name = $"Team {i + 1}",
                        Users = team.ToList()
                    });
                }

                Context.SaveChanges();

                var model = currentEvent.Teams.Select(t => new TeamVM
                {
                    Users = t.Users.Select(u => new UserVM
                    {
                        UserName = u != null ? u.FirstName : "null",
                        SkillRatings = u.GetSkillRatings().Select(x => new SkillRatingVM
                        {
                            Rating = x
                        })
                    })
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GenerateTeamsModal(int Id)
        {
            var selectListItems = new List<SelectListItem>();

            foreach (var item in Enum.GetValues(typeof(AlgorithmType)))
            {
                selectListItems.Add(new SelectListItem { Value = item.ToString(), Text = item.ToString() });
            }

            return PartialView("_generateTeamsPartial", new GenerateTeamsVM
            {
                EventId = Id,
                AlgorithmTypes = selectListItems,
                AlgorithmType = AlgorithmType.AverageTeams
            });
        }


        public ActionResult AddSkillModal(int id)
        {
            return PartialView("_addSkillPartial", new Models.Event.EditVM { Id = id });
        }

        /// <summary>
        /// Form handling for adding skill to Event
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSkill(EditVM vm)
        {
            var currentEvent = Context.Events.GetById(vm.Id);
            var userId = UserHelper.GetCurrentUser().Id;

            Skill skill = Context.Skills.GetByName(vm.NewSkillName);
            if (skill == null)
            {
                skill = new Skill
                {
                    Name = vm.NewSkillName,
                    Description = vm.NewSkillDescription,
                    CreatedBy = Context.Users.GetById(userId),
                };
            };
            currentEvent.Skills.Add(skill);
            Context.SaveChanges();
            return View("Edit", new EditVM { Message = "Skill added" });
        }

        [HttpPost]
        public ActionResult HandleRating(int eventId, int userId, int skillId,IEnumerable<RatingToSendVM> ratings)
        {
            var userToRate = Context.Users.FirstOrDefault(x => x.Id == userId);
            var user = UserHelper.GetCurrentDbUser(Context);
            var userSkill = userToRate.UserSkills.Where(s => s.Skill.Id == skillId).FirstOrDefault();

            //Adding the rating to the database.
            if (userSkill == null)
            {
                userSkill = new UserSkill
                {
                    Skill = Context.Skills.FirstOrDefault(x => x.Id == skillId)
                };
                userToRate.UserSkills.Add(userSkill);
            }

            foreach (var rating in ratings)
            {
                userSkill.Ratings.Add(new Rating
                {
                    Mark = rating.Rating,
                    RatedAt = DateTime.Now,
                    RatedBy = user,
                    Comment = rating.Comment,
                });
            }
            Context.SaveChanges();
            return View("RateTeamMembers",new { state = "success"});
        }

        public ActionResult RatePartial(int userId,int eventId)
        {
            var user = UserHelper.GetCurrentDbUser(Context);
            var currentEventTeam = user.EventTeams.FirstOrDefault(t => t.Event.Id == eventId);
            var userToBeRated = Context.Users.SingleOrDefault(u => u.Id == userId);

            if (currentEventTeam != null && userToBeRated != null)
            {

                var ratedSkills = userToBeRated.UserSkills
                    .Where(s => s.Ratings.Any(r => r.RatedBy.Id == user.Id))
                    .Select(s => new SkillVM
                    {
                        Id = s.Id,
                        Name = s.Skill.Name
                    });

                var allSkills = currentEventTeam.Event.Skills
                    .Select(s => new SkillVM
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .Concat(currentEventTeam.Event.Group.Skills
                     .Select(s => new SkillVM
                     {
                         Id = s.Id,
                         Name = s.Name
                     })).Distinct();

                var skillsToBeRated = allSkills.Where(s => !ratedSkills.Contains(s));

                return PartialView("_rateUserPartial", new RatingVM
                {
                    UserName = userToBeRated.UserName,
                    UserId = userToBeRated.Id,
                    EventId = eventId,
                    Skills = skillsToBeRated
                });
            }

            return HttpNotFound();

        }

        public ActionResult UsersToBeRated(int eventId)
        {
            var user = UserHelper.GetCurrentDbUser(Context);
            var currentEventTeam = user.EventTeams.FirstOrDefault(t => t.Event.Id == eventId);

            if (currentEventTeam != null)
            {
                var skillsToRate = currentEventTeam.Event.Skills
                    .Select(s => s.Id)
                    .Concat(currentEventTeam.Event.Group.Skills
                        .Select(s => s.Id))
                    .Distinct();

                var usersToBeRated = currentEventTeam.Users
                    .Where(u => !u.UserSkills.Where(s => skillsToRate.Contains(s.Skill.Id)).All(s => s.Ratings.Any(r => r.RatedBy.Id == user.Id)))
                    .Select(u => new {
                        id = u.Id,
                        userName = u.UserName
                    });

                return Json(usersToBeRated, JsonRequestBehavior.AllowGet);
            }

            return HttpNotFound();
        }

        public ActionResult SkillsToBeRated(int userId, int eventId)
        {
            var user = UserHelper.GetCurrentDbUser(Context);
            var currentEventTeam = user.EventTeams.FirstOrDefault(t => t.Event.Id == eventId);
            var userToBeRated = Context.Users.SingleOrDefault(u => u.Id == userId);

            if (currentEventTeam != null && userToBeRated != null)
            {
                var ratedSkills = userToBeRated.UserSkills
                    .Where(s => s.Ratings.Any(r => r.RatedBy.Id == user.Id))
                    .Select(s => s.Id);

                var allSkills = currentEventTeam.Event.Skills
                    .Select(s => s.Id)
                    .Concat(currentEventTeam.Event.Group.Skills
                        .Select(s => s.Id))
                    .Distinct();

                var skillsToBeRated = allSkills.Where(s => !ratedSkills.Contains(s));

                return Json(skillsToBeRated, JsonRequestBehavior.AllowGet);
            }

            return HttpNotFound();
        }

        public ActionResult RenameTeamModal(int id)
        {
            var team = Context.EventTeams.GetById(id);

            return PartialView("_renameTeamPartial", new RenameTeamVM
            {
                Id = team.Id,
                Name = team.Name
            });
        }

        [HttpPost]
        public ActionResult RenameTeam(RenameTeamVM vm)
        {
            var team = Context.EventTeams.GetById(vm.Id);
            team.Name = vm.Name;

            Context.SaveChanges();

            return RedirectToAction("Details", new { id = team.Event.Id, state = "success" });
        }
    }
}