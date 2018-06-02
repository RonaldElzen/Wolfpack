using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wolfpack.Web.Models.Group
{
    public class RateVM
    {
        public int SkillToRateId { get; set; }
        public int UserToRateId { get; set; }
        public int GroupId { get; set; }
        public int Rating { get; set; }
        public string RateComment { get; set; }
        public IEnumerable<SelectListItem> UsersList { get; set; }
        public IEnumerable<SelectListItem> SkillsList { get; set; }
    }
}