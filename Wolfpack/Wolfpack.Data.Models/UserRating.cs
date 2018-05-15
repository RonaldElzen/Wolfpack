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
        public User RatedUser { get; set; }
        public User RatedBy { get; set; }
        public Skill RatedQuality { get; set; }
        public DateTime RatedAt { get; set; }
    }
}
