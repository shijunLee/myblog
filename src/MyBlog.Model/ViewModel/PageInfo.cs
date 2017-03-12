using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.ViewModel
{
    public class PageInfo<T>
    {
        public int TotalPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalRecord { get; set; }

        public int CurrentPageSize { get; set; }

        public List<T> Value { get; set; }
    }
}
