using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data.Models;

namespace Wolfpack.BusinessLayer.Extensions
{
    public static class RecoveryExtension
    {
        public static Recovery GetByKey(this DbSet<Recovery> set, string key)
        {
            return set.SingleOrDefault(r => r.Key == key);
        }
    }
}
