using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Notification
{
    public class NotificationsVM
    {
        public IEnumerable<NotificationVM> Notifications { get; set; }

        public IEnumerable<NotificationVM> NotificationHistory { get; set; }
    }
}