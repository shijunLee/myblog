using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyBlog.Common.Tool
{
    public class NetWorkTool:INetWorkTool
    {
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        public NetWorkTool(IHttpContextAccessor httpContextAccessor)
        {
            this._IHttpContextAccessor = httpContextAccessor;
        }

        public string GetRemoteIp()
        {
            var result =   _IHttpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            return   result.ToString();
        }
    }
}
