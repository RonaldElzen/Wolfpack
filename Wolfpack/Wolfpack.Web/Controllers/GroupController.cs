﻿using BusinessLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers.Interfaces;
using Wolfpack.Web.Models.Group;

namespace Wolfpack.Web.Controllers
{
    public class GroupController : BaseController
    {
        /// <summary>
        /// Standard view for creating a new group
        /// </summary>
        /// <returns></returns>
        public GroupController(Context context, IUserHelper userHelper = null, ISessionHelper sessionHelper = null)
            : base(context, userHelper, sessionHelper) { }

        /// <summary>
        /// View all groups
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string message)
        {
            var user = UserHelper.GetCurrentDbUser(Context);
            var groups = Context.Groups.Where(x => x.GroupCreator == user.Id && !x.Archived).Select(g => new GroupVM
            {
                Id = g.Id,
                Category = g.Category,
                CreatedOn = g.CreatedOn,
                GroupName = g.GroupName
            });

            //Get the groups in which user participates
            var participatingGroups = user.Groups.Select(g => new GroupVM
            {
                Id = g.Id,
                Category = g.Category,
                CreatedOn = g.CreatedOn,
                GroupName = g.GroupName,
                Archived = g.Archived
            });

            return View(new UserGroupsVM{CreatedGroups = groups, ParticipatingGroups = participatingGroups });
        }

        /// <summary>
        /// Method to rate a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RateUser(int id)
        {
            var currentGroup = Context.Groups.SingleOrDefault(x => x.Id == id);
            var skills = currentGroup.Skills.Select(s => new Models.Group.SkillVM
            {
                Description = s.Description,
                Id = s.Id,
                Name = s.Name
            });

            var groupUsers = currentGroup.Users.Select(u => new Models.Group.UserVM
            {
                FirstName = u.FirstName,
                Id = u.Id,
                LastName = u.LastName,
                UserName = u.UserName
            });

            var userList = groupUsers.Select(a => new SelectListItem() { Text = a.FirstName, Value = a.Id.ToString()});
            var skillsList = skills.Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString()});
            return View(new Models.Group.RateVM {GroupId = id,SkillsList = skillsList, UsersList = userList});
        }

        [HttpPost]
        public ActionResult SubmitRating(RateVM vm)
        {
            //Gets the user and skill that needs to be rated. 
            var userToRate = Context.Users.FirstOrDefault(x => x.Id == vm.UserToRateId);
            var skillToRate = Context.Skills.FirstOrDefault(x => x.Id == vm.SkillToRateId);
            var userSkill = userToRate.UserSkills.Where(s => s.Skill.Id == skillToRate.Id).FirstOrDefault();
            
            //Adding the rating to the database.
            if(userSkill == null)
            {
                userSkill = new UserSkill
                {
                    Skill = skillToRate
                };

                userToRate.UserSkills.Add(userSkill);
            }

            userSkill.Ratings.Add(new Rating
            {
                Mark = vm.Rating,
                RatedAt = DateTime.Now,
                RatedBy = UserHelper.GetCurrentDbUser(Context),
                Comment = vm.RateComment
            });

            Context.SaveChanges();
            return RedirectToAction("Index", "Group");
        }

        /// <summary>
        /// Remove provided user from provided group
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ActionResult RemoveUserFromGroup(int userId, int groupId)
        {
            int loggedInUserId = UserHelper.GetCurrentUser().Id;
            var singleGroup = Context.Groups.SingleOrDefault(x => x.Id == groupId && x.GroupCreator == loggedInUserId);
            if (singleGroup != null)
            {
                var user = Context.Users.FirstOrDefault(x => x.Id == userId);
                singleGroup.Users.Remove(user);
                Context.SaveChanges();

                //TODO Send notification to removed user (waiting for notification system)
            }
            return RedirectToAction("Details", new { Id = groupId,state = "success" });
        }

        /// <summary>
        /// View single group
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            int loggedInUserId = UserHelper.GetCurrentUser().Id;
            var singleGroup = Context.Groups.SingleOrDefault(x => x.Id == id);
            if (singleGroup != null)
            {
                var skills = singleGroup.Skills.Select(s => new Models.Group.SkillVM
                {
                    Description = s.Description,
                    Id = s.Id,
                    Name = s.Name
                });

                var groupUsers = singleGroup.Users.Select(u => new Models.Group.UserVM
                {
                    FirstName = u.FirstName,
                    Id = u.Id,
                    LastName = u.LastName,
                    UserName = u.UserName
                });

                return View(new GroupVM
                {
                    Category = singleGroup.Category,
                    CreatedOn = singleGroup.CreatedOn,
                    GroupCreator = singleGroup.GroupCreator,
                    GroupName = singleGroup.GroupName,
                    Id = singleGroup.Id,
                    Skills = skills,
                    GroupUsers = groupUsers,
                    Archived = singleGroup.Archived
                });
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Form for deleting group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            //TODO: Add actual deletion after post [HttpDelete]? Dont forget to remove all items that depend on a group
            int loggedInUserId = UserHelper.GetCurrentUser().Id;
            var singleGroup = Context.Groups.FirstOrDefault(x => x.Id == id && x.GroupCreator == loggedInUserId);
            if (singleGroup != null)
            {
                return View(new GroupVM
                {
                    GroupName = singleGroup.GroupName,
                    Category = singleGroup.Category,
                    CreatedOn = singleGroup.CreatedOn
                });
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action for deleting group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id, string Message = "Deleted ")
        {
            //TODO: Dont forget to remove all items that depend on a group
            int loggedInUserId = UserHelper.GetCurrentUser().Id;
            var singleGroup = Context.Groups.FirstOrDefault(x => x.Id == id && x.GroupCreator == loggedInUserId);
            if (singleGroup != null && singleGroup.Archived)
            {
                Context.Groups.Remove(singleGroup);
                Context.SaveChanges();
            }

            else
            {
                Message = "Only archived groups can be deleted.";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action for setting groupstatus to Archived
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Archive(int id)
        {            
            int loggedInUserId = UserHelper.GetCurrentUser().Id;
            var singleGroup = Context.Groups.FirstOrDefault(x => x.Id == id && x.GroupCreator == loggedInUserId);
            if (singleGroup != null)
            {
                singleGroup.Archived = true;
                Context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action for setting groupstatus to not Archived
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult UndoArchive(int id)
        {
            int loggedInUserId = UserHelper.GetCurrentUser().Id;
            var singleGroup = Context.Groups.FirstOrDefault(x => x.Id == id && x.GroupCreator == loggedInUserId);
            if (singleGroup != null)
            {
                singleGroup.Archived = false;
                Context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Create a new group
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Archive(GroupVM vm)
        {
            var group = Context.Groups.SingleOrDefault(g => g.Id == vm.Id);

            foreach(var user in group.Users)
            {
                user.Notifications.Add(new Notification
                {
                    Title = $"{group.GroupName} has been deleted.",
                    Content = $"The group \"{group.GroupName}\" you participate in has been deleted.",
                    Date = DateTime.Now,
                });
            }

            group.Archived = true;
            Context.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Create a new group
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult NewGroup(string message = "")
        {
            return View(new GroupVM() { Message = message });
        }

        /// <summary>
        /// Form handling for adding skill to Group
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSkill(Models.Group.EditVM vm)
        {
            var group = Context.Groups.SingleOrDefault(g => g.Id == vm.Id);
            var userId = UserHelper.GetCurrentUser().Id;

            if (!group.Archived)
            {
                Skill NewSkill = new Skill
                {
                    Name = vm.NewSkillName,
                    Description = vm.NewSkillDescription,
                    CreatedBy = Context.Users.SingleOrDefault(g => g.Id == userId),
                };
                group.Skills.Add(NewSkill);
                Context.SaveChanges();
                return View("Edit", new Models.Group.EditVM { Message = "Skill added" });
            }
            else
            {
                return View("Edit", new Models.Group.EditVM { Message = "Group has been archived and cannot be edited" });

            }
        }

        /// <summary>
        /// Submit action for adding user to a group
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUser(AddUserVM vm)
        {
            string input = vm.UserName;
            
            var user = Context.Users.FirstOrDefault(g => g.UserName == vm.UserName || g.Mail == vm.UserName);
            //Return a list with possible users if the username is not found.
            if (user == null)
            {
                if (MailHelpers.CheckIfValidEmail(vm.UserName))
                {
                    string key = Guid.NewGuid().ToString();
                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    string link = baseUrl + "Account/Register?key=" + key;
                    Context.NewRegisters.Add(new NewRegister
                    {
                        Key = key,
                        Email = vm.UserName,
                        GroupId = vm.GroupId
                    });
                    Context.SaveChanges();
                    MailService ms = new MailService();
                    ms.SendRegisterMail(vm.UserName, link);

                    return View(new AddUserVM { GroupId = vm.GroupId, Message = "An email has been sent to the given email." });
                }

                var possibleUsers = Context.Users.Select(g => new Web.Models.Group.UserVM
                {
                    Id = g.Id,
                    UserName = g.UserName,
                    FirstName = g.FirstName,
                    LastName = g.LastName
                }).Where(g => g.UserName.Contains(vm.UserName)).ToList();

                if(possibleUsers != null && possibleUsers.Count > 0)
                    return View(new AddUserVM { GroupId = vm.GroupId, PossibleUsers = possibleUsers });
                else
                    return RedirectToAction("Details", new { id = vm.Id, state = "No user" });
            }
            else
            {
                var group = Context.Groups.FirstOrDefault(g => g.Id == vm.Id);
                if (!group.Archived)
                {
                    if (group.Users == null)
                    {
                        group.Users = new List<User>();
                    }
                    group.Users.Add(user);
                    Context.SaveChanges();
                
                    return View(new AddUserVM { });
                }
                else
                {
                    var message = "Target group is archived";
                    return RedirectToAction("Index", message);
                }            
            }
        }

        /// <summary>
        /// Submit action for creating a new group, takes a Name and Catagory from inputfields
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewGroup(GroupVM vm)
        {
            var message = "";
            if ((!string.IsNullOrWhiteSpace(vm.GroupName) && (!string.IsNullOrWhiteSpace(vm.GroupName))))
            {
                var userId = UserHelper.GetCurrentUser().Id;
                Context.Groups.Add(new Data.Models.Group
                {
                    GroupName = vm.GroupName,
                    Category = vm.Category,
                    GroupCreator = userId,
                    CreatedOn = DateTime.Now
                });

                Context.SaveChanges();
                message = "Group created!";
            }
            else
            {
                message = "Something went wrong, please fill in all the fields";
            }
            return View(new GroupVM() { Message = message });
        }


        public ActionResult GetNewEventModal(int id)
        {
            return PartialView("_createEventPartial", new GroupVM {Id = id});
        }

        public ActionResult AddUserModal(int id)
        {
            return PartialView("_addUserPartial", new AddUserVM { Id = id });
        }

        public ActionResult AddSkillModal(int id)
        {
            return PartialView("_addSkillPartial", new Models.Group.EditVM {Id = id });
        }

        /// <summary>
        /// Standard view for creating a new event
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult NewEvent(int Id, string message = "")
        {
            var singleGroup = Context.Groups.FirstOrDefault(x => x.Id == Id);

            if (!singleGroup.Archived)
            {
                Session["selectedGroupId"] = Id;
                return View(new EventVM() { GroupId = Id, Message = message });
            }
            message = "Group has been archived and cannot be used";
            return RedirectToAction("Index", message);
        }
        /// <summary>
        /// Submit action for creating a new event, takes an eventname from inputfield
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewEvent(GroupVM vm)
        {
            var message = "";
            var state = "";
            if (!string.IsNullOrWhiteSpace(vm.NewEventName))
            {
                var group = Context.Groups.SingleOrDefault(e => e.Id == vm.Id);

                if (group != null && group.Archived)
                {
                    Context.Events.Add(new Event
                    {
                        EventName = vm.NewEventName,
                        EventCreator = UserHelper.GetCurrentDbUser(Context),
                        CreatedOn = DateTime.Now,
                        Group = group
                    });

                    Context.SaveChanges();
                    message = "Event created!";
                    state = "success";
                }
                else
                {
                    message = "No group users found!";
                    state = "error";
                }
            }
            else
            {
                message = "Something went wrong, please fill in all the fields";
            }
            return RedirectToAction("Details", new { id = vm.Id,state});
        }
    }
}
