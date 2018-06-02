using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Group
{
    public class UserGroupsVM
    {
        public IEnumerable<GroupVM> CreatedGroups { get; set; }
        public IEnumerable<GroupVM> ParticipatingGroups { get; set; }
    }
}