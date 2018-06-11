using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfpack.Web.Helpers.Interfaces
{
    public interface ISessionHelper
    {
        object GetSessionItem(string name);

        void SetSessionItem(string name, object item);
    }
}
