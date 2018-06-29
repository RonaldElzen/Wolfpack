using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Wolfpack.Data.Models;
using Wolfpack.Data;
using System.Web.Mvc;
using Wolfpack.Web.Controllers;

namespace Wolfpack.Web.Tests.Controllers
{
    [TestClass]
    public class SkillControllerTest
    {
        [TestMethod]
        public void GetSkillsPost_Flow_JsonResult()
        {
            // Arrange
            var skills = new List<Skill>()
            {
                new Skill
                {
                    Id = 1,
                    Name = "Unit Test"
                }
            }.AsQueryable();

            var mockContext = new Mock<Context>();

            ///Mock the grouplist into mockdb
            var mockSkills = MockHelper.MockDbSet(skills);
            mockContext.SetupGet(c => c.Skills).Returns(mockSkills.Object);

            var controller = new SkillController(mockContext.Object);

            // Act
            JsonResult result = controller.GetSkills("Un") as JsonResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
