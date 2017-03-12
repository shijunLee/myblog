using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Model.Command;
using MyBlog.Model.ViewModel.Admin;
using Microsoft.Extensions.Logging;
using MyBlog.Common.DapperTool;
using Dapper;
using MyBlog.Model.DataModel;

namespace MyBlog.DAL.Admin.UserManager
{
    public class GetUserInfoCommandInvorker : ICommandInvoker<GetUserCommand, UserInfoViewModel>
    {
        private readonly ILogger<GetUserInfoCommandInvorker> _logger;
        private readonly IDapperConnection _dapperConnection;


        public GetUserInfoCommandInvorker(ILogger<GetUserInfoCommandInvorker> logger, IDapperConnection dapperConnection)
        {
            _logger = logger;
            _dapperConnection = dapperConnection;
        }

        public UserInfoViewModel Execute(GetUserCommand command)
        {

            const string userInfoSql = "SELECT u.user_id,u.user_name,u.password from user_info u ";
            try
            {
                using (var conn = _dapperConnection.GetDataBaseConnection())
                {
                    var userInfo = conn.Query<UserInfo>(userInfoSql).FirstOrDefault();
                    if (userInfo == null)
                    {
                        return null;
                    }
                    else
                    {
                        UserInfoViewModel model = new UserInfoViewModel();
                        model.UserId = userInfo.UserId;
                        model.UserName = userInfo.UserName;
                        model.Password = userInfo.Password;
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("获取用户信息出现异常，异常信息为{0}",ex));
                return null;
            } 
        }
    }
}
