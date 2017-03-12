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

namespace MyBlog.DAL.Admin.UserManager
{
    public class ModifyUserInfoCommandInvorker:ICommandInvoker<ModifyUserCommand,CommonResult>
    {

        private readonly ILogger<ModifyUserInfoCommandInvorker> _logger;
        private readonly IDapperConnection _dapperConnection;

        public ModifyUserInfoCommandInvorker(ILogger<ModifyUserInfoCommandInvorker> logger,
            IDapperConnection dapperConnection)
        {
            _logger = logger;
            _dapperConnection = dapperConnection;
        }

        public CommonResult Execute(ModifyUserCommand command)
        {
            const string sql = "update user_info set user_name=@user_name,password=@password where user_id=@user_id";

            try
            {
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    var result = conn.Execute(sql, new { user_name = command.UserName, password = command.Password, user_id = command.UserId });

                    if (result > 0)
                    {
                        return new CommonResult() { Message = string.Empty, State = 1 };
                    }
                    else
                    {
                        return new CommonResult() { Message = string.Empty, State = 0 };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("更新用户信息出现异常，异常信息为{0}",ex));
                return new CommonResult() { Message = string.Empty, State = 0 };
            }
         
        }

    }
}
