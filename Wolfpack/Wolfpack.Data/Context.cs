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

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Models.Recovery> Recoveries { get; set; }
        public virtual DbSet<LockedAccount> LockedAccounts { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<EventTeam> EventTeams { get; set; }
        public virtual DbSet<UserSkill> UserSkills { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Models.NewRegister> NewRegisters { get; set; }
        public virtual DbSet<Models.Notification> Notifications { get; set; }
    }
}
