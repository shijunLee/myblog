using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.DAL.View
{
    public interface IViewDisplay<in T, out P>
    {
        P Execute(T command);
    }
}
