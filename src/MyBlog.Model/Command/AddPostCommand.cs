using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.Command
{
    public class AddPostCommand
    {
        public string PostTitle { get; set; }

        public string PostSlug { get; set; }

        public string postSubTitle { get; set; }

        public string HeadMask { get; set; }

        public int LayoutType { get; set; }

        public string PostMarkDown { get; set; }

        public string Tags { get; set; }

        public int Published { get; set; }

        public string pubDate { get; set; }

        public string PostHeadImageUrl { get; set; }
    }
}
