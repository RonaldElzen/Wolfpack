using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wolfpack.BusinessLayer;
using Wolfpack.BusinessLayer.Extensions;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Tests;

namespace BusinessLayer.Extensions
{
    [TestClass]
    public class RecoveryExtensionTest
    {
        [TestMethod]
        public void GetByID_Flow_Recovery()
        {
            var key = "test";
            var recoveries = new List<Recovery> { new Recovery { Id = 1, Key = key } }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(recoveries);

            var result = RecoveryExtension.GetByKey(mockSet.Object, key);
            Assert.AreEqual(key, result.Key);
        }
    }
}
