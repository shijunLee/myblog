using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.DataBind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MyBlog.Model.ViewModel;

namespace MyBlog.DAL.View.Tag
{
    public class BlogTagCloudViewInvorker : IViewDisplay<TagCloudDataBind, List<TagCloudViewModel>>
    {

        private readonly IDapperConnection _dapperConnection;
        private readonly ILogger<BlogTagCloudViewInvorker> _logger;

        public BlogTagCloudViewInvorker(IDapperConnection dapperConnection, ILogger<BlogTagCloudViewInvorker> logger)
        {
            this._dapperConnection = dapperConnection;
            this._logger = logger;
        }
        public List<TagCloudViewModel> Execute(TagCloudDataBind command)
        {
            const string sql = "SELECT t.tag as TagName,count(t.post_id) as TagPostCount from tag t GROUP BY t.tag";
            using (var conn = _dapperConnection.GetDataBaseConnection())
            {
                var result = conn.Query<TagCloudViewModel>(sql);
                if (result != null && result.Count() > 0)
                {
                    return result.ToList();
                }
            }
            return null;
        }
    }

}
