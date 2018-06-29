using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wolfpack.BusinessLayer.Extensions;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Tests;

namespace BusinessLayer.Extensions
{
    [TestClass]
    public class SkillExtensionTest
    {
        [TestMethod]
        public void GetByID_Flow_Skill()
        {
            var name = "test";
            var skills = new List<Skill> { new Skill { Id = 1, Name = name } }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(skills);

            var result = SkillExtension.GetByName(mockSet.Object, name);
            Assert.AreEqual(name, result.Name);
        }
    }
}
