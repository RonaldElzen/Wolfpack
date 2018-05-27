using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Event
{
    public class TeamVM
    {
        public IEnumerable<UserVM> Users { get; set; }

        public List<double> AverageSkills { get; set; }
        
        public decimal AveragePersons { get; set; }
    }

    public class UserVM
    {
        public string UserName { get; set; }

        public IEnumerable<double> SkillRatings { get; set; }
    }
}