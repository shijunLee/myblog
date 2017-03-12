using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.ViewModel.Admin
{
    public class PostInfoVIewModel
    {
        /// <summary>
        /// post_id 主键
        /// </summary> 
        public long PostId { get; set; }

        /// <summary>
        /// post_date
        /// </summary> 
        public DateTime PostDate { get; set; }

        /// <summary>
        /// post_title 标题
        /// </summary> 
        public string PostTitle { get; set; }


        /// <summary>
        /// post_sub_title 副标题
        /// </summary> 
        public string PostSubTitle { get; set; }
         


        /// <summary>
        /// 内容预览post_content_preview
        /// </summary> 
        public string PostContentPreview { get; set; }

        //post_markdown
        /// <summary>
        /// markdown 文本内容
        /// </summary> 
        public string PostMarkdown { get; set; }
         
        public string PostTitleSlug { get; set; } 

        /// <summary>
        /// 布局类型
        /// </summary>
    
        public int LayoutType { get; set; }

         
        public string PostHeadImageUrl { get; set; }
         
        public double PostHeaderMask { get; set; }
         
        public int? IsPublish { get; set; } = 0;

        public string Tags { get; set; }
    }
}
