using System;
using System.Collections.Generic;
using Wolfpack.Data.Models;

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
        public IEnumerable<SkillVM> Skills { get; set; }
        public IEnumerable<UserVM> GroupUsers { get; set; }
    }
}