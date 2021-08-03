using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceClaimsHandlingApp.DataContext
{
    public static class SQLConnectionHelper
    {
        public static SqlConnection Connection { get; set; }

        public static SqlConnection GetSQLConnection()
        {
            Connection = new SqlConnection("Server=interview-testing-server.database.windows.net; Database=Interview; User Id=TestLogin; Password=5D9ej2G64s3sd^;");

            try
            {
                Connection.Open();
                Connection.Close();
                return Connection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
