using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Mono.System.Drawing;

namespace MyBlog.Common.Tool
{
    public   interface IVerificationCodeTool
    {
        bool VerfiyVerificationCode(string code,string sessionId);

        byte[] GetVerfiyVerificationImage(string sessionId, int length);
    }
}
