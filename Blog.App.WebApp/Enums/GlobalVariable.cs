using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.App.WebApp.Enums
{
    public static class GlobalVariable
    {
        public static string CurrentUser { get; set; }
        public static int? CurrentUserID { get; set; }
        public static bool IsAdminUser { get; set; }
    }
}
