﻿using System;
using System.Collections.Generic;

namespace Wolfpack.Web.Models.Group
{
    public class AddUserVM
    {
        public int GroupId { get; set; }
        public int Id { get; set; }
        public String UserName { get; set; }
        public IEnumerable<UserVM> PossibleUsers { get; set; }
        public string Message { get; set; }
    }
}