using System;
using System.Collections.Generic;
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
            //string conS = @"Server=PL-MYOB-001;Database=PG1305NZ;Integrated Security=True;user=sa;pwd=sa123SA;Encrypt=False;";
            //string conS = @"Server=172.28.47.227\SQLEXPRESS;Database=platnedpass;Integrated Security=True;user=platnedpassuser;pwd=g91uiYMbcf+u@QF{;Encrypt=False;Connect Timeout=1000;";
            //SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings.Get("ConnStr").ToString());
            string conS = @"Server=172.28.47.227\SQLEXPRESS;Database=platnedpass;User Id=platnedpassuser;Password=g91uiYMbcf+u@QF{;Encrypt=False;Connect Timeout=1000;";
            SqlConnection con = new SqlConnection(conS);
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
            string conS = @"Server=PL-MYOB-001;Database=PG1305NZ;Integrated Security=True;user=sa;pwd=sa123SA;Encrypt=False;";
            //string conS = @"Server=172.28.47.227\SQLEXPRESS;Database=platnedpass;User Id=platnedpassuser;Password=g91uiYMbcf+u@QF{;Encrypt=False;Connect Timeout=1000;";
            SqlConnection con = new SqlConnection(conS);
            return con;
        }
    }
}
