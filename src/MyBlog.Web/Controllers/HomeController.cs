using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBlog.Common.Tool;
using Mono.System.Drawing;
using MyBlog.DAL;
using MyBlog.Model.Command;
using MyBlog.Model.ViewModel;
using MyBlog.Model.DataBind;
using MyBlog.DAL.View;

namespace MyBlog.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

       
        private readonly INetWorkTool _netWorkTool;

        private readonly IViewDisplay<PostListDataBind, PageInfo<PostListViewModel>> _postListView;

        private readonly IViewDisplay<TagCloudDataBind, List<TagCloudViewModel>> _tagCloudView;

        private readonly IViewDisplay<PostDetailDataBind, PostDetialViewModel> _postDetialView;
        public HomeController(ILogger<HomeController> logger, 
            INetWorkTool netWorkTool, IViewDisplay<PostListDataBind, PageInfo<PostListViewModel>> postListCommand,
            IViewDisplay<TagCloudDataBind, List<TagCloudViewModel>> tagCloudView, IViewDisplay<PostDetailDataBind, PostDetialViewModel> postDetialView)
        {
            _logger = logger;
            //this._verificationCodeTool = verificationCodeTool;
            this._postListView = postListCommand;
            this._netWorkTool = netWorkTool;
            this._tagCloudView = tagCloudView;
            this._postDetialView = postDetialView;
        }


        public IActionResult PostDetail(long postId)
        {
            var result = _postDetialView.Execute(new PostDetailDataBind { PostId = postId });
            if (result == null)
            {
                RedirectToAction("Error", "Home");
            }
            TagCloudDataBind tagCloudDataBind = new TagCloudDataBind();
            var tagResult = _tagCloudView.Execute(tagCloudDataBind);
            Tuple<PostDetialViewModel, List<TagCloudViewModel>> tuple = new Tuple<PostDetialViewModel, List<TagCloudViewModel>>(result, tagResult);
            return View(tuple);
        }

        public IActionResult Post(long postId, int postLayout = 1)
        {
            //switch (postLayout)
            //{
            //    case 1:
            //        return RedirectToAction("PostDetail", "Home",new { postId = postId});
            //    case 2:
            //        return RedirectToAction("KeyNote", "Home", new { postId = postId });
            //    default:
            return RedirectToAction("PostDetail", "Home", new { postId = postId });
            //}
        }

        //public IActionResult KeyNote(long postId)
        //{
        //    var result = _postDetialView.Execute(new PostDetailDataBind { PostId = postId });

        //    if (result == null)
        //    {
        //        RedirectToAction("Error", "Home");
        //    }
        //    TagCloudDataBind tagCloudDataBind = new TagCloudDataBind();
        //    var tagResult = _tagCloudView.Execute(tagCloudDataBind);
        //    Tuple<PostDetialViewModel, List<TagCloudViewModel>> tuple = new Tuple<PostDetialViewModel, List<TagCloudViewModel>>(result, tagResult);
        //    return View(tuple);

        //}

        public IActionResult Index(int page = 1, int pageSize = 10)
        {

            PostListDataBind postListDataBind = new PostListDataBind();
            postListDataBind.PageIndex = page;
            postListDataBind.PageSize = pageSize;
            TagCloudDataBind tagCloudDataBind = new TagCloudDataBind();
            var tagResult = _tagCloudView.Execute(tagCloudDataBind);
            var resultModel = _postListView.Execute(postListDataBind);
            //var ipAddr = _netWorkTool.GetRemoteIp();
            Tuple<PageInfo<PostListViewModel>, List<TagCloudViewModel>> tuple = new Tuple<PageInfo<PostListViewModel>, List<TagCloudViewModel>>(resultModel, tagResult);
            return View(tuple);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

       

        //public IActionResult VerfifyCode(string code)
        //{
        //    string id = this.HttpContext.Session.Id;
        //    var result = _verificationCodeTool.VerfiyVerificationCode(code, id);

        //    return Json(new { State = 1, Value = result ? 1 : 0 });
        //}

        public IActionResult Error()
        {
            return View();
        }
    }
}
