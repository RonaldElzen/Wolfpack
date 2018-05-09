using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Wolfpack.Web.Models.Account
{
    public class NewUserVM
    {
        [Required(ErrorMessage = "Please fill in a username.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please fill in your chosen password.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please fill in your chosen password a second time.")]
        public string PasswordCheck { get; set; }

        [Required(ErrorMessage = "Please fill in your emailadres.")]
        public string MailAdress { get; set; }

        [Required(ErrorMessage = "Please fill in your name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please fill in your lastname.")]
        public string LastName { get; set; }

        public string Message { get; set; }
    }
}
