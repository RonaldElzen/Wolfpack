using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Event
{
    public class EventVM
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Message { get; set; }
        public int EventCreator { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}