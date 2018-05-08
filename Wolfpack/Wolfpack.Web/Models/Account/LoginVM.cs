using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Account
{
    public class LoginVM
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}