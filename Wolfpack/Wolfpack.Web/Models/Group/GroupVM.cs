using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Group
{
    public class GroupVM
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public int GroupCreator { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}