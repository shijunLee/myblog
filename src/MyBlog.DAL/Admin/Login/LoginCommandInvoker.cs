using MyBlog.Model.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using Dapper;
using MyBlog.Common.DapperTool;
using MyBlog.Model.DataModel;

namespace MyBlog.DAL.Admin.Login
{
    public class LoginCommandInvoker : ICommandInvoker<LoginCommand, CommonResult>
    {


        private readonly IDapperConnection _dapperConnection;

        public LoginCommandInvoker(IDapperConnection dapperConnection)
        {
            this._dapperConnection = dapperConnection;
        }

        public CommonResult Execute(LoginCommand command)
        {
            string sql = "select * from user_info where user_name=@user_name and password =@password";
            using (var conn = _dapperConnection.GetDataBaseConnection())
            {

             
                var result = conn.Query<UserInfo>(sql,new { user_name=command.UserName, password=command.Password });
               
                if (result.Count() > 0)
                {
                   
                    return new CommonResult()
                    {
                        State =1,
                        Message = "Success"
                    };
                }
                else
                {
                    return new CommonResult()
                    {
                        State = 0,
                        Message = "用户名或密码不正确"
                    };
                }
            }  
        }
    }
}
