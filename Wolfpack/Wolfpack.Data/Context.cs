using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data.Migrations;
using Wolfpack.Data.Models;

namespace Wolfpack.Data
{
    public class Context : DbContext
    {
        public Context() : base("WolfPackContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Models.Recovery> Recoveries { get; set; }
        public DbSet<Models.LockedAccount> LockedAccounts { get; set; }
        public DbSet<Models.Skill> Skills { get; set; }
        public DbSet<Models.UserRating> UserRatings { get; set; }
    }
}
