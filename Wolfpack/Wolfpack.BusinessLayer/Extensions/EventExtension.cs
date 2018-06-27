using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data.Models;

namespace Wolfpack.BusinessLayer.Extensions
{
    public static class EventExtension
    {
        public static Event GetById(this DbSet<Event> set, int id)
        {
            return set.SingleOrDefault(e => e.Id == id);
        }
    }
}
