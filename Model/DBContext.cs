using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DBContext
    {
        public SqlConnection GetConnection()
        {
            var appSettings = new AppConfiguration();
            {
                return new SqlConnection(appSettings.ConnectionString);
            }
        }
    }
}
