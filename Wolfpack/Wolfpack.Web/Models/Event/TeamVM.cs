using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Event
{
    public class TeamVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserVM> Users { get; set; }

        public List<double> AverageSkills { get; set; }

        public double Avg { get; set; }
        public double Total { get; set; }
        public IDictionary<string, double> SkillShit { get; set; }

        public decimal AveragePersons { get; set; }
        public IEnumerable<string> SkillNames { get; set; } 
    }
}