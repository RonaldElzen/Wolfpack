using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Controllers;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Helpers.Interfaces;
using Wolfpack.Web.Models.Profile;

namespace Wolfpack.Web.Tests.Controllers
{
    [TestClass]
    public class ProfileControllerTest
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

            var users = new List<User>()
            {
                mainUser
            }.AsQueryable();

            var mockUser = new UserHelper { Id = 1 };

            var mockContext = new Mock<Context>();

            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentDbUser(mockContext.Object)).Returns(mainUser);
            mockUserHelper.Setup(x => x.GetCurrentUser()).Returns(mockUser);

            var controller = new ProfileController(mockContext.Object, mockUserHelper.Object);

            // Act
            ViewResult result = controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region SearchProfilePost

        [TestMethod]
        public void SearchProfilePost_UserNull_ActionResult()
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

            var mockContext = new Mock<Context>();
            
            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);
            
            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentDbUser(mockContext.Object)).Returns(mainUser);

            var controller = new ProfileController(mockContext.Object, mockUserHelper.Object);

            var vmUserNull = new SearchVM()
            {
                UserName = "nothing"
            };

            var vmUserNotNull = new SearchVM()
            {
                UserName = "UnitTest"
            };

            // Act
            ViewResult resultUserNull = controller.SearchProfile(vmUserNull) as ViewResult;
            RedirectToRouteResult resultUserNotNull = controller.SearchProfile(vmUserNotNull) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultUserNull);
            Assert.IsNotNull(resultUserNotNull);
        }

        #endregion

        #region ViewSkill

        [TestMethod]
        public void ViewSkill_Flow_ViewResult()
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
                CreatedBy = mainUser,
                Description = "Unit Description",
                Id = 1,
                Name = "Unit Skill"
            };

            var skills = new List<Skill>()
            {
                mainSkill
            }.AsQueryable();

            var mainUserSkill = new UserSkill {
                Id = 1,
                Skill = mainSkill,
                User = mainUser
            };

            var ratings = new List<Rating>()
            {
                new Rating{Mark = 5, RatedAt = DateTime.Now, Comment = "Unit Comment", UserSkill = mainUserSkill},
                new Rating{Mark = 6, RatedAt = DateTime.Now, Comment = "Unit Comment2", UserSkill = mainUserSkill}
            }.AsQueryable();

            mainUserSkill.Ratings = ratings.ToList();

            var userSkills = new List<UserSkill>()
            {
                mainUserSkill
            }.AsQueryable();

            var mockContext = new Mock<Context>();
            
            var mockUsers = MockHelper.MockDbSet(users);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);

            var mockSkills = MockHelper.MockDbSet(skills);
            mockContext.SetupGet(c => c.Skills).Returns(mockSkills.Object);

            var mockRatings = MockHelper.MockDbSet(ratings);
            mockContext.SetupGet(c => c.Ratings).Returns(mockRatings.Object);
            
            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentDbUser(mockContext.Object)).Returns(mainUser);

            var controller = new ProfileController(mockContext.Object, mockUserHelper.Object);

            // Act
            ViewResult resultUserNull = controller.ViewSkill(mainSkill.Id, mainUser.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(resultUserNull);
        }

        #endregion
    }
}
