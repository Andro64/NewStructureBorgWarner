using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Data;
using System.Diagnostics;
using ZstdSharp.Unsafe;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{

    public class MYSQL_DB
    {          
        private string connStr = string.Empty;            
        public MYSQL_DB(string _connStr)
        {
            connStr = _connStr;
        }       
        public  DataTable GetDataTable(string table, string columns, string condition = "1")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    MySqlCommand cmd;
                    MySqlDataAdapter rawData;
                    DataTable data = new DataTable();
                    try
                    {
                        conn.Open();
                        string query = $"SELECT {columns} from {table} WHERE {condition};";
                        cmd = new MySqlCommand(query, conn);
                        rawData = new MySqlDataAdapter(cmd);
                        rawData.Fill(data);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
        }
        public DataTable FindAll(string table)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    MySqlCommand cmd;
                    MySqlDataAdapter rawData;
                    DataTable data = new DataTable();
                    try
                    {
                        conn.Open();
                        string query = $"SELECT * from {table};";
                        cmd = new MySqlCommand(query, conn);
                        rawData = new MySqlDataAdapter(cmd);
                        rawData.Fill(data);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }

        }
        
        public int ExecuteIntScalarSP(string procedimiento, params MySqlParameter[] datos)
        {
            int returnv;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(procedimiento, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddRange(datos);
                            MySqlParameter returnValue = new MySqlParameter("ireturnvalue", MySqlDbType.Int32);
                            returnValue.Direction = ParameterDirection.ReturnValue;
                            cmd.Parameters.Add(returnValue);                            
                            conn.Open();
                            cmd.ExecuteScalar();
                            returnv = (int)returnValue.Value;                            
                        }
                    }
                    catch(MySqlException ex)
                    {
                        Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }           
            return returnv;
        }

        public void ExecuteNonQuerySP(string procedimiento, params MySqlParameter[] datos)
        {            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(procedimiento, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddRange(datos);                                                        
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            
        }
        public DataTable ExecuteSP(string procedimiento, params MySqlParameter[] datos)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(procedimiento, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddRange(datos);                           
                            conn.Open();
                            dt.Load(cmd.ExecuteReader());
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return dt;
        }
        public void Insert(MySqlConnection conn, string table, string columns, Object[] values)
        {
            var temp = conn.State.ToString();
            using (MySqlConnection conn2 = new MySqlConnection(connStr))
            {
                conn2.Open();
                string query = $"INSERT INTO {table} ({columns}) VALUES (";

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] is String)
                    {
                        query = i == 0 ? query + "\'" + values[i] + "\'" : query + "," + "\'" + values[i] + "\'";
                    }
                    else
                    {
                        query = i == 0 ? query + values[i] : query + "," + values[i];
                    }
                }

                query += ");";

                MySqlCommand cmd = new MySqlCommand(query, conn2);

                cmd.ExecuteNonQuery();
                conn2.Close();
            }


        }
    }
}