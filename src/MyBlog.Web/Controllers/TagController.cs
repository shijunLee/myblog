using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBlog.DAL.View;
using MyBlog.Model.DataBind;
using MyBlog.Model.ViewModel;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    public class TagController : Controller
    {
        private IViewDisplay<TagPostsDataBind, List<TagPostsViewModel>> _tagPostViewInvorker;

        public TagController(IViewDisplay<TagPostsDataBind, List<TagPostsViewModel>> tagPostViewInvorker)
        {
            this._tagPostViewInvorker = tagPostViewInvorker;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var result = this._tagPostViewInvorker.Execute(null);
            return View(result);
        }
    }
}
