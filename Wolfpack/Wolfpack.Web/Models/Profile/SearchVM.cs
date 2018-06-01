using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Profile
{
    public class SearchVM
    {
        public String UserName { get; set; }
        public IEnumerable<UserVM> PossibleUsers { get; set; }
    }
}