using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Data.Models
{
    public class NewRegister
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int GroupId { get; set; }    
        public string Email { get; set; }
    }
}
