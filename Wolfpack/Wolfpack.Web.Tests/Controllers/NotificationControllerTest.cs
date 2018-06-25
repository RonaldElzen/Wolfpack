using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Wolfpack.Data.Models;
using Wolfpack.Data;
using Wolfpack.Web.Controllers;
using System.Web.Mvc;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Web.Tests.Controllers
{
    [TestClass]
    public class NotificationControllerTest
    {
        [TestMethod]
        public void GetNotification_NotificationIsNull_HttpNotFoundResult()
        {
            // Arrange
            var data = new List<Notification>
            {
                new Notification { Id = 1 }

            }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(data);

            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Notifications).Returns(mockSet.Object);

            var controller = new NotificationController(mockContext.Object);

            // Act

            var result = controller.GetNotification(3) as HttpNotFoundResult;

            // Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNotification_NotificationIsNotNull_ViewResult()
        {
            // Arrange
            var data = new List<Notification>
            {
                new Notification { Id = 1 }

            }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(data);

            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Notifications).Returns(mockSet.Object);

            var controller = new NotificationController(mockContext.Object);

            // Act

            var result = controller.GetNotification(1) as ViewResult;

            // Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNotifications_Flow_ViewResult()
        {
            // Arrange
            var data = new List<Notification>
            {
                new Notification { Id = 1 }

            }.AsQueryable();

            var user = new User
            {
                Id = 1,
                Notifications = new List<Notification>()
                {
                    new Notification
                    {
                        Date = DateTime.Now.AddDays(-3),
                        IsRead = false,
                        Content = "",
                        Title = ""
                    },
                    new Notification
                    {
                        Date = DateTime.Now.AddDays(-1),
                        IsRead = true,
                        Content = "",
                        Title = ""
                    },
                    new Notification
                    {
                        Date = DateTime.Now.AddDays(-1),
                        IsRead = false,
                        Content = "",
                        Title = ""
                    },
                    new Notification
                    {
                        Date = DateTime.Now.AddDays(-3),
                        IsRead = true,
                        Content = "",
                        Title = ""
                    },
                }
            };

            var mockSet = MockHelper.MockDbSet(data);

            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Notifications).Returns(mockSet.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(u => u.GetCurrentDbUser(mockContext.Object)).Returns(user);

            var controller = new NotificationController(mockContext.Object, mockUserHelper.Object);

            // Act

            var result = controller.GetNotifications() as ViewResult;

            // Assert

            Assert.IsNotNull(result);
        }
    }
}
