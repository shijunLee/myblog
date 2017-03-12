using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.ViewModel
{
    public class PostDetialViewModel
    {
        public string PostTitle { get; set; }

        public string PostSubTitle { get; set; }

        public long PostId { get; set; }

        public string PostContent { get; set; }

        public string PostHeaderMask { get; set; }

        public string PostHeaderImageUrl { get; set; }

        public int PostLayoutType { get; set; }

        public string PostDate { get; set; }

        public PostNavigation Previous { get; set; }

        public PostNavigation Next { get; set; }

        public List<string> PostTags { get; set; }
    }


    public class PostNavigation
    {
        public PostNavigation()
        { }

        public PostNavigation(string postTitle, long postId, int postLayoutType)
        {
            this.PostTitle = postTitle;
            this.PostId = postId;
            this.PostLayoutType = postLayoutType;

        }
        public string PostTitle { get; set; }

        public long PostId { get; set; }

        public int PostLayoutType { get; set; }
    }
}
