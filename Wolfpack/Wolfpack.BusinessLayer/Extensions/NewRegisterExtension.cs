using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data.Models;

namespace Wolfpack.BusinessLayer.Extensions
{
    public static class NewRegisterExtension
    {
        public static NewRegister GetByKey(this DbSet<NewRegister> set, string key)
        {
            return set.SingleOrDefault(x => x.Key == key);
        }
    }
}
