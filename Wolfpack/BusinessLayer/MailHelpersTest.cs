using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Wolfpack.BusinessLayer;

namespace BusinessLayer
{
    [TestClass]
    public class MailHelpersTest
    {
        [TestMethod]
        public void CheckIfValidMail_Mail_IsTrue()
        {
            var validMail = MailHelpers.CheckIfValidEmail("wolfpack.nhlstenden@gmail.com");

            Assert.IsTrue(validMail);
        }

        [TestMethod]
        public void CheckIfValidMail_Mail_IsFalse()
        {
            var validMail = MailHelpers.CheckIfValidEmail("wolfpack.nhlstenden@gmail");

            Assert.IsFalse(validMail);
        }
    }
}
