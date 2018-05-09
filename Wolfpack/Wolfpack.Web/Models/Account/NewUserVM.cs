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
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
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

        public string CaptchaResponse { get; set; }
    }
}
