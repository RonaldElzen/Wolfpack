using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wolfpack.Data;
using Wolfpack.Web.Controllers;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void Register_Flow_ViewResult()
        {
            // Arrange
            var mockContext = new Mock<Context>();
            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            // Act
            var result = controller.Register() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Login_CurrentUserIsNull_ViewResult()
        {
            // Arrange
            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser()).Returns<IUserHelper>(null);

            var mockContext = new Mock<Context>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            // Act
            var result = controller.Login() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Login_CurrentUserNotNull_RedirectToRouteResult()
        {
            // Arrange
            var userHelper = new UserHelper { Id = 1, UserName = "UnitTest" };

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser()).Returns(userHelper);

            var mockContext = new Mock<Context>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            // Act
            var result = controller.Login() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
