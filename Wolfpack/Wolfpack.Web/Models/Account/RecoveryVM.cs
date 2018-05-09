using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wolfpack.Data.Models;

namespace Wolfpack.Web.Models.Account
{
    public class RecoveryVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSame { get; set; }
    }
}