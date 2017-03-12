using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using MyBlog.Model.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MyBlog.DAL.Admin.Login
{
    public class AdminUserInitCommandInvoker : ICommandInvoker<AdminUserInitCommand, CommonResult>
    {

        private readonly IDapperConnection _dapperConnection;
        private readonly ILogger<AdminUserInitCommandInvoker> _logger;
        //public AdminUserConfig 
        
        public AdminUserInitCommandInvoker(IDapperConnection dapperConection, ILogger<AdminUserInitCommandInvoker> logger)
        {
            this._dapperConnection = dapperConection;
            this._logger = logger;
        }
        public CommonResult Execute(AdminUserInitCommand command)
        {
            try
            {
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    var count = conn.QuerySingle<int>("select count(1) from user_info");
                    if (count == 0)
                    {
                        conn.Execute("INSERT into user_info(user_name,user_email,password) VALUES(@userName, @userEmail, @password) ",
                            new { userName = command.UserName, userEmail = command.EMail, password = command.Password });
                    }
                }
                return new CommonResult() { State = 1, Message = string.Empty };
            }
            catch (Exception ex)
            {
                return new CommonResult() { State = 0, Message = string.Empty };
            }
            
        }
    }
}
