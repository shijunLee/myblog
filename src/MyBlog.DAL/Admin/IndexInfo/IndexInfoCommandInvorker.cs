using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.Command;
using MyBlog.Model.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MyBlog.DAL.Admin.IndexInfo
{
    public class IndexInfoCommandInvorker : ICommandInvoker<IndexInfoCommand, CommonResult<IndexInfoViewModel>>
    {

        private readonly ILogger<IndexInfoCommandInvorker> _logger;
        private readonly IDapperConnection _dapperConnection;

        public IndexInfoCommandInvorker(ILogger<IndexInfoCommandInvorker> logger, IDapperConnection dapperConnection)
        {
            this._dapperConnection = dapperConnection;
            this._logger = logger;
        }


        public CommonResult<IndexInfoViewModel> Execute(IndexInfoCommand command)
        {
            try
            {
                const string countSql = "select count(*) from  post";
                const string countTagSql = "select count(distinct(tag)) from tag";
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    var result = conn.QuerySingle<int>(countSql);
                    var result2 = conn.QuerySingle<int>(countTagSql);
                    IndexInfoViewModel resultInfo = new IndexInfoViewModel();
                    resultInfo.BlogCount = result;
                    resultInfo.TagCount = result2;
                    return new CommonResult<IndexInfoViewModel>()
                    {
                        State = 1,
                        Message = string.Empty,
                        Value = resultInfo
                    };
                }
            }
            catch (Exception ex)
            {
                return new CommonResult<IndexInfoViewModel>()
                {
                    State = 0 ,
                    Message="进行数量获取出现异常"
                };
            }
            
                
        }
    }
}
