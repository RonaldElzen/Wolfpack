using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data.Models;

namespace Wolfpack.Data
{
    public class Context : DbContext
    {
        public Context() : base("WolfPackContext") { }

        public DbSet<User> Users { get; set; }
    }
}
