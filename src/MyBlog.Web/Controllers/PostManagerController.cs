using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBlog.DAL.Admin;
using MyBlog.Model.Command;
using MyBlog.Model.ViewModel.Admin;
using Microsoft.AspNetCore.Authorization;
using MyBlog.Model.DataModel;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{

    [Authorize]
    public class PostManagerController : Controller
    {

        private readonly ICommandInvoker<PostIndexCommand, List<PostManagerListViewModel>> _postManagerIndexCommandInvorker;
        private readonly ILogger<PostManagerController> _logger;
        private readonly ICommandInvoker<AddPostCommand, CommonResult> _addPostCommandInvorker;
        private readonly ICommandInvoker<GetPostInfoCommand, PostInfoVIewModel> _getPostInfoCommandInvorker;
        private readonly ICommandInvoker<UpdatePostCommand, CommonResult> _updatePostCommandInvorker;
        public PostManagerController(ILogger<PostManagerController> logger, 
            ICommandInvoker<PostIndexCommand, List<PostManagerListViewModel>> postManagerIndexCommandInvorker,
            ICommandInvoker<AddPostCommand, CommonResult> addPostCommandInvorker, 
            ICommandInvoker<GetPostInfoCommand, PostInfoVIewModel> getPostInfoCommandInvorker,
            ICommandInvoker<UpdatePostCommand, CommonResult> updatePostCommandInvorker)
        {
            this._postManagerIndexCommandInvorker = postManagerIndexCommandInvorker;
            this._logger = logger;
            this._addPostCommandInvorker = addPostCommandInvorker;
            this._getPostInfoCommandInvorker = getPostInfoCommandInvorker;
            this._updatePostCommandInvorker = updatePostCommandInvorker;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var result = _postManagerIndexCommandInvorker.Execute(new PostIndexCommand());
            if (result == null)
            {
                result = new List<PostManagerListViewModel>();
            }

            return View(result);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update(long postId)
        {
            var result = _getPostInfoCommandInvorker.Execute(new GetPostInfoCommand() { PostId=postId });
            return View(result);
        }

        [HttpPost]
        public IActionResult Update(long postId, string postTitle, string postSlug, string postSubTitle, string headMask, int layoutType,
            string postMarkDown, string tags, int published, string pubDate, string postHeadImageUrl)
        {

            UpdatePostCommand command = new UpdatePostCommand();
            command.HeadMask = headMask;
            command.LayoutType = layoutType;
            command.PostHeadImageUrl = postHeadImageUrl;
            command.PostMarkDown = postMarkDown;
            command.PostSlug = postSlug;
            command.postSubTitle = postSubTitle;
            command.PostTitle = postTitle;
            command.Published = published;
            command.Tags = tags;
            command.PostId = postId;
            var result = _updatePostCommandInvorker.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        // [ValidateInput(false)]
        public IActionResult Add(string postTitle,string postSlug,string postSubTitle,string headMask,int layoutType,
            string postMarkDown,string tags, int published,string pubDate,string postHeadImageUrl)
        {
            AddPostCommand command = new AddPostCommand();
            command.HeadMask = headMask;
            command.LayoutType = layoutType;
            command.PostHeadImageUrl = postHeadImageUrl;
            command.PostMarkDown = postMarkDown;
            command.PostSlug = postSlug;
            command.postSubTitle = postSubTitle;
            command.PostTitle = postTitle;
            command.Published =  published;
            command.Tags = tags;
            var result = _addPostCommandInvorker.Execute(command);

            return RedirectToAction("Index");
        }
    }
}
