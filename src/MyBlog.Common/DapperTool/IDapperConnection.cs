using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace MyBlog.Common.DapperTool
{
    public interface IDapperConnection
    {
          IDbConnection GetDataBaseConnection();
    }
}
