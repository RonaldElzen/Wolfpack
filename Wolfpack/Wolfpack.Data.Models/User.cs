using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class User
    {
        public User()
        {
            Groups = new List<Group>();
            EventTeams = new List<EventTeam>();
            UserSkills = new List<UserSkill>();
        }

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        public DateTime RegisterDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int LoginAttempts { get; set; }

        public DateTime LastLoginAttempt { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<EventTeam> EventTeams { get; set; }

        public virtual ICollection<UserSkill> UserSkills { get; set; }
    }
}
