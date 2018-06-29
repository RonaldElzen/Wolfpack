using System.Collections.Generic;
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
    public class UserExtensionTest
    {
        [TestMethod]
        public void GetBestSkill_Flow_Skill()
        {
            var skill = new Skill
            {
                Id = 1
            };

            var user = new User
            {
                UserSkills = new List<UserSkill>
                {
                    new UserSkill
                    {
                        Ratings = new List<Rating>
                        {
                            new Rating { Mark = 8.0 }
                        },
                        Skill = skill
                    }
                }
            };

            var result = UserExtension.GetBestSkill(user);
            Assert.AreEqual(skill, result);
        }

        [TestMethod]
        public void GetWorstSkill_Flow_Skill()
        {
            var skill = new Skill
            {
                Id = 1
            };

            var user = new User
            {
                UserSkills = new List<UserSkill>
                {
                    new UserSkill
                    {
                        Ratings = new List<Rating>
                        {
                            new Rating { Mark = 8.0 }
                        },
                        Skill = skill
                    }
                }
            };

            var result = UserExtension.GetWorstSkill(user);
            Assert.AreEqual(skill, result);
        }

        [TestMethod]
        public void GetSkillRatings_Flow_IEnumerableDouble()
        {
            var rating = 7.0;

            var user = new User
            {
                UserSkills = new List<UserSkill>
                {
                    new UserSkill
                    {
                        Ratings = new List<Rating>
                        {
                            new Rating { Mark = rating }
                        },
                    }
                }
            };

            var result = UserExtension.GetSkillRatings(user);
            Assert.AreEqual(rating, result.Average());
        }

        [TestMethod]
        public void AverageSkillScore_Flow_Double()
        {
            var rating = 7.0;

            var user = new User
            {
                UserSkills = new List<UserSkill>
                {
                    new UserSkill
                    {
                        Ratings = new List<Rating>
                        {
                            new Rating { Mark = rating }
                        },
                    }
                }
            };

            var result = UserExtension.AverageSkillScore(user);
            Assert.AreEqual(rating, result);
        }

        [TestMethod]
        public void TotalSkillScore_Flow_Double()
        {
            var rating = 7.0;

            var user = new User
            {
                UserSkills = new List<UserSkill>
                {
                    new UserSkill
                    {
                        Ratings = new List<Rating>
                        {
                            new Rating { Mark = rating }
                        },
                    }
                }
            };

            var result = UserExtension.TotalSkillScore(user);
            Assert.AreEqual(rating, result);
        }

        [TestMethod]
        public void GetByMail_Flow_User()
        {
            var mail = "test@test.com";
            var users = new List<User> { new User { Mail = mail } }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(users);

            var result = UserExtension.GetByMail(mockSet.Object, mail);
            Assert.AreEqual(mail, result.Mail);
        }

        [TestMethod]
        public void GetByID_Flow_User()
        {
            var id = 1;
            var users = new List<User> { new User { Id = id } }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(users);

            var result = UserExtension.GetById(mockSet.Object, id);
            Assert.AreEqual(id, result.Id);
        }
    }
}
