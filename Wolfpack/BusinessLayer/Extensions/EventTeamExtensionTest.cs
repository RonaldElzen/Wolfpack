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
    public class EventTeamExtensionTest
    {
        [TestMethod]
        public void GetByID_Flow_EventTeam()
        {
            var id = 1;
            var eventteams = new List<EventTeam> { new EventTeam { Id = id } }.AsQueryable();

            var mockSet = MockHelper.MockDbSet(eventteams);

            var result = EventTeamExtension.GetById(mockSet.Object, id);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public void GetTotalsPerSkill_Flow_IDictionarySkillDouble()
        {
            var skill = new Skill { Id = 1 };
            var mark = 7.0;
            var dict = new Dictionary<Skill, double>
            {
                { skill, mark }
            };

            var eventTeam = new EventTeam
            {
                Users = new List<User>
                {
                    new User
                    {
                        Id = 1,
                        UserSkills = new List<UserSkill>
                        {
                            new UserSkill
                            {
                                Skill = skill,
                                Ratings = new List<Rating>
                                {
                                    new Rating{Mark = mark}
                                }
                            }
                        }
                    }
                }
            };

            var result = EventTeamExtension.GetTotalsPerSkill(eventTeam);
            Assert.AreEqual(dict.First(), result.First());
        }
    }
}
