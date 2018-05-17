using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Models.Event;

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

        /// <summary>
        /// Generate teams for the event based on the teamsize and amount of teams to be made. 
        /// This method tries to put together the most efficient teams.
        /// </summary>
        /// <param name="id">Event id for which to generate teams</param>
        /// <returns>Overview of the new team</returns>
        public ActionResult GenerateTeams(int id)
        {
            var currentEvent = Context.Events.SingleOrDefault(e => e.Id == id);

            currentEvent.Teams.Clear();

            if(currentEvent != null)
            {
                var groupUsers = currentEvent.Group.Users;
                var groupSize = 7; // TODO implement dynamic groupsize
                var amountOfGroups = groupUsers.Count / groupSize; // TODO implement ability to choose amount of groups

                for (int i = 0; i < amountOfGroups; i++)
                {
                    var team = new EventTeam { Name = $"{currentEvent.EventName}-Team {i + 1}" };

                    for (int j = 0; j < groupSize; j++)
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
                        SkillRatings = u.GetSkillRatings()
                    })
                });

                return View(model);
            }

            return HttpNotFound();
        }
    }
}