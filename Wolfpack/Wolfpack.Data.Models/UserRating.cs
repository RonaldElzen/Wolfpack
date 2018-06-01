using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class UserRating
    {
        public int Id { get; set; }

        public double Rating { get; set; }

        public virtual User RatedUser { get; set; }

        public virtual User RatedBy { get; set; }

        public virtual Skill RatedQuality { get; set; }

        public DateTime RatedAt { get; set; }
   
        public String Comment {get; set;}
    }
}
