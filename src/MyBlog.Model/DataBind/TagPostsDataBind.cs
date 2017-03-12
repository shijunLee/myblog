using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.DataBind
{
    public class TagPostsDataBind
    {
        public int Threshold { get; set; }

        public int Take { get; set; }

        public TagPostsDataBind()
        {
            Threshold = 1;
            Take = int.MaxValue;
        }
    }
}
