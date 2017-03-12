using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
//using System.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace MyBlog.Common.DapperTool
{
    public class DapperConnection: IDapperConnection
    {

        private string connectionString;

        public DapperConnection(IOptions<ConnectionStrings> dbOptions)
        {
            this.connectionString = dbOptions.Value.SqliteConnection;
        }

        public   IDbConnection GetDataBaseConnection()
        {
            IDbConnection conn = new SqliteConnection(connectionString);
            return conn;
        }

      
    }
}
