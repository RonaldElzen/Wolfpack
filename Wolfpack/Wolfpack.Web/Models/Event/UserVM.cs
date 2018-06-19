using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Event
{
    public class UserVM
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public IEnumerable<SkillRatingVM> SkillRatings { get; set; }
        public IEnumerable<SkillRatingVM> TotalRatings { get; set; }
    }
}