using System;
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
                return user.UserSkills
                    .OrderByDescending(r => r.Ratings.Average(a => a.Mark))
                    .FirstOrDefault()
                    .Skill;
            }
        }

        /// <summary>
        /// Gets the lowest rated skill for the user
        /// </summary>
        public static Skill GetWorstSkill(this User user)
        {
            using (var context = new Context())
            {
                return user.UserSkills
                    .OrderBy(r => r.Ratings.Average(a => a.Mark))
                    .FirstOrDefault()
                    .Skill;
            }
        }

        /// <summary>
        /// Gets all average skill ratings for the user
        /// </summary>
        public static IEnumerable<double> GetSkillRatings(this User user)
        {
            using(var context = new Context())
            {
                return user.UserSkills
                    .Select(u => u.Ratings.Average(r => r.Mark))
                    .ToList();
            }
        }

        public static double AverageSkillScore(this User user)
        {
            return user.UserSkills.Average(s => s.Ratings.Average(r => r.Mark));
        }

        public static double TotalSkillScore(this User user)
        {
            return user.UserSkills.Sum(s => s.Ratings.Average(r => r.Mark));
        }
    }
}
