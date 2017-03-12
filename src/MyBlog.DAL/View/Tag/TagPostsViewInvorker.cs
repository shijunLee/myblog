using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.DataBind;
using MyBlog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MyBlog.DAL.View.Tag
{
    public class TagPostsViewInvorker : IViewDisplay<TagPostsDataBind, List<TagPostsViewModel>>
    {

        private readonly IDapperConnection _dapperConnection;
        private readonly ILogger<TagPostsViewInvorker> _logger;

        public TagPostsViewInvorker(IDapperConnection dapperConnection, ILogger<TagPostsViewInvorker> logger)
        {
            this._dapperConnection = dapperConnection;
            this._logger = logger;
        }


        public List<TagPostsViewModel> Execute(TagPostsDataBind command)
        {

            try
            {
                const string sql = "SELECT t.tag as TagName,count(t.post_id) as TagPostCount from tag t GROUP BY t.tag";
                const string allPostSql = @"select t.tag as PostTag,p.post_title as PostTitle,
p.post_sub_title as PostSubTitle,p.post_id as PostId,p.post_date as PostCreateDate from post p left join tag t on t.post_id = p.post_id";
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    var tagResult = conn.Query<TagPostsViewModel>(sql).ToList();
                    var tagPostInfos = conn.Query<TagPostInfo>(allPostSql);
                    foreach (var item in tagResult)
                    {
                        item.TagPostInfos = tagPostInfos.Where(p => p.PostTag == item.TagName).OrderByDescending(p => p.PostCreateDate).ToList();
                    }
                    return tagResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("获取标签页信息失败出现异常，异常信息为{0}",ex));
                return null;
            }
        

           // throw new NotImplementedException();
        }
    }
}
