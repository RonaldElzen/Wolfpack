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
    public class EventExtensionTest
    {
        [TestMethod]
        public void GetByID_Flow_Event()
        {
            var id = 1;
            var events = new List<Event>{ new Event { Id = id } }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(events);

            var result = EventExtension.GetById(mockSet.Object, id);
            Assert.AreEqual(id, result.Id);
        }
    }
}
