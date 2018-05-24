using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class Group
    {
        public Group()
        {
            Users = new List<User>();
            Skills = new List<Skill>();
        }
   
        public int Id { get; set; }

        public int GroupCreator { get; set; }

        public string GroupName { get; set; }

        public string Category { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<User> Users { get; set; }
   
        public virtual ICollection<Skill> Skills { get; set; }
    }
}