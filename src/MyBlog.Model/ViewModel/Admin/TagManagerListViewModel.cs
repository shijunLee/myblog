using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.ViewModel.Admin
{
    public class TagManagerListViewModel
    {
        public string TagName { get; set; }

        public string TagSlug { get; set; }

        public int TagCount { get; set; }
    }
}
