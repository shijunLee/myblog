using MyBlog.Common.DapperTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.DataModel
{
    public class Post
    {
        /// <summary>
        /// post_id 主键
        /// </summary>
        [Column(Name = "post_id")]
        public long PostId { get; set; }

        /// <summary>
        /// post_date
        /// </summary>
        [Column(Name = "post_date")]
        public DateTime PostDate { get; set; }

        /// <summary>
        /// post_title 标题
        /// </summary>
        [Column(Name = "post_title")]
        public string PostTitle { get; set; }


        /// <summary>
        /// post_sub_title 副标题
        /// </summary>
        [Column(Name = "post_sub_title")]
        public string PostSubTitle { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Column(Name = "post_content")]
        public string PostContent { get; set; }


        /// <summary>
        /// 内容预览post_content_preview
        /// </summary>
        [Column(Name = "post_content_preview")]
        public string PostContentPreview { get; set; }

        //post_markdown
        /// <summary>
        /// markdown 文本内容
        /// </summary>
        [Column(Name = "post_markdown")]
        public string PostMarkdown { get; set; }
       
        //post_title_slug
        /// <summary>
        /// 标题汉语拼音
        /// </summary>
        [Column(Name = "post_title_slug")]
        public string PostTitleSlug { get; set; }
 

        /// <summary>
        /// 编辑人显示名称
        /// </summary>
        [Column(Name = "author_display_name")]
        public string AuthorDisplayName { get; set; }
        //author_email
        /// <summary>
        /// 编辑人email
        /// </summary>
        [Column(Name = "author_email")]
        public string AuthorEmail { get; set; }


        /// <summary>
        /// 用户id
        /// </summary>
        [Column(Name = "user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// 布局类型
        /// </summary>
        [Column(Name = "layout_type")]
        public int LayoutType { get; set; } = 0;


        [Column(Name = "post_head_img_url")]
        public string PostHeadImgUrl { get; set; }

        [Column(Name = "header_mask")]
        public double HeaderMask { get; set; }

        //is_publish
        [Column(Name = "is_publish")]
        public int? IsPublish { get; set; } = 0;
    }
}
