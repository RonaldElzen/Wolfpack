using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wolfpack.BusinessLayer;

namespace BusinessLayer
{
    [TestClass]
    public class HashingTest
    {
        [TestMethod]
        public void Hash_HashedPassword_HasLengthHigherThanTen()
        {
            var hashedPassword = Hashing.Hash("testPassword");

            Assert.IsTrue(hashedPassword.Length > 10);
        }
        
        [TestMethod]
        public void Hash_HashedPassword_StartsWithInfo()
        {
            var hashedPassword = Hashing.Hash("testPassword");

            Assert.IsTrue(hashedPassword.StartsWith("$MYHASH$V1$"));
        }

        [TestMethod]
        public void Verify_HashedPasswordAgainstSamePassword_IsTrue()
        {
            var password = "testPassword";
            var hashedPassword = Hashing.Hash(password);

            Assert.IsTrue(Hashing.Verify(password, hashedPassword));
        }

        [TestMethod]
        public void Verify_HashedPasswordAgainstOtherPassword_IsFalse()
        {
            var password = "testPassword";
            var hashedPassword = Hashing.Hash("otherPassword");

            Assert.IsFalse(Hashing.Verify(password, hashedPassword));
        }
    }
}
