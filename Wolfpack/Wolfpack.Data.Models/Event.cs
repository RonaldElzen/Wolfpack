using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class Event
    {
        public Event()
        {
            Teams = new List<EventTeam>();
            Skills = new List<Skill>();
        }

        public int Id { get; set; }

        public string EventName { get; set; }

        public virtual User EventCreator { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<EventTeam> Teams { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}
