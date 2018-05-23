using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wolfpack.Data;
using Wolfpack.Web;
using Wolfpack.Web.Controllers;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Tests.Web.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_Flow_ViewResult()
        {
            // Arrange
            var mockContext = new Mock<Context>();
            var mockUserHelper = new Mock<IUserHelper>();
            var controller = new HomeController(mockContext.Object, mockUserHelper.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
