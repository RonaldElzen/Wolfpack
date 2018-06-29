using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.BusinessLayer.Extensions;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers.Interfaces;
using Wolfpack.Web.Models.Notification;

namespace Wolfpack.Web.Controllers
{
    public class NotificationController : BaseController
    {
        public NotificationController(Context context, IUserHelper userHelper = null, ISessionHelper sessionHelper = null) 
            : base(context, userHelper, sessionHelper) { }

        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the view for a single notification
        /// </summary>
        /// <param name="id">Id of the notification</param>
        /// <returns>HttpNotFound if the id doesn't exist</returns>
        /// <returns>View for the notification</returns>
        public ActionResult GetNotification(int id)
        {
            var notification = Context.Notifications.GetById(id);

            if (notification == null)
                return HttpNotFound();

            notification.IsRead = true;
            Context.SaveChanges();

            return View("Notification", new NotificationVM
            {
                Id = notification.Id,
                Title = notification.Title,
                Content = notification.Content,
                Date = notification.Date,
                IsRead = notification.IsRead
            });
        }

        /// <summary>
        /// Gets a view with a list of all notifications for a user, including a history
        /// </summary>
        /// <returns>View with list of notifications</returns>
        public ActionResult GetNotifications()
        {
            var minDate = DateTime.Now.AddDays(-2);
            var notifications = UserHelper.GetCurrentDbUser(Context).Notifications
                .Where(n => !n.IsRead)
                .Select(n => new NotificationVM
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    Date = n.Date,
                    IsRead = n.IsRead
                }).ToList();

            var notificationHistory = UserHelper.GetCurrentDbUser(Context).Notifications
                .Where(n => n.IsRead)
                .Select(n => new NotificationVM
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    Date = n.Date,
                    IsRead = n.IsRead
                }).ToList();

            var model = new NotificationsVM
            {
                Notifications = notifications,
                NotificationHistory = notificationHistory
            };

            return View("Notifications", model);
        }

        public ActionResult GetNotificationCount()
        {
            var minDate = DateTime.Now.AddDays(-2);
            var notifications = UserHelper.GetCurrentDbUser(Context).Notifications
                .Where(n => n.IsRead == false)
                .ToList();

            return Json(notifications.Count(), JsonRequestBehavior.AllowGet);
        }
    }
}