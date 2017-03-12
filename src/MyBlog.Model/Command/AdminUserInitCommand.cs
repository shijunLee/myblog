using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.Command
{
    public class AdminUserInitCommand
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string EMail { get; set; }
    }
}
