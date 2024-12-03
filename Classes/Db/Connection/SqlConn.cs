using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatnedMahara.DataAccess.Connection
{
    public class SqlConn
    {
        [Obsolete]
        public static SqlConnection OpenConnectiion()
        {            
            SqlConnection con = GetConnection();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            else
                con.Close();
            return con;
        }

        [Obsolete]
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PlatnedMaharaDbDev"]?.ConnectionString;
            //string conS = @"Server=172.28.47.227\SQLEXPRESS;Database=platnedpass;User Id=platnedpassuser;Password=g91uiYMbcf+u@QF{;Encrypt=False;Connect Timeout=1000;";
            SqlConnection con = new SqlConnection(connectionString);
            return con;
        }
    }
}
