using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Event
{
    public class UserEventsVM
    {
       public IEnumerable<EventVM> CreatedEvents { get; set; }
       public IEnumerable<EventVM> ParticipatingEvents { get; set; }
    }
}