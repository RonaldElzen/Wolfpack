﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data.Models;

namespace Wolfpack.BusinessLayer
{
    public static class EventTeamExtension
    {
        public static EventTeam GetById(this DbSet<EventTeam> set, int id)
        {
            return set.SingleOrDefault(t => t.Id == id);
        }

        public static IDictionary<Skill, double> GetTotalsPerSkill(this EventTeam team)
        {
            var result = new Dictionary<Skill, double>();
            var skills = team.Users.SelectMany(u => u.UserSkills.Select(s => s.Skill)).Distinct();

            foreach(var skill in skills)
            {
                double total = 0;

                foreach(var user in team.Users)
                {
                    total += user.UserSkills.Where(s => s.Skill == skill).FirstOrDefault().Ratings.Average(r => r.Mark);
                }

                result.Add(skill, total);
            }

            return result;
        }
    }
}
