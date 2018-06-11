using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wolfpack.Web.Helpers.Interfaces;

namespace Wolfpack.Web.Helpers
{
    public class SessionHelper : ISessionHelper
    {
        public object GetSessionItem(string name)
        {
            return HttpContext.Current.Session[name];
        }

        public void SetSessionItem(string name, object item)
        {
            HttpContext.Current.Session[name] = item;
        }
    }
}