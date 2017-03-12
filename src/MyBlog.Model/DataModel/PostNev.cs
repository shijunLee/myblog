using MyBlog.Common.DapperTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.DataModel
{
    public class PostNev
    {

        /// <summary>
        /// post_id 主键
        /// </summary>
        [Column(Name = "post_title")]
        public string PostTitle { get; set; }

        /// <summary>
        /// post_id 主键
        /// </summary>
        [Column(Name = "post_id")]

        public long PostId { get; set; }


        /// <summary>
        /// post_id 主键
        /// </summary>
        [Column(Name = "layout_type")]
        public int PostLayoutType { get; set; }
    }
}
