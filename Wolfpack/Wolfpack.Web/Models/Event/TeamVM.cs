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

        public IEnumerable<string> SkillNames { get; set; } 
    }
}