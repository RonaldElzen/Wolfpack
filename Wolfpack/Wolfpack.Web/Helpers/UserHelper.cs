using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wolfpack.Data;
using Wolfpack.Data.Models;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public const string CURRENTUSER = "currentUser";

        /// <summary>
        /// Sets the current user. This is used during the login.
        /// </summary>
        /// <param name="user">User to set</param>
        public void SetCurrentUser(User user)
        {
            HttpContext.Current.Session[CURRENTUSER] = new UserHelper
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }

        /// <summary>
        /// Gets the current logged in user
        /// </summary>
        /// <returns>The current logged in user</returns>
        public IUserHelper GetCurrentUser()
        {
            return (UserHelper)HttpContext.Current.Session[CURRENTUSER];
        }

        /// <summary>
        /// Gets the user object from the database from the currently logged in user
        /// </summary>
        /// <param name="context">Database connection</param>
        /// <returns>user object from database</returns>
        public User GetCurrentDbUser(Context context)
        {
            var user = (UserHelper)HttpContext.Current.Session[CURRENTUSER];

            return context.Users.SingleOrDefault(u => u.Id == user.Id);
        }

        /// <summary>
        /// Clears the current user from the session. Used to log out.
        /// </summary>
        public void ClearCurrentUser()
        {
            HttpContext.Current.Session[CURRENTUSER] = null;
        }
    }
}