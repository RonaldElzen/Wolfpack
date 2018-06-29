using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data.Models;

namespace Wolfpack.BusinessLayer.Extensions
{
    public static class GroupExtension
    {
        public static Group GetById(this DbSet<Group> set, int id)
        {
            return set.SingleOrDefault(g => g.Id == id);
        }
    }
}
