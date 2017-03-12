﻿using Pinyin4net;
using Pinyin4net.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBlog.Common.Extensions
{
    public static class StringExtensions
    {
        private static readonly HanyuPinyinOutputFormat format;

        static StringExtensions()
        {
            format = new HanyuPinyinOutputFormat();
            format.ToneType = HanyuPinyinToneType.WITHOUT_TONE;
            format.VCharType = HanyuPinyinVCharType.WITH_V;
            format.CaseType = HanyuPinyinCaseType.LOWERCASE;
        }

        public static bool IsNullOrWhitespace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static string FormatWith(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        public static string GetMd5Hash(this string value) {

            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            var byteData = System.Text.Encoding.UTF8.GetBytes(value);
            using (var md5 = MD5.Create())
            {
                byteData = md5.ComputeHash(byteData);
            }
            var ret = new StringBuilder();
            foreach (var item in byteData)
            {
                ret.Append(item.ToString("x2").ToLowerInvariant());
            }
            return ret.ToString();
        }

        public static string ToSlug(this string value)
        {
            value = value.ToLowerInvariant();

            value = ConvertChineseToPY(value);

            value = value.Replace("#", "-sharp ").Replace("@", "-at ")
                         .Replace("$", "-dollar ").Replace("%", "-percent ")
                         .Replace("&", "-and ").Replace("||", "-or ");

            value = Regex.Replace(value.TrimEnd(), @"\s", "-", RegexOptions.Compiled);

            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            value = value.Trim('-', '_');

            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        private static string ConvertChineseToPY(string value)
        {
            return Regex.Replace(value, "[\u4e00-\u9fa5]", (m) => string.Format(" {0} ", m.Value.ChsToPinYin()));
        }

        #region 汉字转拼音

        /// <summary>
        /// 简体中文转拼音
        /// </summary>
        /// <param name="chs">简体中文字</param>
        /// <returns>拼音</returns>
        private static string ChsToPinYin(this string chs)
        {
            var myRegex = new Regex("^[\u4e00-\u9fa5]$");
            var returnstr = "";
            var nowchar = chs.ToCharArray();
            for (var j = 0; j < nowchar.Length; j++)
            {
                if (myRegex.IsMatch(nowchar[j].ToString()))
                {
                    var pingStrs = PinyinHelper.ToHanyuPinyinStringArray(nowchar[j], format);
                    if (pingStrs.Any())
                    {
                        returnstr += pingStrs[0];
                    }
                    else
                        returnstr += nowchar[j].ToString();
                }
                else
                {
                    returnstr += nowchar[j].ToString();
                }
            }
            return returnstr;
        }

        #endregion 汉字转拼音
    }
}
