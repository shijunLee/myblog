using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.Command;
using MyBlog.Model.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MyBlog.DAL.Admin.TagManager
{
    public class TagListCommandInvorker : ICommandInvoker<TagManagerCommand, List<TagManagerListViewModel>>
    {
        private readonly ILogger<TagListCommandInvorker> _logger;
        private readonly IDapperConnection _dapperConnection;

        public TagListCommandInvorker(ILogger<TagListCommandInvorker> logger,
            IDapperConnection dapperConnection)
        {
            _logger = logger;
            _dapperConnection = dapperConnection;
        }
        public List<TagManagerListViewModel> Execute(TagManagerCommand command)
        { 
            const string sql = "SELECT t.tag as TagName,count(t.post_id) as TagCount,slug as TagSlug  from tag t GROUP BY t.tag";
            try
            {
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    var result = conn.Query<TagManagerListViewModel>(sql).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("查询标签出现异常，异常信息为{0}",ex));
                return null;
            }
             
        }
    }
}
