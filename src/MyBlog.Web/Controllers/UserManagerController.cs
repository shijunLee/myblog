using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Model.Command;
using MyBlog.DAL.Admin;
using MyBlog.Model.ViewModel.Admin;
using System.Security.Cryptography;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    public class UserManagerController : Controller
    {

        private readonly ICommandInvoker<ModifyUserCommand, CommonResult> _modifyUserInfoCommandInvorker;
        private readonly ICommandInvoker<GetUserCommand, UserInfoViewModel> _getUserInfoCommandInvorker;


        public UserManagerController(ICommandInvoker<ModifyUserCommand, CommonResult> modifyUserInfoCommandInvorker,
            ICommandInvoker<GetUserCommand, UserInfoViewModel> getUserInfoCommandInvorker)
        {
            _modifyUserInfoCommandInvorker = modifyUserInfoCommandInvorker;
            _getUserInfoCommandInvorker = getUserInfoCommandInvorker;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var result = _getUserInfoCommandInvorker.Execute(new GetUserCommand());
            return View(result);
        }

        [HttpPost]
        public IActionResult Index(int userId, string userName, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || password != confirmPassword)
            {
                var info = _getUserInfoCommandInvorker.Execute(new GetUserCommand());
                ViewBag.Message = "您的密码输入不正确";
                return View(info);
            }

            MD5 md5 = MD5.Create();
            var passwordBytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var passwordStr = BitConverter.ToString(passwordBytes).Replace("-", "");
            ModifyUserCommand command = new ModifyUserCommand();
            command.UserId = userId;
            command.UserName = userName;
            command.Password = passwordStr;
            var modifyInfo = _modifyUserInfoCommandInvorker.Execute(command);
            if (modifyInfo.State == 1)
            {
                ViewBag.Message = "更新用户信息成功";
            }
            else
            {
                ViewBag.Message = "更新用户信息失败";
            }
            var result = _getUserInfoCommandInvorker.Execute(new GetUserCommand());
            return View(result);
        }
    }
}
