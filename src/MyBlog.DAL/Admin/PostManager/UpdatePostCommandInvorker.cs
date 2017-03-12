using Dapper;
using HeyRed.MarkdownSharp;
using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Common.Extensions;
using MyBlog.Model.Command;
using MyBlog.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBlog.DAL.Admin.PostManager
{
    public class UpdatePostCommandInvorker : ICommandInvoker<UpdatePostCommand, CommonResult>
    {
        private readonly ILogger<UpdatePostCommandInvorker> _logger;
        private readonly IDapperConnection _dapperConnection;

        public UpdatePostCommandInvorker(ILogger<UpdatePostCommandInvorker> logger, IDapperConnection dapperConnection)
        {
            this._logger = logger;
            this._dapperConnection = dapperConnection;
        }
        public CommonResult Execute(UpdatePostCommand command)
        {
            const string sql = @"update post set post_date =@post_date , post_title =@post_title ,post_sub_title =@post_sub_title ,
                                    post_content_preview =@post_content_preview ,
                                    post_markdown =@post_markdown ,post_title_slug =@post_title_slug ,
                                    layout_type =@layout_type , post_head_img_url =@post_head_img_url ,
                                    header_mask =@header_mask , is_publish =@is_publish where post_id =@post_id ";
            const string deleteSql = "delete from tag where  post_id = @post_id ";
            const string tagSql = @"INSERT into tag(
                                    tag,
                                    post_id,
                                    slug
                                    )
                                    VALUES(
                                    @tag,
                                    @postId,
                                    @slug
                                    )";
            Markdown markdown = new Markdown();
            try
            {
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    Post post = new Post();
                    post.AuthorDisplayName = "White Lee";
                    post.AuthorEmail = "lishjun01@126.com";
                    post.LayoutType = command.LayoutType;
                    post.PostContent = null;
                    // post.PostContentPreview = command.PostMarkDown;
                    if (!string.IsNullOrWhiteSpace(command.PostMarkDown))
                    {
                        var content = markdown.Transform(command.PostMarkDown);
                        var des = Regex.Replace(content, "<[^>]+>", string.Empty).Trim();
                        var length = des.Length < 240 ? des.Length : 240;
                        des = des.Substring(0, length) + " ...";
                        post.PostContentPreview = des;
                    }
                    post.PostDate = DateTime.Now;
                    double headMask = 0f;
                    if (!string.IsNullOrWhiteSpace(command.HeadMask))
                    {
                        if (double.TryParse(command.HeadMask, out headMask))
                        {
                            post.HeaderMask = headMask;
                        }
                    }
                    post.PostHeadImgUrl = command.PostHeadImageUrl;
                    post.PostMarkdown = command.PostMarkDown;
                    post.PostSubTitle = command.postSubTitle;
                    post.PostTitle = command.PostTitle;
                    post.PostTitleSlug = command.PostSlug;
                    post.IsPublish = command.Published;
                    post.PostId = command.PostId;
                  
                    var result = conn.Execute(sql, new
                    {
                        post_date = post.PostDate,
                        post_title = post.PostTitle,
                        post_sub_title = post.PostSubTitle,
                        post_content_preview = post.PostContentPreview,
                        post_markdown = post.PostMarkdown,
                        post_title_slug = post.PostTitleSlug, 
                        layout_type = post.LayoutType,
                        post_head_img_url = post.PostHeadImgUrl,
                        header_mask = post.HeaderMask,
                        is_publish = post.IsPublish,
                        post_id = post.PostId

                    });

                    if (result > 0)
                    {
                        conn.Execute(deleteSql, new { post_id = command.PostId });
                        var tags = command.Tags.Split(',');
                        foreach (var item in tags)
                        {
                            var tagResult = conn.Execute(tagSql, new
                            {
                                tag = item,
                                postId = command.PostId,
                                slug = item.ToSlug()
                            });
                            if (tagResult != 1)
                            {
                                _logger.LogError(string.Format("添加标签{0}出现错误", item));
                            }
                        }
                        return new CommonResult()
                        {
                            State = 1,
                            Message = string.Empty
                        };
                    }
                    else
                    {
                        return new CommonResult()
                        {
                            State = 0,
                            Message = "更新失败"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("更新blog失败，异常信息为{0}",ex));
                return new CommonResult()
                {
                    State = 0,
                    Message = "更新出现异常"
                };
            }
                  
        }
    }
}
