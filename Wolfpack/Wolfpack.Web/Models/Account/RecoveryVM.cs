using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wolfpack.Data.Models;

namespace Wolfpack.Web.Models.Account
{
    public class RecoveryVM
    {
        [Required(ErrorMessage = "Please fill a correct email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please fill in a new password")]
        [StringLength(24, MinimumLength = 8)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please fill in your password again")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [StringLength(24, MinimumLength = 8)]
        public string ConfirmPassword { get; set; }
    }
}