using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.Command
{
    public class TagManagerCommand
    {
        public int PageIndex { get; set;}

        public int PageSize { get; set; }
    }
}
