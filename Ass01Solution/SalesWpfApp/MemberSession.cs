using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWpfApp
{
    public class MemberSession
    {
        public static Member? CurrentMember = null;

        public static string Role = BusinessObject.Enum.Role.User.ToString();
    }
}
