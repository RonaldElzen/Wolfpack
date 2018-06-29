using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Event
{
    public class RatingToSendVM
    {
        
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}