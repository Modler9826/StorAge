using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace StorAge
{
    /// <summary>
    /// Oracle数据库配置类
    /// </summary>
    public class OracleConfig
    {
        static string connStr = @"Data Source=192.168.2.55/topprd;User Id=dsdata;Password=dsdata;";
        public static DataTable queryDataTable(string sql)
        {
            string connStrd = connStr;
            DataSet ds = new DataSet();
            using (OracleConnection conn = new OracleConnection(connStrd))
            {

                OracleDataAdapter da = new OracleDataAdapter(sql, conn);
                da.Fill(ds);
            }
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        } //取数据函数

        public static string tjson(DataTable dt)
        {
            string str_json = "";

            if (dt.Rows.Count > 0)
            {
                str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);

            }
            else
            {
                str_json = "{\"name\":\"错误\"}";

            }
            // str_json = str_json.Replace("[", "");
            //str_json = str_json.Replace("]", "");
            return str_json;

        } //转换数据库为json格式dll封装


        public class itemcb
        {

            public string zt { get; set; }

            public string jd { get; set; }



        }
    }
}