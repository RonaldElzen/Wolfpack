using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data.Models;

namespace Wolfpack.BusinessLayer.Extensions
{
    public static class SkillExtension
    {
        public static Skill GetByName(this DbSet<Skill> set, string name)
        {
            return set.SingleOrDefault(s => s.Name == name);
        }
    }
}
