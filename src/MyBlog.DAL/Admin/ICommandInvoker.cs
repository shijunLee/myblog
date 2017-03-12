using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.DAL.Admin
{
    public  interface ICommandInvoker< in T,out P> 
    {
        P Execute(T command);
    }
}
