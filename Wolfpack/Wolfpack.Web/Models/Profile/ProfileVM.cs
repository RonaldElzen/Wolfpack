using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Profile
{
    public class ProfileVM
    {
        public String UserName { get; set; }
        public DateTime MemberSince { get; set; }
        public IEnumerable<SkillVM> Skills { get; set; }
    }
}