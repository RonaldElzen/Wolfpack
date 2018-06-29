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
    public class NewRegisterExtensionTest
    {
        [TestMethod]
        public void GetByID_Flow_NewRegister()
        {
            var key = "test";
            var newRegisters = new List<NewRegister> { new NewRegister { Id = 1, Key = key } }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(newRegisters);

            var result = NewRegisterExtension.GetByKey(mockSet.Object, key);
            Assert.AreEqual(key, result.Key);
        }
    }
}
