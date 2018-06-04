using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public double Mark { get; set; }

        public virtual User RatedBy { get; set; }

        public DateTime RatedAt { get; set; }

        public string Comment { get; set; }

        public virtual UserSkill UserSkill { get; set; }
    }
}
