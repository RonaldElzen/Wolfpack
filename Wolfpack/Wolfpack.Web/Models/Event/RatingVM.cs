using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Event
{
    public class RatingVM
    {
        public string UserName { get; set; }
        public IEnumerable<SkillVM> Skills { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string RateComment { get; set; }
    }
}