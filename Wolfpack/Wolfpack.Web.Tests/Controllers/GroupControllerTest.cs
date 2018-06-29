using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Controllers;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Web.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTest
    {
        #region Index

        [TestMethod]
        public void Index_Flow_ViewResult()
        {
            // Arrange
            ///Setup group in mockdb
            var groups = new List<Group>()
            {
                new Group
                {
                    Id = 1,
                    Category = "Unit Category",
                    CreatedOn = DateTime.Now,
                    GroupName = "Unit Group",
                    GroupCreator = 1,
                }
            }.AsQueryable();

            ///create mockuser to use with userhelper
            var user = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var mockContext = new Mock<Context>();

            ///Mock the grouplist into mockdb
            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            ///Mock userhelper to get loggedinUser
            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentDbUser(mockContext.Object)).Returns(user);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            // Act
            ViewResult result = controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region RateUser

        [TestMethod]
        public void RateUser_Flow_ViewResult()
        {
            //Arange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainSkill = new Skill
            {
                Description = "Unit test skill description",
                Id = 1,
                Name = "Unit test skill"
            };

            var skills = new List<Skill>()
            {
                mainSkill
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
                Category = "Unit Category",
                CreatedOn = DateTime.Now,
                GroupName = "Unit Group",
                GroupCreator = mainUser.Id,
                Users = users.ToList(),
                Skills = skills.ToList()
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var mainEvent = new Event
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                EventName = "Unit Event",
                EventCreator = mainUser,
                Skills = skills.ToList(),
                Group = mainGroup,
            };

            var mainTeam = new EventTeam
            {
                Event = mainEvent
            };

            var teams = new List<EventTeam>()
            {
                mainTeam
            }.AsQueryable();

            mainEvent.Teams = teams.ToList();

            var events = new List<Event>()
            {
                mainEvent
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);
            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);
            var mockEvents = MockHelper.MockDbSet(events);
            mockContext.SetupGet(c => c.Events).Returns(mockEvents.Object);
            var mockSkills = MockHelper.MockDbSet(skills);
            mockContext.SetupGet(c => c.Skills).Returns(mockSkills.Object);
            var mockTeams = MockHelper.MockDbSet(teams);
            mockContext.SetupGet(c => c.EventTeams).Returns(mockTeams.Object);

            var controller = new GroupController(mockContext.Object);

            // Act
            ViewResult resultGroup = controller.RateUser(mainGroup.Id) as ViewResult;
            ViewResult resultUser = controller.RateUser(0, mainUser.Id) as ViewResult;
            ViewResult resultTeam = controller.RateUser(0, 0, mainTeam.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(resultGroup);
            Assert.IsNotNull(resultUser);
            Assert.IsNotNull(resultTeam);
        }

        #endregion

        #region SubmitRating

        [TestMethod]
        public void SubmitRating_UserSkillIsNull_RedirectToRouteResult()
        {
            //Arange
            var user = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
            };

            var users = new List<User>()
            {
                user
            }.AsQueryable();

            var skill = new Skill
            {
                Description = "Unit test skill description",
                Id = 1,
                Name = "Unit test skill",
                CreatedBy = user
            };

            var skills = new List<Skill>()
            {
                skill
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(skills);
            mockContext.SetupGet(c => c.Skills).Returns(mockGroups.Object);

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentDbUser(mockContext.Object)).Returns(user);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            var vm = new Models.Group.RateVM
            {
                SkillToRateId = 1,
                UserToRateId = 1,
                Rating = 5,
                RateComment = "Unit Rating Comment"
            };

            // Act
            var result = controller.SubmitRating(vm) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SubmitRating_UserSkillIsNotNull_RedirectToRouteResult()
        {
            //Arange
            var user = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
            };

            var users = new List<User>()
            {
                user
            }.AsQueryable();

            var skill = new Skill
            {
                Description = "Unit test skill description",
                Id = 1,
                Name = "Unit test skill",
                CreatedBy = user
            };

            var skills = new List<Skill>()
            {
                skill
            }.AsQueryable();

            var userskills = new List<UserSkill>()
            {
                new UserSkill
                {
                    Id = 1,
                    User = user,
                    Skill = skill
                }
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(skills);
            mockContext.SetupGet(c => c.Skills).Returns(mockGroups.Object);

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockUserSkills = MockHelper.MockDbSet(userskills);
            mockContext.SetupGet(c => c.UserSkills).Returns(mockUserSkills.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentDbUser(mockContext.Object)).Returns(user);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            var vm = new Models.Group.RateVM
            {
                SkillToRateId = 1,
                UserToRateId = 1,
                Rating = 5,
                RateComment = "Unit Rating Comment"
            };

            // Act
            var result = controller.SubmitRating(vm) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region RemoveUserFromGroup

        [TestMethod]
        public void RemoveUserFromGroup_SingleGroupNotNull_RedirectToRouteResult()
        {
            //Arange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
                Category = "Unit Category",
                CreatedOn = DateTime.Now,
                GroupName = "Unit Group",
                GroupCreator = mainUser.Id,
                Users = users.ToList()
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);
            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);
            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            // Act
            var result = controller.RemoveUserFromGroup(mainUser.Id, mainGroup.Id) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Details

        [TestMethod]
        public void Details_SingleGroupNotNull_ViewResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainSkill = new Skill
            {
                Description = "Unit test skill description",
                Id = 1,
                Name = "Unit test skill",
                CreatedBy = mainUser
            };

            var skills = new List<Skill>()
            {
                mainSkill
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
                Category = "Unit Category",
                CreatedOn = DateTime.Now,
                GroupName = "Unit Group",
                GroupCreator = mainUser.Id,
                Users = users.ToList(),
                Skills = skills.ToList(),
                Archived = false
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();
            
            

            var mockContext = new Mock<Context>();
            
            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockSkills = MockHelper.MockDbSet(skills);
            mockContext.SetupGet(c => c.Skills).Returns(mockSkills.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            // Act
            ViewResult result = controller.Details(mainGroup.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Details_SingleGroupNull_RedirectToRouteResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            // Act
            RedirectToRouteResult result = controller.Details(0) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Delete
        [TestMethod]
        public void Delete_SingleGroupNotNull_ViewResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
                Category = "Unit Category",
                CreatedOn = DateTime.Now,
                GroupName = "Unit Group",
                GroupCreator = mainUser.Id,
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();


            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            // Act
            ViewResult result = controller.Delete(mainGroup.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_SingleGroupNull_RedirectToRouteResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            // Act
            RedirectToRouteResult result = controller.Delete(0) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region DeletePost

        [TestMethod]
        public void DeletePost_FLow_RedirectToRouteResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            // Act
            RedirectToRouteResult resultGroupNotNull = controller.Delete(mainGroup.Id, "Deleted") as RedirectToRouteResult;
            RedirectToRouteResult resultGroupNull = controller.Delete(0, "Deleted") as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultGroupNotNull);
            Assert.IsNotNull(resultGroupNull);
        }

        #endregion

        #region Archive

        [TestMethod]
        public void Archive_Flow_RedirectToRouteResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            // Act
            RedirectToRouteResult resultGroupNotNull = controller.Archive(mainGroup.Id) as RedirectToRouteResult;
            RedirectToRouteResult resultGroupNull = controller.Archive(0) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultGroupNotNull);
            Assert.IsNotNull(resultGroupNull);
        }

        #endregion

        #region UndoArchive

        [TestMethod]
        public void UndoArchive_Flow_RedirectToRouteResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
                Archived = true
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            // Act
            RedirectToRouteResult resultGroupNotNull = controller.UndoArchive(mainGroup.Id) as RedirectToRouteResult;
            RedirectToRouteResult resultGroupNull = controller.UndoArchive(0) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultGroupNotNull);
            Assert.IsNotNull(resultGroupNull);
        }

        #endregion

        #region ArchivePost

        [TestMethod]
        public void ArchivePost_Flow_RedirectToRouteResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
                GroupName = "Unit Group",
                Users = users.ToList(),
                Archived = false
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            var vm = new Models.Group.GroupVM
            {
                Id = mainGroup.Id
            };

            // Act
            RedirectToRouteResult result = controller.Archive(vm) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region NewGroup

        [TestMethod]
        public void NewGroup_Flow_ViewResult()
        {
            //Arange
            var mockContext = new Mock<Context>();

            var controller = new GroupController(mockContext.Object);

            var vm = new Models.Group.GroupVM
            {
                Message = "Unit Message"
            };

            // Act
            ViewResult result = controller.NewGroup(vm) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region AddSkillPost

        [TestMethod]
        public void AddSkillPost_Flow_ViewResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
                Archived = true
            };

            var secondGroup = new Group {
                Id = 2,
                Archived = false
            };

            var groups = new List<Group>()
            {
                mainGroup,
                secondGroup
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            var vmArchived = new Models.Group.EditVM
            {
                Id = mainGroup.Id
            };

            var vmNotArchived = new Models.Group.EditVM
            {
                Id = secondGroup.Id,
                NewSkillName = "Unit Skill",
                NewSkillDescription = "Unit Skill Description"
            };

            // Act
            ViewResult resultGroupArchived = controller.AddSkill(vmArchived) as ViewResult;
            ViewResult resultGroupNotArchived = controller.AddSkill(vmNotArchived) as ViewResult;

            // Assert
            Assert.IsNotNull(resultGroupArchived);
            Assert.IsNotNull(resultGroupNotArchived);
        }

        #endregion

        #region AddUserPost

        [TestMethod]
        public void AddUserPost_UserNull_ActionResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com",
                Notifications = new List<Notification>()
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
            };

            var groups = new List<Group>()
            {
                mainGroup,
            }.AsQueryable();

            var notifications = new List<Notification>().AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockNotifications = MockHelper.MockDbSet(notifications);
            mockContext.SetupGet(c => c.Notifications).Returns(mockNotifications.Object);

            var controller = new GroupController(mockContext.Object);

            var vmUserNullMail = new Models.Group.AddUserVM
            {
                Id = mainGroup.Id,
                UserName = "wolfpack.nhlstenden@gmail.com",
                GroupId = mainGroup.Id
            };

            var vmUserNullUsernameHalf = new Models.Group.AddUserVM
            {
                Id = mainGroup.Id,
                UserName = "Unit",
                GroupId = mainGroup.Id
            };

            var vmUserNullUsernameNone = new Models.Group.AddUserVM
            {
                Id = mainGroup.Id,
                UserName = "Nothing",
                GroupId = mainGroup.Id
            };

            // Act
            //ViewResult resultValidMail = controller.AddUser(vmUserNullMail) as ViewResult; TODO: Moq Request

            ViewResult resultUsernameWithPossibleUsers = controller.AddUser(vmUserNullUsernameHalf) as ViewResult;
            RedirectToRouteResult resultUsernameNoPossibleUsers = controller.AddUser(vmUserNullUsernameNone) as RedirectToRouteResult;

            // Assert
            //Assert.IsNotNull(resultValidMail); TODO: Moq Request

            Assert.IsNotNull(resultUsernameWithPossibleUsers);
            Assert.IsNotNull(resultUsernameNoPossibleUsers);
        }

        [TestMethod]
        public void AddUserPost_UserNotNull_ActionResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
                Archived = true
            };

            var secondGroup = new Group
            {
                Id = 2,
                Archived = false
            };

            var groups = new List<Group>()
            {
                mainGroup,
                secondGroup
            }.AsQueryable();

            var newRegisters = new List<NewRegister>().AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockNewRegisters = MockHelper.MockDbSet(newRegisters);
            mockContext.SetupGet(c => c.NewRegisters).Returns(mockNewRegisters.Object);

            var controller = new GroupController(mockContext.Object);

            var vmGroupArchived = new Models.Group.AddUserVM
            {
                Id = mainGroup.Id,
                UserName = mainUser.UserName,
                GroupId = mainGroup.Id
            };

            var vmGroupNotArchived = new Models.Group.AddUserVM
            {
                Id = secondGroup.Id,
                UserName = mainUser.UserName,
                GroupId = secondGroup.Id
            };

            // Act
            RedirectToRouteResult resultGroupArchived = controller.AddUser(vmGroupArchived) as RedirectToRouteResult;
            RedirectToRouteResult resultGroupNotArchived = controller.AddUser(vmGroupNotArchived) as RedirectToRouteResult; //fails is RedirexctToRoute?

            // Assert
            Assert.IsNotNull(resultGroupArchived);
            Assert.IsNotNull(resultGroupNotArchived);
        }

        #endregion

        #region NewGroupPost

        [TestMethod]
        public void NewGroupPost_IsNullOrWhiteSpace_ViewResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var mainGroup = new Group
            {
                Id = 1,
                Archived = true
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            var vmGroupNameNotNullOrWhiteSpace = new Models.Group.GroupVM
            {
                Id = mainGroup.Id,
                GroupName = "Unit Group",
                Category = "Unit Category"
            };

            var vmGroupNameNull = new Models.Group.GroupVM
            {
                Id = mainGroup.Id,
                GroupName = null
            };

            var vmGroupNameWhiteSpace = new Models.Group.GroupVM
            {
                Id = mainGroup.Id,
                GroupName = ""
            };

            // Act
            ViewResult resultGroupNotNullOrWhiteSpace = controller.NewGroup(vmGroupNameNotNullOrWhiteSpace) as ViewResult;

            ViewResult resultGroupNull = controller.NewGroup(vmGroupNameNull) as ViewResult;
            ViewResult resultGroupWhiteSpace = controller.NewGroup(vmGroupNameWhiteSpace) as ViewResult;

            // Assert
            Assert.IsNotNull(resultGroupNotNullOrWhiteSpace);

            Assert.IsNotNull(resultGroupNull);
            Assert.IsNotNull(resultGroupWhiteSpace);
        }

        #endregion

        #region Modals

        [TestMethod]
        public void GetNewEventModal_Flow_PartialViewResult()
        {
            //Arange
            var mockContext = new Mock<Context>();
            var controller = new GroupController(mockContext.Object);

            // Act
            PartialViewResult result = controller.GetNewEventModal(1) as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RatingProgressModal_Flow_PartialViewResult()
        {
            //Arange
            var mockContext = new Mock<Context>();
            var controller = new GroupController(mockContext.Object);

            // Act
            PartialViewResult result = controller.GetNewEventModal(1) as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddUserModal_Flow_PartialViewResult()
        {
            //Arange
            var mockContext = new Mock<Context>();
            var controller = new GroupController(mockContext.Object);

            // Act
            PartialViewResult result = controller.GetNewEventModal(1) as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddSkillModal_Flow_PartialViewResult()
        {
            //Arange
            var mockContext = new Mock<Context>();
            var controller = new GroupController(mockContext.Object);

            // Act
            PartialViewResult result = controller.GetNewEventModal(1) as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region NewEventPost

        [TestMethod]
        public void NewEventPost_IsNullOrWhiteSpace_RedirectToRouteResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com"
            };

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainGroup = new Group
            {
                Id = 1,
                Archived = true,
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var events = new List<Event>().AsQueryable();

            var mockContext = new Mock<Context>();

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockEvents = MockHelper.MockDbSet(events);
            mockContext.SetupGet(c => c.Events).Returns(mockEvents.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentDbUser(mockContext.Object)).Returns(mainUser);

            var controller = new GroupController(mockContext.Object, mockUserHelper.Object);

            var vmEventNameNotNullOrWhiteSpaceGroupNotNull = new Models.Group.GroupVM
            {
                Id = mainGroup.Id,
                NewEventName = "Unit Event",
                Archived = false
            };

            var vmEventNameNotNullOrWhiteSpaceGroupNull = new Models.Group.GroupVM
            {
                Id = 0,
                NewEventName = "Unit Event",
                Archived = false
            };

            var vmEventNameNotNullOrWhiteSpaceGroupNotNullArchived = new Models.Group.GroupVM
            {
                Id = mainGroup.Id,
                NewEventName = "Unit Event",
                Archived = true
            };

            var vmEventNameNull = new Models.Group.GroupVM
            {
                Id = mainGroup.Id,
                NewEventName = null
            };

            var vmEventNameWhiteSpace = new Models.Group.GroupVM
            {
                Id = mainGroup.Id,
                NewEventName = ""
            };

            // Act
            RedirectToRouteResult resultEventNameNotNullOrWhiteSpaceGroupNotNull = controller.NewEvent(vmEventNameNotNullOrWhiteSpaceGroupNotNull) as RedirectToRouteResult;
            RedirectToRouteResult resultEventNameNotNullOrWhiteSpaceGroupNull = controller.NewEvent(vmEventNameNotNullOrWhiteSpaceGroupNotNullArchived) as RedirectToRouteResult;
            RedirectToRouteResult resultEventNameNotNullOrWhiteSpaceGroupNotNullArchived = controller.NewEvent(vmEventNameNotNullOrWhiteSpaceGroupNull) as RedirectToRouteResult;

            RedirectToRouteResult resultEventNameNull = controller.NewEvent(vmEventNameNull) as RedirectToRouteResult;
            RedirectToRouteResult resultEventNameWhiteSpace = controller.NewEvent(vmEventNameNull) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultEventNameNotNullOrWhiteSpaceGroupNotNull);
            Assert.IsNotNull(resultEventNameNotNullOrWhiteSpaceGroupNull);
            Assert.IsNotNull(resultEventNameNotNullOrWhiteSpaceGroupNotNullArchived);

            Assert.IsNotNull(resultEventNameNull);
            Assert.IsNotNull(resultEventNameWhiteSpace);
        }

        #endregion
    }
}