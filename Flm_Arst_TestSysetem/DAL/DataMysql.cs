using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using Flm_Arst_TestSysetem.ViewModel;
using Flm_Arst_TestSysetem.Base;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flm_Arst_TestSysetem.DAL
{


    
     
        public static class DataMysql


        {
            static string strConn = "Database=zuhuoqi;Data Source=127.0.0.1;User Id=root;Password=119120;pooling=false;CharSet=utf8;port=3306;AllowLoadLocalInfile=true";//
        static string infile = "SET GLOBAL local_infile=1";
        static string Read_maxID = "select row_id+1 MAX , Test_name  from  fr_record ORDER BY ROW_ID DESC LIMIT 1";
        public static string Test_name = string.Empty;
       
        public static bool ExcuteSQL(string SQL)
            {



                MySqlConnection sqlConn = new MySqlConnection(strConn);
                MySqlCommand command = new MySqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = sqlConn;
                command.CommandText = SQL;

            try
            {
                if (sqlConn == null)
                { return false; }
                sqlConn.Open();
                command.ExecuteNonQueryAsync();
                return true;
            }
            catch(Exception e)
            {
                //this.message = ex.Message;
                return false;
            }
           
            finally
            {
                if (sqlConn != null)
                {
                    try {
                        sqlConn.Close();
                        sqlConn.Dispose();
                    }
                    catch(Exception e)
                    { }
                    
                }
            }
            }
        public static int deleteSQL(string SQL)
        {



            MySqlConnection sqlConn = new MySqlConnection(strConn);
            MySqlCommand command = new MySqlCommand();
            command.CommandType = CommandType.Text;
            command.Connection = sqlConn;
            command.CommandText = SQL;
           
            try
            {
                if (sqlConn == null)
                { return 0; }
                sqlConn.Open();
                return command.ExecuteNonQuery();
              //   true;
            }
            catch (Exception e)
            {
                //this.message = ex.Message;
                return 0;
            }

            finally
            {
                if (sqlConn != null)
                {
                    
                        sqlConn.Close();
                       // sqlConn.Dispose();
                   

                }
            }
        }

        private static void SqlBulkCopyByDatatable(string connectionString, string TableName, DataTable dt)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
                    {
                        try
                        {
                            sqlbulkcopy.DestinationTableName = TableName;
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                            }
                            sqlbulkcopy.WriteToServer(dt);
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
      //  SET GLOBAL local_infile=1
//SHOW VARIABLES LIKE '%local%';
       // const string ConnectionString = "server=localhost;port=3306;user=root;password=123456;database=mysql;SslMode = none;AllowLoadLocalInfile=true";
        public static int BulkInsert<T>(List<T> entities, string tableName)
        {   string disablekeys = "alter table "+tableName+" disable keys";
            string enablekeys = "alter table " + tableName + " enable keys"; //alter table table_name enable keys
            ExcuteSQL(infile);
            ExcuteSQL(disablekeys);
            DataTable dt = entities.ToDataTable();
            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = strConn;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                if (tableName.IsNullOrEmpty())
                {
                    var tableAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
                    if (tableAttribute != null)
                        tableName = ((TableAttribute)tableAttribute).Name;
                    else
                        tableName = typeof(T).Name;
                }

                int insertCount = 0;
                string tmpPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid().ToString() + ".tmp");
                string csv = dt.ToCsvStr();
                File.WriteAllText(tmpPath, csv, Encoding.UTF8);

                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    
                       MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tmpPath,
                        Local = true,
                        NumberOfLinesToSkip = 0,
                        TableName = tableName,
                        CharacterSet = "utf8"
                         
                    };
                    try
                    {
                        bulk.Columns.AddRange(dt.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
                        insertCount = bulk.Load();
                        tran.Commit();
                      
                    }
                    catch (MySqlException ex)
                    {
                        if (tran != null)
                            tran.Rollback();
                        ExcuteSQL(enablekeys);
                     //   throw ex;
                    }
                }
                ExcuteSQL(enablekeys);
                File.Delete(tmpPath);
                return insertCount;
            }

        }
        public static bool IsNullable(Type t)
            {
                return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
            }

        
            public static Type GetCoreType(Type t)
            {
                if (t != null && IsNullable(t))
                {
                    if (!t.IsValueType)
                    {
                        return t;
                    }
                    else
                    {
                        return Nullable.GetUnderlyingType(t);
                    }
                }
                else
                {
                    return t;
                }
            }
            //public static void Dispose()
            //{
            //    if (sqlConn != null && sqlConn.State == ConnectionState.Open)
            //    {
            //        sqlConn.Close();
            //    }
            //    sqlConn.Dispose();
            //    command.Dispose();
            //}

            public static DataTable QuerySQL(string SQL)
            {
               
                MySqlConnection sqlConn = new MySqlConnection(strConn);
              
                DataTable dataTable = new DataTable();
                try
                {
                    sqlConn.Open();
                    new MySqlDataAdapter(SQL, sqlConn).Fill(dataTable);
                  
                }
                catch
                {
                dataTable = null;
                }
              
                if (sqlConn != null)
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                }
             
                return dataTable;
            }
        /// <summary>
        /// 获取试验编号
        /// </summary>
        /// <returns></returns>
        public static string Get_test_name()
        {
            try
            {
                DataTable dt = QuerySQL(Read_maxID);
                if (dt.Rows.Count < 1)
                {
                    return "";
                }
                Test_name = DateTime.Now.ToString("yyyy:MM:dd:HH:mm") +"_No."+ (Convert.ToInt32( dt.Rows[0]["Test_name"].ToString().Split('.')[1])+1).ToString();
                return Test_name;
            }
            catch (Exception E)
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("ErrorCode #", "Error" + E.ToString(), LogType.Fault);
                return "";
            }

        }
        
     
        
        
        public static void Insert_record(int index,double rate,string tst_name,string media_name,string users_name)

        {

             int idx = 0;
            Globalvariable.systemMonitorViewModel.data_count = 0;
            while (Globalvariable.systemMonitorViewModel.CurrentDevice.IsRecord)
            {

                while ( Globalvariable.systemMonitorViewModel.Min+100>= index)
                {
                    Thread.Sleep(0);
                    try
                    {
                        idx++;
                     
                            string val = Math.Round((double)SystemMonitorViewModel.LineSeries1.Values[(int)index], 3).ToString() + "#" +
                                  Math.Round((double)SystemMonitorViewModel.LineSeries2.Values[(int)index], 3).ToString() + "#" +
                                  Math.Round((double)SystemMonitorViewModel.LineSeries3.Values[(int)index], 3).ToString() + "#" +
                                  Math.Round((double)SystemMonitorViewModel.LineSeries4.Values[(int)index], 3).ToString() + "#" +
                                  Math.Round((double)SystemMonitorViewModel.LineSeries5.Values[(int)index], 3).ToString() + "#" +
                                   Math.Round((double)SystemMonitorViewModel.LineSeries6.Values[(int)index], 3).ToString() + "#" +
                                     Math.Round((double)SystemMonitorViewModel.LineSeries7.Values[(int)index], 3).ToString() + "#" +
                                    Math.Round((double)SystemMonitorViewModel.LineSeries8.Values[(int)index], 3).ToString() + "#" +
                                     Math.Round((double)SystemMonitorViewModel.pressure1.Values[(int)index], 3).ToString() + "#" +
                                      Math.Round((double)SystemMonitorViewModel.pressure2.Values[(int)index], 3).ToString();
                            index++;
                        Globalvariable.systemMonitorViewModel.data_count = index;
                            string sql = "INSERT into fr_record(Test_index, Test_name, senors, Vals, Clock_Rate,media_name,Test_users,Create_time) VALUES(" + idx + ",'" + tst_name + "','z1-z8,p1-p2','" + val + "','" + rate + "','" + media_name + "','" + users_name + "','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')";
                        new Task(() => { ExcuteSQL(sql); }).Start();

                      
                        if (SystemMonitorViewModel.LineSeries1.Values.Count < 1)
                        {
                            continue;
                        }
                    }

                    catch (Exception e)
                    {
                        Globalvariable.systemMonitorViewModel.LogInfoAdd("ErrorCode #", "Error" + e.ToString(), LogType.Fault);
                        break;
                    }
                }
                if (Globalvariable.systemMonitorViewModel.Min + 100 > 1)
                {
                    Globalvariable.industrialBLL.Insert_result(tst_name);
                }
            }
        
        }

       //   public static 
        

        }


    public static class ExtentionHelper
    {
        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 将Json字符串转为DataTable
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this string jsonStr)
        {
            return jsonStr == null ? null : JsonConvert.DeserializeObject<DataTable>(jsonStr);
        }
        /// <summary>
        /// 将IEnumerable'T'转为对应的DataTable
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> iEnumberable)
        {
            return iEnumberable.ToJson().ToDataTable();
        }
        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else
            {
                string objStr = obj.ToString();
                return string.IsNullOrEmpty(objStr);
            }
        }

        /// <summary>
        ///将DataTable转换为标准的CSV字符串
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>返回标准的CSV</returns>
        public static string ToCsvStr(this DataTable dt)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colum = dt.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}



