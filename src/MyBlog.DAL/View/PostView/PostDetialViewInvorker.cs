using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.DataBind;
using MyBlog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MyBlog.Model.DataModel;
using HeyRed.MarkdownSharp;

namespace MyBlog.DAL.View.PostView
{
    public class PostDetialViewInvorker : IViewDisplay<PostDetailDataBind, PostDetialViewModel>
    {


        private readonly IDapperConnection _dapperConnection;
        private readonly ILogger<PostDetialViewInvorker> _logger;

        public PostDetialViewInvorker(IDapperConnection dapperConnection, ILogger<PostDetialViewInvorker> logger)
        {
            this._dapperConnection = dapperConnection;
            this._logger = logger; 
        }


        public PostDetialViewModel Execute(PostDetailDataBind command)
        {

            try
            {
                const string postGetSql = "select * from  post  p where p.post_id = @postId";
                const string tagGetsql = "select t.tag from tag t where t.post_id = @postId";
                const string prePostSql = "select p2.post_id,p2.layout_type,p2.post_title from post p2 where p2.post_id=(SELECT max(p.post_id) from post p where p.post_id<@postId)";
                const string nextPostSql = "select p2.post_id,p2.layout_type,p2.post_title from post p2 where p2.post_id=(SELECT min(p.post_id) from post p where p.post_id>@postId)";
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    var postinfo = conn.QueryFirstOrDefault<MyBlog.Model.DataModel.Post>(postGetSql, new { postId = command.PostId });
                    if (postinfo != null)
                    {
                        var tags = conn.Query<string>(tagGetsql, new { postId = command.PostId });
                        var prePost = conn.QueryFirstOrDefault<PostNev>(prePostSql, new { postId = command.PostId });
                        var nevPost = conn.QueryFirstOrDefault<PostNev>(nextPostSql, new { postId = command.PostId });
                        PostDetialViewModel model = new PostDetialViewModel();
                        PostNavigation pre = new PostNavigation();
                        PostNavigation next = new PostNavigation();
                        if (prePost != null)
                        {
                            pre.PostId = prePost.PostId;
                            pre.PostLayoutType = prePost.PostLayoutType;
                            pre.PostTitle = prePost.PostTitle;
                        }
                        else
                        {
                            pre = null;
                        }
                        if (nevPost != null)
                        {
                            next.PostId = nevPost.PostId;
                            next.PostLayoutType = nevPost.PostLayoutType;
                            next.PostTitle = nevPost.PostTitle;
                        }
                        else
                        {
                            next = null;
                        }
                        model.PostTags = tags.ToList();
                        model.Next = next;
                        model.Previous = pre;
                        Markdown markdown = new Markdown();
                        model.PostContent = markdown.Transform(postinfo.PostMarkdown); //postinfo.PostContent;
                        model.PostDate = postinfo.PostDate.ToString("yyyy-MM-dd HH:mm:ss");
                        model.PostHeaderImageUrl = postinfo.PostHeadImgUrl;
                        model.PostHeaderMask = postinfo.HeaderMask.ToString();
                        model.PostId = postinfo.PostId;
                        model.PostSubTitle = postinfo.PostSubTitle;
                        model.PostTitle = postinfo.PostTitle;
                        model.PostLayoutType = postinfo.LayoutType;
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("查询博客详细信息出现异常，异常信息为{0}", ex));
            } 
            return null;
        }
    }
}
