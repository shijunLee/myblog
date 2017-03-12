using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.Command
{
    public class LoginCommand
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfrimPassword { get; set; }


        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

    }
}
