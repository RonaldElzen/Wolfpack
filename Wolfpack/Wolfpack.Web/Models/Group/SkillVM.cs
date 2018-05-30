using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Group
{
    public class SkillVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}