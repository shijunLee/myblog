using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.Command;
using MyBlog.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MyBlog.Model.ViewModel.Admin;

namespace MyBlog.DAL.Admin.PostManager
{
    public class GetPostInfoCommandInvorker : ICommandInvoker<GetPostInfoCommand, PostInfoVIewModel>
    {
        private ILogger<GetPostInfoCommandInvorker> _logger;
        private IDapperConnection _dapperConnection;

        public GetPostInfoCommandInvorker(ILogger<GetPostInfoCommandInvorker> logger,
            IDapperConnection dapperConnection)
        {
            this._logger = logger;
            this._dapperConnection = dapperConnection;
        }

        public PostInfoVIewModel Execute(GetPostInfoCommand command)
        {
            try
            {
                const string selectSql = "select * from post where post_id = @postId";
                const string selectTagSql = "select * from tag where post_id = @postId";
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    var tagResult = conn.Query<BlogTag>(selectTagSql, new { postId = command.PostId });
                    var result = conn.QuerySingle<Post>(selectSql, new { postId = command.PostId });
                    PostInfoVIewModel model = new PostInfoVIewModel();
                    var tags = tagResult.Select(p => p.Tag).ToArray();
                    var tagStr = String.Join(",", tags);
                    model.IsPublish = result.IsPublish;
                    model.LayoutType = result.LayoutType;
                    model.PostContentPreview = result.PostContentPreview;
                    model.PostDate = result.PostDate;
                    model.PostHeaderMask = result.HeaderMask;//.HasValue?result.PostHeaderMask.Value:0;
                    model.PostHeadImageUrl = result.PostHeadImgUrl;
                    model.PostId = result.PostId;
                    model.PostMarkdown = result.PostMarkdown;
                    model.PostSubTitle = result.PostSubTitle;
                    model.PostTitle = result.PostTitle;
                    model.PostTitleSlug = result.PostTitleSlug;
                    model.Tags = tagStr;
                    return model;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("获取博客信息出现异常{0}",ex));
                return null;
            }

             
        }
    }
}
