using MyBlog.Common.DapperTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.DataModel
{
    public class UserInfo
    {
        //        user_id
      
        [Column(Name = "user_id")]
        public int UserId { get; set; }
        //user_name

        [Column(Name = "user_name")]
        public string UserName { get; set; }
        //user_email
        [Column(Name = "user_email")]
        public string UserEmail { get; set; }
        //user_phone

        [Column(Name = "user_phone")]
        public string UserPhone { get; set; }
        
        //user_sign
        [Column(Name = "user_sign")]
        public string UserSign { get; set; }
        //user_photo
        [Column(Name = "user_photo")]
        public string UserPhoto { get; set; }
        //user_nick_name
        [Column(Name = "user_nick_name")]
        public string UserNickName { get; set; }

        [Column(Name = "password")]
        public string Password { get; set; }


    }
}
