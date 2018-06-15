using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
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
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Details(int Id)
        {
            int id = UserHelper.GetCurrentUser().Id;
            var singleEvent = Context.Events.SingleOrDefault(x => x.Id == Id && x.EventCreator.Id == id);
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
                    Avg = t.Users.Average(u => u.UserSkills.Average(s => s.Ratings.Average(r => r.Mark)))
                });

                return View(new EventVM
                {
                    CreatedOn = singleEvent.CreatedOn,
                    EventCreator = singleEvent.EventCreator.Id,
                    EventName = singleEvent.EventName,
                    GroupId = singleEvent.Group.Id,
                    Id = singleEvent.Id,
                    Skills = skills,
                    Teams = teams
                });
            }
            else
            {
                return RedirectToAction("Index", "EventController");
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
            var team = Context.EventTeams.SingleOrDefault(x => x.Id == id);
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
                        Mark = us.Ratings.Average(r => r.Mark),
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
            return View("GenerateTeamsForm", new GenerateTeamsVM { EventId = Id });
        }

        /// <summary>
        /// Form handling for adding skill to Event
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSkill(EditVM vm)
        {
            var currentEvent = Context.Events.FirstOrDefault(g => g.Id == vm.Id);
            var userId = UserHelper.GetCurrentUser().Id;

            Skill skill = Context.Skills.FirstOrDefault(g => g.Name == vm.NewSkillName);
            if (skill == null)
            {
                skill = new Skill
                {
                    Name = vm.NewSkillName,
                    Description = vm.NewSkillDescription,
                    CreatedBy = Context.Users.FirstOrDefault(g => g.Id == userId),
                };
            };
            currentEvent.Skills.Add(skill);
            Context.SaveChanges();
            return View("Edit", new EditVM { Message = "Skill added" });
        }

        [HttpPost]
        public ActionResult GenerateHigestAverageTeams(GenerateTeamsVM vm)
        {
            var currentEvent = Context.Events.SingleOrDefault(e => e.Id == vm.EventId);

            currentEvent.Teams.Clear();
            
            if(currentEvent != null)
            {
                var groupUsers = currentEvent.Group.Users;
                var teamSize = 7; // TODO
                var amountOfTeams = groupUsers.Count / teamSize; // TODO

                var orderedGroupUsers = groupUsers
                    .OrderByDescending(u => u.UserSkills
                        .Average(s => s.Ratings
                            .Average(r => r.Mark)));

                for(int i = 0; i < amountOfTeams; i++)
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
                            Mark = x
                        })
                    })
                });

                return View("GenerateTeams", model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult GenerateTeams(GenerateTeamsVM vm)
        {
            var currentEvent = Context.Events.SingleOrDefault(e => e.Id == vm.EventId);

            currentEvent.Teams.Clear();

            if (currentEvent != null)
            {
                var groupUsers = currentEvent.Group.Users;
                var teamSize = 7; // TODO implement dynamic groupsize

                //TODO Actually implement this
                var teamSizeMin = 0;
                var teamSizeMax = 0;
                var maxTeams = 0;
                if (vm.MinTeamSize > 0)
                    teamSizeMin = vm.MinTeamSize;
                if (vm.MaxTeamSize > 0 && vm.MaxTeamSize >= vm.MinTeamSize)
                    teamSizeMax = vm.MaxTeamSize;
                if (vm.MaxTeamsAmount > 0)
                    maxTeams = vm.MaxTeamsAmount;

                if (teamSizeMin < 1 || teamSizeMax < 1 || maxTeams < 1)
                {
                    vm.Message = "Please make sure you have filled in everything and that max team size isnt higher than min team size";
                    return View("GenerateTeamsForm", vm);
                }

                //set teamsize to max teamsize for now
                teamSize = teamSizeMax;

                var amountOfTeams = groupUsers.Count / teamSize; // TODO implement ability to choose amount of teams

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

                while(teamsToChange != null && teamsToChange.Count() > 1)
                {
                    var skillToSwitch = teamsToChange.Select(t => new
                    {
                        Team = t,
                        Skill = t.GetTotalsPerSkill().OrderByDescending(x => x.Value).FirstOrDefault().Key
                    });

                    teamsToChange = GetTeamsToChange(currentEvent);
                }

                Context.SaveChanges();

                var model = currentEvent.Teams.Select(t => new TeamVM
                {
                    Users = t.Users.Select(u => new UserVM
                    {
                        UserName = u != null ? u.FirstName : "null",
                        SkillRatings = u.GetSkillRatings().Select(x => new SkillRatingVM
                        {
                            Mark = x
                        })
                    })
                });

                return View(model);

                IEnumerable<EventTeam> GetTeamsToChange(Event e)
                {
                    var totalScorePerTeam = e.Teams.Select(t => new
                    {
                        Team = t,
                        Total = t.Users.Sum(u => u.TotalSkillScore())
                    });

                    var averageScore = totalScorePerTeam.Average(t => t.Total);
                    var allowedDifference = 10;
                    return totalScorePerTeam.Where(t => t.Total > averageScore + allowedDifference || t.Total < averageScore - allowedDifference).Select(x => x.Team);
                }
            }

            return HttpNotFound();
        }

        /// <summary>
        /// Generate teams for the event based on the teamsize and amount of teams to be made. 
        /// This method tries to put together the most efficient teams.
        /// </summary>
        /// <param name="id">Event id for which to generate teams</param>
        /// <returns>Overview of the new team</returns>
        [HttpPost]
        public ActionResult GenerateTeamsOld(GenerateTeamsVM vm)
        {
            var currentEvent = Context.Events.SingleOrDefault(e => e.Id == vm.EventId);

            currentEvent.Teams.Clear();

            if (currentEvent != null)
            {
                var groupUsers = currentEvent.Group.Users;
                var teamSize = 7; // TODO implement dynamic groupsize

                //TODO Actually implement this
                var teamSizeMin = 0;
                var teamSizeMax = 0;
                var maxTeams = 0;
                if (vm.MinTeamSize > 0)
                    teamSizeMin = vm.MinTeamSize;
                if (vm.MaxTeamSize > 0 && vm.MaxTeamSize >= vm.MinTeamSize)
                    teamSizeMax = vm.MaxTeamSize;
                if (vm.MaxTeamsAmount > 0)
                    maxTeams = vm.MaxTeamsAmount;

                if (teamSizeMin < 1 || teamSizeMax < 1 || maxTeams < 1)
                {
                    vm.Message = "Please make sure you have filled in everything and that max team size isnt higher than min team size";
                    return View("GenerateTeamsForm", vm);
                }

                //set teamsize to max teamsize for now
                teamSize = teamSizeMax;

                var amountOfTeams = groupUsers.Count / teamSize; // TODO implement ability to choose amount of teams

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

                Context.SaveChanges();

                var model = currentEvent.Teams.Select(t => new TeamVM
                {
                    Users = t.Users.Select(u => new UserVM
                    {
                        UserName = u != null ? u.FirstName : "null",
                        SkillRatings = u.GetSkillRatings().Select(x => new SkillRatingVM
                        {
                            Mark = x
                        })
                    })
                });

                return View(model);
            }

            return HttpNotFound();
        }
    }
}