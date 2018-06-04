﻿using BusinessLayer.Services;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Standard view for creating a new group
        /// </summary>
        /// <returns></returns>
        public GroupController(Context context) : base(context) { }

        /// <summary>
        /// View all groups
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = UserHelper.GetCurrentDbUser(Context);
            //Get the groups created by user
            var createdGroups = Context.Groups.Where(x => x.GroupCreator == user.Id).Select(g => new GroupVM
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
            });

            return View(new UserGroupsVM{CreatedGroups = createdGroups, ParticipatingGroups = participatingGroups });
        }

        /// <summary>
        /// View for edit
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id, string message = "")
        {
            var users = Context.Groups.SingleOrDefault(x => x.Id == id).Users.Select(u => new EditVMUser {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName
            });

            return View(new Models.Group.EditVM
            {
                Id = id,
                GroupUsers = users,
                Message = message
            });
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
            if(singleGroup != null)
            {
                var user = Context.Users.FirstOrDefault(x => x.Id == userId);
                singleGroup.Users.Remove(user);
                Context.SaveChanges();

                //TODO Send notification to removed user (waiting for notification system)
            }
            return RedirectToAction("Edit", new { Id = groupId });
        }

        /// <summary>
        /// View single group
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            int loggedInUserId = UserHelper.GetCurrentUser().Id;
            var singleGroup = Context.Groups.SingleOrDefault(x => x.Id == id && x.GroupCreator == loggedInUserId);
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
                    GroupUsers = groupUsers
                });
            }
            return RedirectToAction("Index", "GroupController");
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

            Skill NewSkill = new Skill
            {
                Name = vm.NewSkillName,
                Description = vm.NewSkillDescription,
                CreatedBy = Context.Users.SingleOrDefault(g => g.Id == userId),
            };
            group.Skills.Add(NewSkill);

            Context.SaveChanges();
            var users = Context.Groups.SingleOrDefault(x => x.Id == vm.Id).Users.Select(u => new EditVMUser
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName
            });
            
            return View("Edit" , new Models.Group.EditVM {
                Id = group.Id,
                GroupUsers = users,
                Message = "Skill added"
            });
        }

        /// <summary>
        /// Submit action for adding user to a group
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUser(AddUserVM vm)
        {
            var user = Context.Users.FirstOrDefault(g => g.UserName == vm.UserName);
            //Return a list with possible users if the username is not found.
            if (user == null)
            {
                var possibleUsers = Context.Users.Select(g => new Web.Models.Group.UserVM
                {
                    Id = g.Id,
                    UserName = g.UserName,
                    FirstName = g.FirstName,
                    LastName = g.LastName
                }).Where(g => g.UserName.Contains(vm.UserName)).ToList();

                if(possibleUsers != null && possibleUsers.Count > 0)
                    return View(new AddUserVM { PossibleUsers = possibleUsers });
                else
                    return View(new AddUserVM { Message = "No user found" });

            }
            else
            {
                Group group = Context.Groups.FirstOrDefault(g => g.Id == 1);
                if (group.Users == null)
                {
                    group.Users = new List<User>();
                }
                group.Users.Add(user);
                Context.SaveChanges();
                return View("Edit", new Models.Group.EditVM { Message = "User added" });
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
                Context.Groups.Add(new Group
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

        /// <summary>
        /// Standard view for creating a new event
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult NewEvent(int Id, string message = "")
        {
            Session["selectedGroupId"] = Id;
            return View(new EventVM() { GroupId = Id, Message = message });
        }

        /// <summary>
        /// Submit action for creating a new event, takes an eventname from inputfield
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewEvent(EventVM vm)
        {
            var message = "";
            if (!string.IsNullOrWhiteSpace(vm.EventName))
            {
                var group = Context.Groups.SingleOrDefault(e => e.Id == vm.GroupId);
                if (group != null)
                {
                    Context.Events.Add(new Event
                    {
                        EventName = vm.EventName,
                        EventCreator = UserHelper.GetCurrentDbUser(Context),
                        CreatedOn = DateTime.Now,
                        Group = group
                    });

                    Context.SaveChanges();
                    message = "Event created!";
                }
                else
                {
                    message = "No group users found!";
                }
            }
            else
            {
                message = "Something went wrong, please fill in all the fields";
            }
            return View(new EventVM() { Message = message });
        }
    }
}
