using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.DAL.Admin
{
    public class CommonResult
    {
        public int State { get; set; }

        public String Message { get; set; }
    }

    public class CommonResult<T>: CommonResult
    {
        public  T Value { get; set; }
    }
}
