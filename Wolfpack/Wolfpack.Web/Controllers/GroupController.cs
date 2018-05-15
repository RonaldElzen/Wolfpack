using BusinessLayer.Services;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Models.Group;

namespace Wolfpack.Web.Controllers
{
    public class GroupController : BaseController
  {
        private string _message;

        public GroupController(Context context) : base(context) { }
        public ActionResult New(string message = "")
        {
            return View(new GroupVM() { Message = message });
        }
        
        [HttpPost]
        public ActionResult New(GroupVM vm)
        {        
            if ((!string.IsNullOrWhiteSpace(vm.GroupName) && (!string.IsNullOrWhiteSpace(vm.GroupName))))
            {
                var userId = UserHelper.GetCurrentUser().Id;
                Context.Group.Add(new Group
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
        
        public ActionResult Manager(string message = "")
        {
            return View(new GroupVM() { Message = message });
        }

    }
}