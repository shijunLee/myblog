using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.Command;
using MyBlog.Model.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MyBlog.Model.DataModel;

namespace MyBlog.DAL.Admin.PostManager
{
    public class PostManagerIndexCommandInvorker : ICommandInvoker<PostIndexCommand, List<PostManagerListViewModel>>
    {

        private readonly ILogger<PostManagerIndexCommandInvorker> _logger;

        private readonly IDapperConnection _dapperConnection;

        public PostManagerIndexCommandInvorker(ILogger<PostManagerIndexCommandInvorker> logger,
            IDapperConnection dapperConnection)
        {
            this._dapperConnection = dapperConnection;
            this._logger = logger;
        }

        public List<PostManagerListViewModel> Execute(PostIndexCommand command)
        {

            List<PostManagerListViewModel> listResult = new List<PostManagerListViewModel>();
            const string sql = "select a.post_id,a.post_title,a.post_sub_title,a.post_date from post a ";
            try
            {
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    var result = conn.Query<Post>(sql);
                    foreach (var item in result)
                    {
                        PostManagerListViewModel viewModel = new PostManagerListViewModel();
                        viewModel.PostDate = item.PostDate.ToString("yyyy-MM-dd HH:mm:ss");
                        viewModel.PostId = item.PostId;
                        viewModel.PostSubTitle = item.PostSubTitle;
                        viewModel.PostTitle = item.PostTitle;
                        listResult.Add(viewModel);
                    }
                }
                return listResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("获取博客列表失败，异常信息为{0}",ex));
                return null;
            } 
        }
    }
}
