using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Group
{
    public class RatingProgressVM
    {
        public IDictionary<string, KeyValuePair<int, int>> Progress { get; set; }
    }
}