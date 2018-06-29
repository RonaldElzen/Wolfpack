using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Profile
{
    public class SkillRatingVM
    {
        public double Rating { get; set; }
        public DateTime RatedAt { get; set; }
        public string Comment { get; set; }
        public double AverageMark { get; set; }
    }
}