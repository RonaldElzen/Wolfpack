using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Profile
{
    public class UserVM

    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<SkillVM> Skills { get; set; }
    }
}