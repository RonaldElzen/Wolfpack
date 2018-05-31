using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class UserSkill
    {
        public UserSkill()
        {
            Ratings = new List<Rating>();
        }

        public int Id { get; set; }

        public virtual User User { get; set; }

        public virtual Skill Skill { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
