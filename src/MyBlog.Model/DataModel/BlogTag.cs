using MyBlog.Common.DapperTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.DataModel
{
    public class BlogTag
    {
        //
        /// <summary>
        /// 标签id
        /// </summary>
        [Column(Name = "tag_id")]
        public int TagId { get; set; }
        //tag
        /// <summary>
        /// 标签内容
        /// </summary>
        [Column(Name = "tag")]
        public string Tag { get; set; }
        //post_id

        /// <summary>
        /// 博客主键id
        /// </summary>
        [Column(Name = "post_id")]
        public int PostId { get; set; }
        //slug
        /// <summary>
        /// 标签 拼音
        /// </summary>
        [Column(Name = "slug")]
        public string Slug { get; set; }

    }
}
