using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wolfpack.BusinessLayer;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Controllers;
using Wolfpack.Web.Helpers;
using Wolfpack.Web.Helpers.Enums;
using Wolfpack.Web.Helpers.Interfaces;
using Wolfpack.Web.Models.Account;

namespace Wolfpack.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        #region Register

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

        #endregion

        #region Login

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

        #endregion

        #region LoginPost

        [TestMethod]
        public void LoginPost_UserIsNull_ViewResult()
        {
            // Arrange
            var data = new List<User>
            {
                new User { Id = 1, UserName = "Test1" },
                new User { Id = 2, UserName = "Test2" }

            }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(data);

            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Users).Returns(mockSet.Object);

            var mockUserHelper = new Mock<IUserHelper>();

            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            var vm = new LoginVM { LoginName = "Test5", Password = "Test" };

            // Act

            var result = controller.Login(vm) as ViewResult;

            // Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoginPost_InCorrectPassword_ViewResult()
        {
            // Arrange
            var data = new List<User>
            {
                new User { Id = 1, UserName = "Test1", Password = Hashing.Hash("Test2") }

            }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(data);

            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Users).Returns(mockSet.Object);

            var mockUserHelper = new Mock<IUserHelper>();

            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            var vm = new LoginVM { LoginName = "Test1", Password = "Test" };

            // Act

            var result = controller.Login(vm) as ViewResult;

            // Assert

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoginPost_CorrectPassword_RedirectToRouteResult()
        {
            // Arrange
            var data = new List<User>
            {
                new User { Id = 1, UserName = "Test1", Password = Hashing.Hash("Test") }

            }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(data);

            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Users).Returns(mockSet.Object);

            var mockUserHelper = new Mock<IUserHelper>();

            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            var vm = new LoginVM { LoginName = "Test1", Password = "Test" };

            // Act

            var result = controller.Login(vm) as RedirectToRouteResult;

            // Assert

            Assert.IsNotNull(result);
        }

        #endregion

        #region Logout

        [TestMethod]
        public void Logout_Flow_RedirectToRouteResult()
        {
            // Arrange
            var userHelper = new UserHelper { Id = 1, UserName = "UnitTest" };

            var mockUserHelper = new Mock<IUserHelper>();
            mockUserHelper.Setup(x => x.GetCurrentUser()).Returns(userHelper);

            var mockContext = new Mock<Context>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            // Act
            var result = controller.Logout() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region RegisterPost
        //TODO RegisterPost Test
        [TestMethod, Ignore]
        public void RegisterPost_InValidEmail_ViewResult()
        {
            // Arrange
            var mockContext = new Mock<Context>();
            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            var vm = new RegisterVM { MailAdress = "test" };

            // Act

            var result = controller.RegisterPost(vm) as ViewResult;

            // Assert

            Assert.IsNotNull(result);
        }

        #endregion

        #region Recovery

        [TestMethod]
        public void Recovery_KeyIsNull_ViewResult()
        {
            // Arrange
            var mockContext = new Mock<Context>();
            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            var vm = new RecoveryVM { Key = null };

            // Act
            var result = controller.Recovery(vm) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Recovery_RecoveryIsNull_ViewResult()
        {
            // Arrange
            var data = new List<Recovery>()
            {
                new Recovery
                {
                    Id = 1,
                    Key = "TestKey",
                    User = null
                }
            }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(data);

            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Recoveries).Returns(mockSet.Object);

            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object);

            var vm = new RecoveryVM { Key = "TestKey2" };

            // Act
            var result = controller.Recovery(vm) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Recovery_SuccessFullRecovery_ViewResult()
        {
            // Arrange
            var data = new List<Recovery>()
            {
                new Recovery
                {
                    Id = 1,
                    Key = "TestKey",
                    User = new User
                    {
                        Id = 1,
                        UserName = "Test"
                    }
                }
            }.AsQueryable();

            var vm = new RecoveryVM { Key = "TestKey" };

            var mockSet = MockHelper.MockDbSet(data);
            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Recoveries).Returns(mockSet.Object);

            var mockSession = new Mock<ISessionHelper>();
            mockSession.Setup(s => s.SetSessionItem("recoveryKey", vm.Key));

            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object, mockSession.Object);

            // Act
            var result = controller.Recovery(vm) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region RecoveryForm

        [TestMethod]
        public void RecoveryForm_NotSamePassword_ViewResultWithFailedModel()
        {
            // Arrange
            var vm = new RecoveryVM { Password = "Test", PasswordCheck = "Test2" };
            
            var mockSession = new Mock<ISessionHelper>();
            mockSession.Setup(s => s.GetSessionItem("recoveryKey")).Returns("TestKey");

            var mockContext = new Mock<Context>();
            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object, mockSession.Object);

            // Act
            var result = controller.RecoveryForm(vm) as ViewResult;
            var resultModel = result.Model as RecoveryVM;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(resultModel.Status, RecoveryStatus.Failed);
        }

        [TestMethod]
        public void RecoveryForm_KeyIsNull_ViewResult()
        {
            // Arrange
            var vm = new RecoveryVM { Password = "Test", PasswordCheck = "Test" };

            var mockSession = new Mock<ISessionHelper>();
            mockSession.Setup(s => s.GetSessionItem("recoveryKey")).Returns(null);

            var mockContext = new Mock<Context>();
            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object, mockSession.Object);

            // Act
            var result = controller.RecoveryForm(vm) as ViewResult;
            var resultModel = result.Model as RecoveryVM;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(resultModel.Status, RecoveryStatus.Failed);
        }

        [TestMethod]
        public void RecoveryForm_RecoveryIsNull_ViewResultWithFailedModel()
        {
            // Arrange
            var data = new List<Recovery>()
            {
                new Recovery
                {
                    Id = 1,
                    Key = "TestKey",
                    User = null
                }
            }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(data);
            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Recoveries).Returns(mockSet.Object);

            var vm = new RecoveryVM { Password = "Test", PasswordCheck = "Test" };

            var mockSession = new Mock<ISessionHelper>();
            mockSession.Setup(s => s.GetSessionItem("recoveryKey")).Returns("NotTestKey");
            
            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object, mockSession.Object);

            // Act
            var result = controller.RecoveryForm(vm) as ViewResult;
            var resultModel = result.Model as RecoveryVM;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(resultModel.Status, RecoveryStatus.Failed);
        }

        [TestMethod]
        public void RecoveryForm_UserIsNull_ViewResultWithFailedModel()
        {
            // Arrange
            var data = new List<Recovery>()
            {
                new Recovery
                {
                    Id = 1,
                    Key = "TestKey",
                    User = null
                }
            }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(data);
            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Recoveries).Returns(mockSet.Object);

            var vm = new RecoveryVM { Password = "Test", PasswordCheck = "Test" };

            var mockSession = new Mock<ISessionHelper>();
            mockSession.Setup(s => s.GetSessionItem("recoveryKey")).Returns("TestKey");

            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object, mockSession.Object);

            // Act
            var result = controller.RecoveryForm(vm) as ViewResult;
            var resultModel = result.Model as RecoveryVM;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(resultModel.Status, RecoveryStatus.Failed);
        }

        [TestMethod]
        public void RecoveryForm_Succesfull_ViewResultWithFailedModel()
        {
            // Arrange
            var data = new List<Recovery>()
            {
                new Recovery
                {
                    Id = 1,
                    Key = "TestKey",
                    User = new User { Password = Hashing.Hash("Test") }
                }
            }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(data);
            var mockContext = new Mock<Context>();
            mockContext.SetupGet(c => c.Recoveries).Returns(mockSet.Object);

            var vm = new RecoveryVM { Password = "Test", PasswordCheck = "Test" };

            var mockSession = new Mock<ISessionHelper>();
            mockSession.Setup(s => s.GetSessionItem("recoveryKey")).Returns("TestKey");

            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object, mockSession.Object);

            // Act
            var result = controller.RecoveryForm(vm) as ViewResult;
            var resultModel = result.Model as RecoveryVM;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(resultModel.Status, RecoveryStatus.Changed);
        }

        #endregion

        #region RecoveryNew

        [TestMethod]
        public void RecoveryNew_EmailIsNull_ViewResult()
        {
            // Arrange
            var vm = new RecoveryVM { Email = null };
            
            var mockContext = new Mock<Context>();
            var mockUserHelper = new Mock<IUserHelper>();
            var mockSession = new Mock<ISessionHelper>();
            var controller = new AccountController(mockContext.Object, mockUserHelper.Object, mockSession.Object);

            // Act
            var result = controller.RecoveryNew(vm) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion
    }
}
