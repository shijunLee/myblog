using MyBlog.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mono.System.Drawing;
using System.Text;
using Mono.System.Drawing.Drawing2D;
using Microsoft.Extensions.Logging;

namespace MyBlog.Common.Tool
{
    public class VerificationCodeTool : IVerificationCodeTool
    {
        private readonly ICache cacheManager;

        private readonly ILogger<VerificationCodeTool> logger;
        /// <summary>
        /// 依赖注入函数
        /// </summary>
        /// <param name="cache"></param>
        public VerificationCodeTool(ICache cache, ILogger<VerificationCodeTool> _logger)
        {
            this.cacheManager = cache;
            this.logger = _logger;
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <param name="sessionId">当前用户session</param>
        /// <param name="length">生成验证码长度</param>
        /// <returns></returns>
        public byte[] GetVerfiyVerificationImage(string sessionId,int length=4)
        {
            char[] chr = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
            Random r = new Random(Guid.NewGuid().GetHashCode());
            StringBuilder strbuilder = new StringBuilder();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                var strIndex = r.Next(1, 60);
                strbuilder.Append(chr[strIndex - 1]);
            }
            str = strbuilder.ToString();
            cacheManager.Add(sessionId,str,5000);
            int iWordWidth = 15;
            int iImageWidth = str.Length * iWordWidth;
            Bitmap image = new Bitmap(iImageWidth, 20);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器 
                Random random = new Random();
                //清空图片背景色 
                g.Clear(Color.White);

                //画图片的背景噪音点
                for (int i = 0; i < 20; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                //画图片的背景噪音线 
                for (int i = 0; i < 2; i++)
                {
                    int x1 = 0;
                    int x2 = image.Width;
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    if (i == 0)
                    {
                        g.DrawLine(new Pen(Color.Gray, 2), x1, y1, x2, y2);
                    }

                }


                for (int i = 0; i < str.Length; i++)
                {

                    string Code = str[i].ToString();
                    int xLeft = iWordWidth * (i);
                    random = new Random(xLeft);
                    int iSeed = DateTime.Now.Millisecond;
                    int iValue = random.Next(iSeed) % 4;
                    if (iValue == 0)
                    {
                        Font font = new Font("Arial", 13, (FontStyle.Bold | Mono.System.Drawing.FontStyle.Italic));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Red, 1.5f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 1)
                    {
                        Font font = new Mono.System.Drawing.Font("楷体", 13, (FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.DarkRed, 1.3f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 2)
                    {
                        Font font = new Mono.System.Drawing.Font("宋体", 13, (Mono.System.Drawing.FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Green, Color.Blue, 1.2f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 3)
                    {
                        Font font = new Mono.System.Drawing.Font("黑体", 13, (Mono.System.Drawing.FontStyle.Bold | Mono.System.Drawing.FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Green, 1.8f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                }
                //////画图片的前景噪音点 
                //for (int i = 0; i < 8; i++)
                //{
                //    int x = random.Next(image.Width);
                //    int y = random.Next(image.Height);
                //    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                //}
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(); 
                image.Save(ms, Mono.System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] bytes = ms.ToArray();
                ms.Dispose();
                return bytes;
            } catch (Exception ex)
            {
                
                logger.LogError(String.Format("进行图片初始化失败,异常信息为{0}",ex));
                return null;
            }
            finally
            {
                g.Dispose();
                image.Dispose();

            } 
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool VerfiyVerificationCode(string code, string sessionId)
        {
            // cacheManager.Add(sessionId, str);
            var str = cacheManager.Get<string>(sessionId);
            if (!string.IsNullOrWhiteSpace(str))
            {
                if (code.ToLower() == str.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
