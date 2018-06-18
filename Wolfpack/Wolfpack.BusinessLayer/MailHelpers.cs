using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wolfpack.BusinessLayer
{
    public static class MailHelpers
    {
        /// <summary>
        /// Checks if the email provided is a valid email and returns a boolean
        /// </summary>
        public static bool CheckIfValidEmail(string email)
        {
            string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            Match match = Regex.Match(email.Trim(), pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }
    }
}
