﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.Command
{
    public class ModifyUserCommand
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
