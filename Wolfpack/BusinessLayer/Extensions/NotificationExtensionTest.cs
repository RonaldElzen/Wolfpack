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
    public class NotificationExtensionTest
    {
        [TestMethod]
        public void GetByID_Flow_Notification()
        {
            var id = 1;
            var notifications = new List<Notification> { new Notification { Id = id } }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(notifications);

            var result = NotificationExtension.GetById(mockSet.Object, id);
            Assert.AreEqual(id, result.Id);
        }
    }
}
