using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class EventTeam
    {
        public EventTeam()
        {
            Users = new List<User>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
