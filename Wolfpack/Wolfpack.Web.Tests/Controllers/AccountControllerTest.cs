using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wolfpack.Web.Controllers;
using Wolfpack.Web.Models.Account;

namespace Wolfpack.Web.Tests.Controllers
{
    [TestClass]
    public class AccuntControllerTest
    {
        [TestMethod]
        public void Login_GetMethod_ReturnsLoginView()
        {
            var controller = new AccountController();
            var result = controller.Login() as ViewResult;
            Assert.AreEqual("Login", result.ViewName);
        }
    }
}
