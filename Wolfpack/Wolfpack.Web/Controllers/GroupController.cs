using BusinessLayer.Services;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Models.Event;
using Wolfpack.Web.Models.Group;

namespace Wolfpack.Web.Controllers
{
    public class GroupController : BaseController
  {
        private string _message;

        /// <summary>
        /// Standard view for creating a new group
        /// </summary>
        /// <returns></returns>
        public GroupController(Context context) : base(context) { }
        public ActionResult NewGroup(string message = "")
        {
            return View(new GroupVM() { Message = message });
        }

        /// <summary>
        /// Submit action for creating a new group, takes a Name and Catagory from inputfields
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult NewGroup(GroupVM vm)
        {        
            if ((!string.IsNullOrWhiteSpace(vm.GroupName) && (!string.IsNullOrWhiteSpace(vm.GroupName))))
            {
                var userId = UserHelper.GetCurrentUser().Id;
                Context.Groups.Add(new Group
                {
                    GroupName = vm.GroupName,
                    Category = vm.Category,
                    GroupCreator = userId,
                    CreatedOn = DateTime.Now
                });
           
                Context.SaveChanges();
                _message = "Group created!";
            }
            else
            {
                _message = "Something went wrong, please fill in all the fields";
            }
            return View(new GroupVM() { Message = _message });
        }        

         /// <summary>
         /// Standard view for creating a new event
         /// </summary>
         /// <param name="message"></param>
         /// <returns></returns>
         /// 
         public ActionResult NewEvent(string message = "")
         {
             return View(new EventVM() { Message = message });
         }

         /// <summary>
         /// Submit action for creating a new group, takes an eventname from inputfield
         /// </summary>
         /// <param name="vm"></param>
         /// <returns></returns>
        [HttpPost]
         public ActionResult NewEvent(EventVM vm)
         {
            if (!string.IsNullOrWhiteSpace(vm.EventName))
         {
            var userId = UserHelper.GetCurrentUser().Id;
            Context.Events.Add(new Event
            {
               EventName = vm.EventName,
               EventCreator = userId,
               CreatedOn = DateTime.Now
            });

               Context.SaveChanges();
               _message = "Event created!";
            }
            else
            {
               _message = "Something went wrong, please fill in all the fields";
            }
            return View(new EventVM() { Message = _message });
        }
    }
}
