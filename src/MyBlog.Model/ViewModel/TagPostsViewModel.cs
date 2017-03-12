using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.ViewModel
{
    public class TagPostsViewModel
    {
        public string TagName { get; set; }

        public string TagPostCount { get; set; }

        public List<TagPostInfo> TagPostInfos { get; set;}
    }

    public class TagPostInfo
    {
        public string PostTitle { get; set; }

        public string PostId { get; set; }

        public string PostSubTitle { get; set; }

        public DateTime PostCreateDate { get; set; }

        public string PostTag { get; set; }

    }
}
