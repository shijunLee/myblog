using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.Command;
using MyBlog.Model.DataModel;
using System;
using HeyRed.MarkdownSharp;
using System.Text.RegularExpressions;
using Dapper;
using MyBlog.Common.Extensions;
namespace MyBlog.DAL.Admin.PostManager
{
    public class AddPostCommandInvorker : ICommandInvoker<AddPostCommand, CommonResult>
    {

        private readonly ILogger<PostManagerIndexCommandInvorker> _logger;

        private readonly IDapperConnection _dapperConnection;

        public AddPostCommandInvorker(ILogger<PostManagerIndexCommandInvorker> logger, IDapperConnection dapperConnection)
        {
            this._logger = logger;
            this._dapperConnection = dapperConnection;
        }
        public CommonResult Execute(AddPostCommand command)
        {
            const string insertSql = @" 
                            INSERT into post(
                            post_date,
                            post_title,
                            post_sub_title,
                            post_content_preview,
                            post_markdown,
                            post_title_slug,
                            author_display_name,
                            author_email,
                            layout_type,
                            post_head_img_url,
                            header_mask,
                            is_publish
                            )
                            values
                            (@post_date,
                            @post_title,
                            @post_sub_title,
                            @post_content_preview,
                            @post_markdown,
                            @post_title_slug,
                            @author_display_name,
                            @author_email,
                            @layout_type,
                            @post_head_img_url,
                            @header_mask,
                            @is_publish);select last_insert_rowid() from post";
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
                    var result = conn.ExecuteScalar<int>(insertSql, new
                    {
                        post_date = post.PostDate,
                        post_title = post.PostTitle,
                        post_sub_title = post.PostSubTitle,
                        post_content_preview = post.PostContentPreview,
                        post_markdown = post.PostMarkdown,
                        post_title_slug = post.PostTitleSlug,
                        author_display_name = post.AuthorDisplayName,
                        author_email = post.AuthorEmail,
                        layout_type = post.LayoutType,
                        post_head_img_url = post.PostHeadImgUrl,
                        header_mask = post.HeaderMask,
                        is_publish = post.IsPublish
                    });

                    if (result > 0)
                    {
                        var tags = command.Tags.Split(',');
                        foreach (var item in tags)
                        {
                            var tagResult = conn.Execute(tagSql, new
                            {
                                tag = item,
                                postId = result,
                                slug = item.ToSlug()
                            });
                            if (tagResult != 1)
                            {
                                _logger.LogError(string.Format("添加标签{0}出现错误",item));
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
                            Message = "添加失败"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("添加博客出现异常，异常信息为{0}",ex));
                return new CommonResult()
                {
                    State = 0,
                    Message = "添加出现异常"
                };
            }
          
        }
    }
}
