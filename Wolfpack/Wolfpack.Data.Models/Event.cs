using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public int EventCreator { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
