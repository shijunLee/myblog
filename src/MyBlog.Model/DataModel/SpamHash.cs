using MyBlog.Common.DapperTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.DataModel
{
    public class SpamHash
    {
        //spam_hash_id
        /// <summary>
        /// 主键
        /// </summary>
        [Column(Name = "spam_hash_id")]
        public string SpamHashId { get; set; }
        //hash
        /// <summary>
        /// hash
        /// </summary>
        [Column(Name = "hash")]
        public string Hash { get; set; }
        //pass

        
        [Column(Name = "pass")]
        public int Pass { get; set; }
         
        /// <summary>
        /// 博客主键
        /// </summary>
        [Column(Name = "post_key")]
        public string PostKey { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Name = "created_time")]
        public DateTime CreatedTime { get; set; }
    }
}
