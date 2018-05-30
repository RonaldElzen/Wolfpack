using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wolfpack.Data.Models;

namespace Wolfpack.Web.Models.Event
{
    public class EventVM
    {
        public int Id { get; set; }

        public string EventName { get; set; }

        public string Message { get; set; }

        public int EventCreator { get; set; }

        public DateTime CreatedOn { get; set; }

        public int GroupId { get; set; }
        public IEnumerable<SkillVM> Skills { get; set; }
    }
}