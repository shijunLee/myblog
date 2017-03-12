using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Model.ViewModel.Admin;
using MyBlog.Model.Command;
using MyBlog.DAL.Admin;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    public class TagManagerController : Controller
    {
        private readonly ICommandInvoker<TagManagerCommand, List<TagManagerListViewModel>> _tagListCommandInvorker;

        public TagManagerController(ICommandInvoker<TagManagerCommand, List<TagManagerListViewModel>> tagListCommandInvorker)
        {
            _tagListCommandInvorker = tagListCommandInvorker;
        }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            var result = _tagListCommandInvorker.Execute(new TagManagerCommand());

            return View(result);
        }
    }
}
