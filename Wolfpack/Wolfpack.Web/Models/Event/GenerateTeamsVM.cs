using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Web.Helpers.Enums;

namespace Wolfpack.Web.Models.Event
{
    public class GenerateTeamsVM
    {
        public int EventId { get; set; }

        public int TeamSize { get; set; }

        public string Message { get; set; }

        public AlgorithmType AlgorithmType { get; set; }

        public IEnumerable<SelectListItem> AlgorithmTypes { get; set; }
    }
}