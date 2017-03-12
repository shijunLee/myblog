using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.Command
{
    public class UpdatePostCommand:AddPostCommand
    {
        public long PostId { get; set; }
    }
}
