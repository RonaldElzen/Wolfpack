using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wolfpack.Data.Models;

namespace Wolfpack.Web.Models.Profile
{
    public class SingleSkillVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfRatings { get; set; }
        public double AverageRating { get; set; }
        public IEnumerable<SkillRatingVM> Ratings { get; set; }
    }
}