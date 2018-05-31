using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wolfpack.Web.Models.Group
{
    public class RateVM
    {
        public SkillVM SkillToRate { get; set; }
        public UserVM UserToRate { get; set; }
        public int RateGiverId { get; set; }
        public int GroupId { get; set; }
        public IEnumerable<SkillVM> Skills { get; set; }
        public IEnumerable<UserVM> GroupUsers { get; set; }
        public IEnumerable<SelectListItem> UsersList { get; set; }
        public IEnumerable<SelectListItem> SkillsList { get; set; }
    }
}