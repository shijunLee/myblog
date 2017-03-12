
using Dapper;
using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.DataBind;
using MyBlog.Model.DataModel;
using MyBlog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.DAL.View.PostView
{
    public class PostListViewInvorker : IViewDisplay<PostListDataBind, PageInfo<PostListViewModel>>
    {

        private readonly IDapperConnection _dapperConnection;
        private readonly ILogger<PostListViewInvorker> _logger;

        public PostListViewInvorker(IDapperConnection dapperConnection, ILogger<PostListViewInvorker> logger)
        {
            this._dapperConnection = dapperConnection;
            this._logger = logger;
        }
        public PageInfo<PostListViewModel> Execute(PostListDataBind command)
        {
            try
            {
                const string countSql = "select count(1) from post p ";
                const string sql = "select p.post_id,p.post_content_preview,p.post_date,p.post_title,p.layout_type from post p ORDER BY p.post_date DESC LIMIT @startIndex,@pagesize ";
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                
                    var count = conn.QueryFirstOrDefault<int>(countSql);
                    var result = conn.Query<Post>(sql, new { startIndex = (command.PageIndex - 1) * command.PageSize, pagesize = command.PageSize });
                    PageInfo<PostListViewModel> resultViewModel = new PageInfo<PostListViewModel>();
                    resultViewModel.CurrentPage = command.PageIndex;
                    resultViewModel.CurrentPageSize = command.PageSize;
                    resultViewModel.TotalPage = (int)Math.Ceiling((double)count / command.PageSize);
                    resultViewModel.TotalRecord = count;

                    if (result != null)
                    {
                        List<PostListViewModel> modelList = new List<PostListViewModel>();
                        foreach (var item in result)
                        {
                            PostListViewModel model = new PostListViewModel();
                            model.PostContentPreview = item.PostContentPreview;
                            model.PostCreateTime = item.PostDate.ToString("yyyy-MM-dd HH:mm:ss");
                            model.PostId = item.PostId;
                            model.PostTitle = item.PostTitle;
                            model.PostLayout = item.LayoutType;
                            modelList.Add(model);
                        }
                        resultViewModel.Value = modelList;

                    }
                    return resultViewModel;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(string.Format("请求博客列表出现异常，异常信息为{0}", ex));
                return null;
            }     
        }
    }
}
