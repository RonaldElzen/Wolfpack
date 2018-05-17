using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class Recovery
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public virtual User User { get; set; }
    }
}
