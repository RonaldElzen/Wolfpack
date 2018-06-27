using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data.Models;

namespace Wolfpack.BusinessLayer.Extensions
{
    public static class NotificationExtension
    {
        public static Notification GetById(this DbSet<Notification> set, int id)
        {
            return set.SingleOrDefault(n => n.Id == id);
        }
    }
}
