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
    public class GroupExtensionTest
    {
        [TestMethod]
        public void GetByID_Flow_Group()
        {
            var id = 1;
            var groups = new List<Group> { new Group { Id = id } }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(groups);

            var result = GroupExtension.GetById(mockSet.Object, id);
            Assert.AreEqual(id, result.Id);
        }
    }
}
