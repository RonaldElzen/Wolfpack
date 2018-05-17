﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data;
using Wolfpack.Data.Models;

namespace Wolfpack.BusinessLayer
{
    public static class UserExtension
    {
        /// <summary>
        /// Gets the highest rated skill for the user
        /// </summary>
        public static Skill GetBestSkill(this User user)
        {
            using (var context = new Context())
            {
                return context.UserRatings
                    .Where(r => r.RatedUser.Id == user.Id)
                    .GroupBy(r => r.RatedQuality)
                    .OrderByDescending(r => r.Average(x => x.Rating))
                    .FirstOrDefault()
                    .Key;


            }
        }

        /// <summary>
        /// Gets the lowest rated skill for the user
        /// </summary>
        public static Skill GetWorstSkill(this User user)
        {
            using (var context = new Context())
            {
                return context.UserRatings
                    .Where(r => r.RatedUser.Id == user.Id)
                    .GroupBy(r => r.RatedQuality)
                    .OrderBy(r => r.Average(x => x.Rating))
                    .FirstOrDefault()
                    .Key;
            }
        }

        /// <summary>
        /// Gets all average skill ratings for the user
        /// </summary>
        public static IEnumerable<double> GetSkillRatings(this User user)
        {
            using(var context = new Context())
            {
                return context.UserRatings
                    .Where(r => r.RatedUser.Id == user.Id)
                    .OrderBy(r => r.RatedQuality.Id)
                    .GroupBy(r => r.RatedQuality)
                    .Select(r => r.Average(x => x.Rating))
                    .ToList();
            }
        }
    }
}
