using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.ViewModel
{
    public class PostListViewModel
    {
        /// <summary>
        /// PostId
        /// </summary>
        public long PostId { get; set; }

        /// <summary>
        /// 博客内容预览
        /// </summary>
        public string PostContentPreview { get; set; }

        /// <summary>
        /// 博客标题
        /// </summary>
        public string PostTitle { get; set; }

        /// <summary>
        /// 博客创建时间
        /// </summary>
        public string PostCreateTime { get; set; }


        /// <summary>
        /// 博客布局
        /// </summary>
        public int PostLayout { get; set; }

    }
}
