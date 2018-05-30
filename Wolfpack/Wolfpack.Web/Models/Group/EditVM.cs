using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Group
{
    public class EditVM
    {
        public int Id { get; set; }
        public String Message { get; set; }
        public String NewSkillName { get; set; }
        public String NewSkillDescription { get; set; }
        public IEnumerable<EditVMUser> GroupUsers { get; set; }
    }

    public class EditVMUser
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
}