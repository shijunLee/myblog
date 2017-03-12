using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.DataBind
{
    public class PostListDataBind
    {
        
        /// <summary>
        /// 请求分页数
        /// </summary>
        public int PageSize { get; set; }

       
        /// <summary>
        ///  请求页数
        /// </summary>
        public int PageIndex { get; set; }
    }
}
