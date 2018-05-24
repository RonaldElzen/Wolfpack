using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Event
{
    public class EditVM
    {
        public int Id { get; set; }
        public String Message { get; set; }
        public String NewSkillName { get; set; }
        public String NewSkillDescription { get; set; }
    }
}