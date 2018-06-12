using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Group
{
    public class EditVM
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string NewSkillName { get; set; }
        public string NewSkillDescription { get; set; }
        public IEnumerable<EditVMUser> GroupUsers { get; set; }
    }

    public class EditVMUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}