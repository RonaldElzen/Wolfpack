using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wolfpack.Web.Models.Account
{
    public class NewUserVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordCheck { get; set; }
        public string MailAdress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
