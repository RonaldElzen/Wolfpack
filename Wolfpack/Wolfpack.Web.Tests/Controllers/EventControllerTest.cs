using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Wolfpack.Data.Models;
using Wolfpack.Data;
using Wolfpack.Web.Helpers.Interfaces;
using System.Web.Mvc;
using Wolfpack.Web.Controllers;
using Wolfpack.Web.Models.Event;

namespace Wolfpack.Web.Tests.Controllers
{
    [TestClass]
    public class EventControllerTest
    {
        #region Index

        [TestMethod]
        public void Index_Flow_ViewResult()
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
            
            var events = new List<Event>()
            {
                new Event
                {
                    Id = 1,
                    CreatedOn = DateTime.Now,
                    EventCreator = mainUser,
                    EventName = "Unit Event",
                }
            }.AsQueryable();

            var mockContext = new Mock<Context>();
            
            var mockEvents = MockHelper.MockDbSet(events);
            mockContext.SetupGet(c => c.Events).Returns(mockEvents.Object);
            
            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentDbUser(mockContext.Object)).Returns(mainUser);

            var controller = new EventController(mockContext.Object, mockUserHelper.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Details

        [TestMethod]
        public void Details_EventNull_ActionResult()
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
                Users = users.ToList()
            };

            var groups = new List<Group>()
            {
                mainGroup
            }.AsQueryable();

            var skills = new List<Skill>()
            {
                new Skill {CreatedBy = mainUser, Id = 1 }
            }.AsQueryable();

            var mainEvent = new Event
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                EventCreator = mainUser,
                EventName = "Unit Event",
                Group = mainGroup,
                Skills = skills.ToList(),
            };

            var teams = new List<EventTeam>()
            {
                new EventTeam{Event = mainEvent, Id = 1, Users = users.ToList()}
            }.AsQueryable();

            mainEvent.Teams = teams.ToList();

            var events = new List<Event>()
            {
                mainEvent
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockEvents = MockHelper.MockDbSet(events);
            mockContext.SetupGet(c => c.Events).Returns(mockEvents.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new EventController(mockContext.Object, mockUserHelper.Object);

            // Act
            ViewResult resultEventNotNull = controller.Details(mainEvent.Id) as ViewResult;
            RedirectToRouteResult resultEventNull = controller.Details(0) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultEventNotNull);
            Assert.IsNotNull(resultEventNull);
        }

        #endregion

        #region Edit

        [TestMethod]
        public void Edit_Flow_ViewResult()
        {
            var mockContext = new Mock<Context>();

            var controller = new EventController(mockContext.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Delete

        [TestMethod]
        public void Delete_EventNull_ActionResult()
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

            var mainEvent = new Event
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                EventCreator = mainUser,
                EventName = "Unit Event",
            };

            var events = new List<Event>()
            {
                mainEvent
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockEvents = MockHelper.MockDbSet(events);
            mockContext.SetupGet(c => c.Events).Returns(mockEvents.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new EventController(mockContext.Object, mockUserHelper.Object);

            // Act
            ViewResult resultEventNotNull = controller.Delete(mainEvent.Id) as ViewResult;
            RedirectToRouteResult resultEventNull = controller.Delete(0) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultEventNotNull);
            Assert.IsNotNull(resultEventNull);
        }

        #endregion

        #region Team

        [TestMethod]
        public void Team_Flow_ViewResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com",
            };

            var ratings = new List<Rating>()
            {
                new Rating{Mark = 6.0, Id = 1}
            }.AsQueryable();

            var mainSkill = new Skill
            {
                Id = 1,
                Name = "Unit Skill"
            };

            var userSkills = new List<UserSkill>()
            {
                new UserSkill{Id = 1, Ratings = ratings.ToList(), User = mainUser, Skill = mainSkill}
            }.AsQueryable();

            mainUser.UserSkills = userSkills.ToList();

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainTeam = new EventTeam
            {
                Id = 1,
                Users = users.ToList()
            };

            var teams = new List<EventTeam>()
            {
                mainTeam
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockTeams = MockHelper.MockDbSet(teams);
            mockContext.SetupGet(c => c.EventTeams).Returns(mockTeams.Object);

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockUserSkills = MockHelper.MockDbSet(userSkills);
            mockContext.SetupGet(c => c.UserSkills).Returns(mockUserSkills.Object);

            var mockRatings = MockHelper.MockDbSet(ratings);
            mockContext.SetupGet(c => c.Ratings).Returns(mockRatings.Object);

            var controller = new EventController(mockContext.Object);

            // Act
            ViewResult result = controller.Team(mainTeam.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region GenerateTeams

        [TestMethod]
        public void GenerateTeams_Flow_ViewResult()
        {
            // Arrange
            var mockContext = new Mock<Context>();

            var controller = new EventController(mockContext.Object);

            // Act
            ViewResult result = controller.GenerateTeams(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region GenerateTeamsPost

        [TestMethod]
        public void GenerateTeamsPost_Flow_ActionResult()
        {
            // Arrange
            var mainUser = new User
            {
                Id = 1,
                FirstName = "Unit",
                LastName = "Test",
                UserName = "UnitTest",
                Mail = "unittest@wolfpack.com",
                Notifications = new List<Notification>(),
            };

            var secondUser = new User
            {
                Id = 2,
                FirstName = "Unit2",
                LastName = "Test2",
                UserName = "UnitTest2",
                Mail = "unittest2@wolfpack.com",
                Notifications = new List<Notification>()
            };

            var ratings = new List<Rating>()
            {
                new Rating{Mark = 6.0, Id = 1}
            }.AsQueryable();

            var mainSkill = new Skill
            {
                Id = 1,
                Name = "Unit Skill"
            };

            var userSkills1 = new List<UserSkill>()
            {
                new UserSkill{Id = 1, Ratings = ratings.ToList(), User = mainUser, Skill = mainSkill},
            }.AsQueryable();

            var userSkills2 = new List<UserSkill>()
            {
                new UserSkill { Id = 2, Ratings = ratings.ToList(), User = secondUser, Skill = mainSkill }
            }.AsQueryable();

            mainUser.UserSkills = userSkills1.ToList();
            secondUser.UserSkills = userSkills2.ToList();

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mainTeam = new EventTeam
            {
                Id = 1,
                Users = users.ToList()
            };

            var teams = new List<EventTeam>()
            {
                mainTeam
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

            var mainEvent = new Event
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                EventCreator = mainUser,
                EventName = "Unit Event",
                Group = mainGroup,
                Teams = teams.ToList()
            };

            var events = new List<Event>()
            {
                mainEvent
            }.AsQueryable();

            var notifications = new List<Notification>().AsQueryable();

            var mockContext = new Mock<Context>();

            var mockTeams = MockHelper.MockDbSet(teams);
            mockContext.SetupGet(c => c.EventTeams).Returns(mockTeams.Object);

            var mockEvents = MockHelper.MockDbSet(events);
            mockContext.SetupGet(c => c.Events).Returns(mockEvents.Object);

            var mockGroups = MockHelper.MockDbSet(groups);
            mockContext.SetupGet(c => c.Groups).Returns(mockGroups.Object);

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockUserSkills1 = MockHelper.MockDbSet(userSkills1);
            mockContext.SetupGet(c => c.UserSkills).Returns(mockUserSkills1.Object);

            var mockUserSkills2 = MockHelper.MockDbSet(userSkills2);
            mockContext.SetupGet(c => c.UserSkills).Returns(mockUserSkills2.Object);

            var mockRatings = MockHelper.MockDbSet(ratings);
            mockContext.SetupGet(c => c.Ratings).Returns(mockRatings.Object);

            var controller = new EventController(mockContext.Object);

            var vmTeamSizeBelowOne = new GenerateTeamsVM
            {
                TeamSize = 0,
                EventId = mainEvent.Id
            };
            var vmAlgorithmTypeAverageTeams = new GenerateTeamsVM
            {
                TeamSize = 4,
                EventId = mainEvent.Id,
                AlgorithmType = Helpers.Enums.AlgorithmType.AverageTeams
            };
            var vmAlgorithmTypeBestTeam = new GenerateTeamsVM
            {
                TeamSize = 4,
                EventId = mainEvent.Id,
                AlgorithmType = Helpers.Enums.AlgorithmType.BestTeam
            };

            // Act
            ViewResult resultTeamSizeBelowOne = controller.GenerateTeams(vmTeamSizeBelowOne) as ViewResult;
            //RedirectToRouteResult resultAlgorithmTypeAverageTeams = controller.GenerateTeams(vmAlgorithmTypeAverageTeams) as RedirectToRouteResult;
            //RedirectToRouteResult resultAlgorithmTypeBestTeam = controller.GenerateTeams(vmAlgorithmTypeBestTeam) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultTeamSizeBelowOne);
            //Assert.IsNotNull(resultAlgorithmTypeAverageTeams); TODO: Remove or fix unit test
            //Assert.IsNotNull(resultAlgorithmTypeBestTeam); TODO: Remove or fix unit test
        }

        #endregion

        #region AddSkill

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

            var mainEvent = new Event
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                EventCreator = mainUser,
                EventName = "Unit Event",
            };

            var events = new List<Event>()
            {
                mainEvent
            }.AsQueryable();

            var mainSkill = new Skill
            {
                CreatedBy = mainUser,
                Id = 1,
                Name = "Unit Skill"
            };

            var skills = new List<Skill>()
            {
                mainSkill
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            var mockEvents = MockHelper.MockDbSet(events);
            mockContext.SetupGet(c => c.Events).Returns(mockEvents.Object);

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockSkills = MockHelper.MockDbSet(skills);
            mockContext.SetupGet(c => c.Skills).Returns(mockSkills.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser().Id).Returns(mainUser.Id);

            var controller = new EventController(mockContext.Object, mockUserHelper.Object);

            var vm = new EditVM
            {
                Id = mainEvent.Id,
                NewSkillDescription = "Unit Skill Description",
                NewSkillName = "Unit Skill Name"
            };

            // Act
            ViewResult result = controller.AddSkill(vm) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion
    }
}
