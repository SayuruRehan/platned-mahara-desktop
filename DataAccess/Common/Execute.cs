using PlatnedMahara.Classes;
using PlatnedMahara.DataAccess.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlatnedMahara.DataAccess.Common
{
    public class Execute
    {
        //-------------------Private varible---------------------
        private SqlCommand SqlCmd;
        private SqlDataAdapter SqlAdp;
        //-------------------Private methods---------------------

        #region DataSet
        /// <summary>
        /// Return DataSet for SelectQuery/SPs ~ASB~
        /// </summary>
        /// <param name="SelectQuery"></param>
        /// <returns>DataSet</returns>
        [Obsolete]
        private DataSet ReturnDataSet(string SelectQuery, CommandType cmdType)
        {
            DataSet ds;
            SqlConnection conn = SqlConn.OpenConnectiion();
            try
            {
                SqlCmd = new SqlCommand();
                ds = new DataSet();

                SqlCmd.Connection = conn;
                SqlCmd.CommandText = SelectQuery;
                SqlCmd.CommandType = cmdType;
                SqlAdp = new SqlDataAdapter(SqlCmd);
                SqlAdp.Fill(ds);

                return ds;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        /// <summary>
        /// Return DataSet for Select Query/SPs for Specified parameters ~ASB~
        /// </summary>
        /// <param name="SelectQuery"></param>
        /// <param name="_SqlParameter"></param>
        /// <returns></returns>
        [Obsolete]
        private DataSet ReturnDataSet(string SelectQuery, SqlParameter[] _SqlParameter, CommandType cmdType)
        {
            DataSet ds;
            SqlConnection conn = null;
            try
            {
                SqlCmd = new SqlCommand();
                ds = new DataSet();

                conn = SqlConn.OpenConnectiion();
                SqlCmd.Connection = conn;
                SqlCmd.CommandText = SelectQuery;
                SqlCmd.CommandType = cmdType;
                SqlCmd.CommandTimeout = 60;
                for (int i = 0; i < _SqlParameter.Length; i++)
                {
                    SqlCmd.Parameters.Add(_SqlParameter[i]);
                }

                SqlAdp = new SqlDataAdapter(SqlCmd);
                SqlAdp.Fill(ds);

                conn.Close();
                return ds;

            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        #endregion

        #region DataTable

        /// <summary>
        /// Return DataTable for SelectQuery ~ASB~
        /// </summary>
        /// <param name="SelectQuery"></param>
        /// <returns>DataTable</returns>
        [Obsolete]
        private DataTable ReturnDataTable(string SelectQuery, CommandType cmdType)
        {
            DataTable dt;
            SqlConnection conn = null;
            try
            {
                SqlCmd = new SqlCommand();
                dt = new DataTable();
                conn = SqlConn.OpenConnectiion();
                SqlCmd.Connection = conn;
                SqlCmd.CommandText = SelectQuery;
                SqlCmd.CommandType = cmdType;
                SqlAdp = new SqlDataAdapter(SqlCmd);
                SqlAdp.Fill(dt);
                conn.Close();

                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        /// <summary>
        /// Return DataTable for Select Query for Specified Sql Paraneters ~ASB~
        /// </summary>
        /// <param name="SelectQuery"></param>
        /// <param name="_SqlParameter"></param>
        /// <returns>DataTable</returns>
        [Obsolete]
        private DataTable ReturnDataTable(string SelectQuery, SqlParameter[] _SqlParameter, CommandType cmdType)
        {
            DataTable dt;
            SqlConnection conn = null;
            try
            {
                SqlCmd = new SqlCommand();
                dt = new DataTable();
                conn = SqlConn.OpenConnectiion();
                SqlCmd.Connection = conn;
                SqlCmd.CommandText = SelectQuery;
                SqlCmd.CommandType = cmdType;
                SqlCmd.CommandTimeout = 120;
                for (int i = 0; i < _SqlParameter.Length; i++)
                {
                    SqlCmd.Parameters.Add(_SqlParameter[i]);
                }

                SqlAdp = new SqlDataAdapter(SqlCmd);
                SqlAdp.Fill(dt);

                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        #endregion

        #region DataRow
        /// <summary>
        /// Return DataRow for SelectQuery ~ASB~
        /// </summary>
        /// <param name="SelectQuery"></param>
        /// <returns>DataRow</returns>
        [Obsolete]
        private DataRow ReturnDataRow(string SelectQuery, CommandType cmdType)
        {
            DataTable dt;
            SqlConnection conn = null;
            try
            {
                SqlCmd = new SqlCommand();
                dt = new DataTable();
                conn = SqlConn.OpenConnectiion();

                SqlCmd.Connection = conn;
                SqlCmd.CommandText = SelectQuery;
                SqlCmd.CommandType = cmdType;
                SqlAdp = new SqlDataAdapter(SqlCmd);
                SqlAdp.Fill(dt);

                return dt.Rows[0];
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        /// <summary>
        /// Return Datarow for Select Query ~ASB~ 
        /// </summary>
        /// <param name="SelectQuery"></param>
        /// <param name="_SqlParameter"></param>
        /// <returns>DataRow</returns>
        [Obsolete]
        private DataRow ReturnDataRow(string SelectQuery, SqlParameter[] _SqlParameter, CommandType cmdType)
        {

            DataTable dt;
            SqlConnection conn = null;
            try
            {
                SqlCmd = new SqlCommand();
                dt = new DataTable();
                conn = SqlConn.OpenConnectiion();

                SqlCmd.Connection = conn;
                SqlCmd.CommandText = SelectQuery;
                SqlCmd.CommandType = cmdType;
                SqlCmd.CommandTimeout = 60;
                for (int i = 0; i < _SqlParameter.Length; i++)
                {
                    SqlCmd.Parameters.Add(_SqlParameter[i]);
                }

                SqlAdp = new SqlDataAdapter(SqlCmd);
                SqlAdp.Fill(dt);
                if (dt.Rows.Count == 0)
                    return null;

                return dt.Rows[0];
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;

            }
        }

        #endregion

        //-------------------Public methods------------------------

        #region SqlParameters

        public static SqlParameter AddParameter(string Name, object Value)
        {
            SqlParameter Parm;
            try
            {
                Parm = new SqlParameter();
                Parm.ParameterName = Name;
                Parm.Value = Value;

                return Parm;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlParameter AddParameter(string Name, object Value, SqlDbType ParamType)
        {
            SqlParameter Parm;
            try
            {
                Parm = new SqlParameter();
                Parm.ParameterName = Name;
                Parm.Value = Value;
                //Parm.DbType = ParamType;

                return Parm;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Execute Select Queries / SPs

        /// <summary>
        /// Execute Select Queries ~ASB~
        /// </summary>
        /// <param name="SelectQuery"></param>
        /// <param name="ReturnType"></param>
        /// <returns>object</returns>
        [Obsolete]
        public object Executes(string SelectQuery, ReturnType ReturnType, CommandType cmdType)
        {
            try
            {
                Object objValue = new object();
                switch (ReturnType)
                {
                    case (ReturnType.DataTable):
                        {
                            objValue = ReturnDataTable(SelectQuery, cmdType);
                        }
                        break;

                    case (ReturnType.DataSet):
                        {
                            objValue = ReturnDataSet(SelectQuery, cmdType);
                        }
                        break;

                    case (ReturnType.DataRow):
                        {
                            objValue = ReturnDataRow(SelectQuery, cmdType);
                        }
                        break;
                }
                return objValue;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Execute Parameterized Select Queries/SPs ~ASB~
        /// </summary>
        /// <param name="SelectQuery"></param>
        /// <param name="ReturnType"></param>
        /// <param name="_SqlParameter"></param>
        /// <returns>object</returns>

        [Obsolete]
        public T ExecuteSp<T>(string ProcName, params SqlParameter[] Para)
        {
            SqlConnection con;
            using (con = new SqlConnection(SqlConn.GetConnection().ConnectionString))
            {
                con.Open();
                int x = 0;
                SqlCommand com = new SqlCommand();
                if (Para != null)
                {
                    com.Parameters.AddRange(Para);
                }
                com.CommandText = ProcName;
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                x = com.ExecuteNonQuery();
                //bool isSelect = true;
                //if (x < 0)
                //{
                //    isSelect = false;
                //}

                Type typeParameterType = typeof(T);
                DataTable t;
                DataSet ds;
                if (typeParameterType.Name == "DataTable")
                {
                    t = new DataTable();
                    da.Fill(t);
                    return (T)Convert.ChangeType(t, typeof(T));
                }
                else if (typeParameterType.Name == "DataSet")
                {
                    ds = new DataSet();
                    da.Fill(ds);
                    return (T)Convert.ChangeType(ds, typeof(T));
                }
                else if (typeParameterType.Name == "Int32")
                {
                    t = new DataTable();
                    da.Fill(t);
                    return (T)Convert.ChangeType(t.Rows[0][0], typeof(T));

                }
                else
                {
                    throw new Exception("Invalid T parameter ....");
                }
            }
        }

        [Obsolete]
        public List<T> ExecuteSpList<T>(string ProcName, params SqlParameter[] Para) where T : new()
        {
            SqlConnection con;
            DataTable datatable;
            DataSet ds = new DataSet();

            using (con = new SqlConnection(SqlConn.GetConnection().ConnectionString))
            {
                con.Open();
                int x = 0;
                SqlCommand com = new SqlCommand();
                if (Para != null)
                {
                    com.Parameters.AddRange(Para);
                }
                com.CommandText = ProcName;
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                x = com.ExecuteNonQuery();

                da.Fill(ds);

                List<T> list = new List<T>();

                datatable = ds.Tables[0];
                List<string> columnsNames = new List<string>();

                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);

                list = datatable.AsEnumerable().ToList().ConvertAll<T>(row => GetObject<T>(row, columnsNames));

                return list;

            }
        }

        private T GetObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower().Trim() == objProperty.Name.ToLower().Trim());

                    if (!string.IsNullOrEmpty(columnname))
                    {
                        if (objProperty.PropertyType.FullName.ToUpper().Contains("BYTE[]"))
                        {
                            byte[] a = (byte[])row[columnname];
                            objProperty.SetValue(obj, a);
                        }
                        else
                        {
                            value = row[columnname].ToString();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                                {
                                    value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                    objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                                }
                                else
                                {
                                    value = row[columnname].ToString().Replace("%", "");
                                    objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                                }
                            }
                        }

                    }
                }
                return obj;
            }
            catch { return obj; }
        }

        [Obsolete]
        public object Executes(string SelectQuery, ReturnType ReturnType, SqlParameter[] _SqlParameter, CommandType cmdType)
        {
            try
            {
                Object objValue = new object();
                switch (ReturnType)
                {
                    case (ReturnType.DataTable):
                        {
                            objValue = ReturnDataTable(SelectQuery, _SqlParameter, cmdType);
                        }
                        break;

                    case (ReturnType.DataSet):
                        {
                            objValue = ReturnDataSet(SelectQuery, _SqlParameter, cmdType);
                        }
                        break;

                    case (ReturnType.DataRow):
                        {
                            objValue = ReturnDataRow(SelectQuery, _SqlParameter, cmdType);
                        }
                        break;
                }
                return objValue;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Execute Identity

        /// <summary>
        /// Execute and Return Identity Value ~ASB~
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="_SqlParameter"></param>
        /// <returns>int</returns>
        [Obsolete]
        public int ExecuteIdentity(string cmdText, SqlParameter[] _SqlParameter, CommandType cmdType = CommandType.StoredProcedure)
        {
            int linkID = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(SqlConn.GetConnection().ConnectionString))
                {
                    con.Open();
                    SqlCmd = new SqlCommand();
                    SqlCmd.Connection = con;

                    for (int i = 0; i < _SqlParameter.Length; i++)
                        SqlCmd.Parameters.Add(_SqlParameter[i]);


                    if (cmdType == CommandType.StoredProcedure)
                    {
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.CommandText = cmdText;
                    }
                    else
                    {
                        cmdText += " SELECT CAST(scope_identity()as int)";
                        SqlCmd.CommandType = CommandType.Text;
                        SqlCmd.CommandText = cmdText;
                    }
                    linkID = (int)SqlCmd.ExecuteScalar();
                    return linkID;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Execute Aggregates

        /// <summary>
        /// Execute Aggregates Queries ~ASB~
        /// </summary>
        /// <param name="Query"></param>
        /// <returns>object</returns>
        [Obsolete]
        public object ExecuteAggregates(string Query)
        {
            object Value = 0;
            SqlCmd = new SqlCommand();
            SqlConnection conn = null;
            try
            {
                conn = SqlConn.OpenConnectiion();
                SqlCmd.Connection = conn;
                SqlCmd.CommandType = CommandType.Text;
                SqlCmd.CommandText = Query;
                Value = SqlCmd.ExecuteScalar();
                return Value;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        /// <summary>
        /// Execute Parameterized Aggregates Queries ~ASB~
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="_SqlParameter"></param>
        /// <returns></returns>
        [Obsolete]
        public object ExecuteAggregates(string Query, SqlParameter[] _SqlParameter)
        {
            SqlCmd = new SqlCommand();
            SqlConnection conn = null;
            try
            {
                conn = SqlConn.OpenConnectiion();
                SqlCmd.Connection = conn;
                SqlCmd.CommandType = CommandType.Text;
                SqlCmd.CommandText = Query;

                for (int i = 0; i < _SqlParameter.Length; i++)
                    SqlCmd.Parameters.Add(_SqlParameter[i]);

                object obj = SqlCmd.ExecuteScalar();
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        #endregion

        #region Execute

        /// <summary>
        /// Execute Insert/Delete/Update ~ASB~
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="_SqlParameter"></param>
        /// <returns>int</returns>
        [Obsolete]
        public int Executes(string cmdText, SqlParameter[] _SqlParameter, CommandType cmdType = CommandType.StoredProcedure)
        {
            int NoOfRowsEffected = 0;
            SqlCmd = new SqlCommand();
            SqlConnection conn = null;
            try
            {
                conn = SqlConn.OpenConnectiion();
                SqlCmd.Connection = conn;
                SqlCmd.CommandType = cmdType;
                SqlCmd.CommandText = cmdText;
                SqlCmd.CommandTimeout = 600;
                for (int i = 0; i < _SqlParameter.Length; i++)
                    SqlCmd.Parameters.Add(_SqlParameter[i]);
                NoOfRowsEffected = SqlCmd.ExecuteNonQuery();
                return NoOfRowsEffected;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        /// <summary>
        /// Execute Sps with Output Parameters ~ASB~
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parm"></param>
        [Obsolete]
        public void spExecuteOutParm(string cmdText, IDataParameter[] parm)
        {

            SqlCmd = new SqlCommand();
            SqlConnection conn = null;
            try
            {
                conn = SqlConn.OpenConnectiion();
                SqlCmd.Connection = conn;
                SqlCmd.CommandText = cmdText;
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.AddRange(parm);
                SqlCmd.ExecuteNonQuery();
                SqlCmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        /// <summary>
        /// Execute quary without  any parameters
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        [Obsolete]
        public int Executes(string cmdText, CommandType cmdType)
        {
            int NoOfRowsEffected = 0;
            SqlCmd = new SqlCommand();
            SqlConnection conn = null;
            try
            {
                conn = SqlConn.OpenConnectiion();
                SqlCmd.Connection = conn;
                SqlCmd.CommandType = cmdType;
                SqlCmd.CommandText = cmdText;
                NoOfRowsEffected = SqlCmd.ExecuteNonQuery();
                return NoOfRowsEffected;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                SqlCmd = null;
            }
        }

        #endregion

        //--------------------*************------------------------
    }
}
