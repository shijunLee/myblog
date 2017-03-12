using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyBlog.DAL;
using MyBlog.Model.Command;
using MyBlog.DAL.Admin;
using MyBlog.Common.Tool;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using MyBlog.Model.config;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Security.Claims;
using MyBlog.Model.ViewModel.Admin;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    [Authorize] 
      
    public class AdminController : Controller
    {

        private readonly ICommandInvoker<LoginCommand, CommonResult> _command;
        private readonly IVerificationCodeTool _verificationCodeTool;
        private readonly IAntiforgery _antiforgery;
        private AdminUserConfig adminUserConfig;
        private readonly ICommandInvoker<IndexInfoCommand, CommonResult<IndexInfoViewModel>> _indexInfoCommand;


        private readonly ICommandInvoker<AdminUserInitCommand, CommonResult> _adminUserInitCommand;

        public AdminController(ICommandInvoker<LoginCommand, CommonResult> command,
            IVerificationCodeTool verificationCodeTool, IAntiforgery antiforgery,
            ICommandInvoker<AdminUserInitCommand, CommonResult> adminUserInitCommand,
            ICommandInvoker<IndexInfoCommand, CommonResult<IndexInfoViewModel>> indexInfoCommand,
            IOptions<AdminUserConfig> admin)
        {
            this._command = command;
            this._verificationCodeTool = verificationCodeTool;
            this._antiforgery = antiforgery;
            this._adminUserInitCommand = adminUserInitCommand;
            this.adminUserConfig = admin.Value;
            this._indexInfoCommand = indexInfoCommand;
        }

        public IActionResult IndexInfo()
        {
            IndexInfoViewModel model = new IndexInfoViewModel();
            var infos = _indexInfoCommand.Execute(new IndexInfoCommand());
            if (infos.State == 0)
            {
                model.BlogCount = 0;
                model.TagCount = 0;
            }
            else
            {
                model = infos.Value;
            }
            return View(model);
        }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            //RouteData
            ViewBag.ControllerName = "起始页";
            ViewBag.ActionName = "信息";
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Login()
        {

            MD5 md5 = MD5.Create();
            var password = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(adminUserConfig.Password));
            var passwordStr = BitConverter.ToString(password).Replace("-", "");
            this._adminUserInitCommand.Execute(new AdminUserInitCommand() { EMail = adminUserConfig.EMail, Password = passwordStr, UserName = adminUserConfig.UserName });
            return View();
        }

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string returnUrl,string userName,string password,string verifyCode)
        {
            MD5 md5 = MD5.Create();
            var passwordBytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var passwordStr = BitConverter.ToString(passwordBytes).Replace("-", "");
            // this._adminUserInitCommand.Execute(new AdminUserInitCommand() { EMail = adminUserConfig.EMail, Password = passwordStr, UserName = adminUserConfig.UserName });
            string sessionId = this.HttpContext.Session.Id;
            var verfiyResult = _verificationCodeTool.VerfiyVerificationCode(verifyCode, sessionId);
            if (verfiyResult)
            {
                var result = _command.Execute(new LoginCommand() { Password = passwordStr, UserName = userName });
                if (result.State == 1)
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,userName),

                        };
                    var id = new ClaimsIdentity(claims, "CustomApiKeyAuth");

                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
                    claimsPrincipal.AddIdentity(id);
                    HttpContext.Authentication.SignInAsync("MyCookieMiddlewareInstance", claimsPrincipal);
                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    { 
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Message = "帐号密码不正确！";
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetVerifyImage()
        {
            this.HttpContext.Session.Set("key", System.Text.Encoding.UTF8.GetBytes("0"));
            string id = this.HttpContext.Session.Id;
            var aaa = this.HttpContext.Connection.RemoteIpAddress;
            var image = _verificationCodeTool.GetVerfiyVerificationImage(id, 5);
            return File(image, "image/gif");

        }
    }
}
