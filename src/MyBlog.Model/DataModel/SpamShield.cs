using MyBlog.Common.DapperTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Model.DataModel
{
    public class SpamShield
    {
        
 
        /// <summary>
        /// tick
        /// </summary>
        [Column(Name = "tick")]
        public string Tick { get; set; }

        /// <summary>
        /// hash
        /// </summary>
        [Column(Name = "hash")]
        public string Hash { get; set; }

    }
}
