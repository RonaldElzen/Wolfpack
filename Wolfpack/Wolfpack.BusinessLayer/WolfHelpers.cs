﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wolfpack.BusinessLayer
{
    //Global helpers class for methods that are needed if multiple places.
    public static class WolfHelpers
    {
        /// <summary>
        /// Checks if the email provided is a valid email and returns a boolean
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool CheckIfValidEmail(string email)
        {
            string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            Match match = Regex.Match(email.Trim(), pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }
    }
}
