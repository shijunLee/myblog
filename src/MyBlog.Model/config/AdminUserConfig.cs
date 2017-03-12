using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.config
{
    public class AdminUserConfig
    {
        /// <summary>
        /// 管理员用户名配置
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 管理员密码配置
        /// </summary>
        public string Password { get; set; }

        public string EMail { get; set; }
    }
}
