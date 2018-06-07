using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfpack.Data;
using Wolfpack.Data.Models;

namespace Wolfpack.Web.Helpers.Interfaces
{
    public interface IUserHelper
    {
        int Id { get; set; }

        string UserName { get; set; }

        IUserHelper GetCurrentUser();

        void ClearCurrentUser();

        void SetCurrentUser(User user);

        User GetCurrentDbUser(Context context);
    }
}
