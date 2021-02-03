using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace StorAge
{
    public class GetWeb
    {
        public class GetSum
        {
            public string web { get; set; }//web文件
            public double sum1 { get; set; }//数量汇总
            public double sum2 { get; set; }//参考数量汇总
        }

        /// <param name="site">据点</param>
        /// <param name="type">cct 车贴
        ///                    cdt 单透
        ///                    cpp PPF
        ///                    cwg 外购
        ///                    
        ///                    bbj 背胶大卷
        ///                    bdt 单透大卷
        ///                    bgz 硅纸
        ///                    bsz 素纸
        ///                    
        ///                    yyz 原纸
        ///                    yhg 化工
        ///                    yjs 胶水
        ///                    ymw 膜 外购
        ///                    ymz 膜 自产
        /// </param>
        /// <param name="dayType">
        ///              a 1-15天
        ///              b 16-30天
        ///              c 31-90天
        ///              d 91-180天
        ///              e >180
        ///              f 其他(无条码日期)
        /// </param>
        public static GetSum WebMX(string site, string type, string dayType,int pageIndex)
        {
            GetSum gs = new GetSum();
            gs.web = "";
            gs.sum1 = 0;
            gs.sum2 = 0;
            string sql_ct = StorAge.GetSql.MXsql(site,type,dayType,pageIndex);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);

            foreach (DataRow dr in da.Rows)
            {
                gs.web += "<tr>";
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    if (i == 10 || i == 12 || i == 13)
                    {
                        gs.web += "<td style = \"text-align: right;\">" + String.Format("{0:N2}", dr[i]) + "</td>";
                    }
                    else
                    {
                        gs.web += "<td>" + dr[i].ToString() + "</td>";
                    }
                }
                gs.sum1 += Convert.ToDouble(dr["INAG008"].ToString());
                gs.sum2 += Convert.ToDouble(dr["INAG025"].ToString());
                gs.web += "</tr>";
            }
            gs.web += "<tr>";
            gs.web += "<td colspan=\"9\">总数量:</td><td colspan=\"2\">" + String.Format("{0:N2}", gs.sum1) + "</td>";
            gs.web += "<td colspan=\"2\">参考总数量:</td><td colspan=\"2\">" + String.Format("{0:N2}", gs.sum2) + "</td>";
            gs.web += "</tr>";
            return gs;
        }

        public static double[] Web03Left(string type)
        {
            double[] num = new double[2];
            string sql_ct = StorAge.GetSql.Web03left(type);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);

            foreach (DataRow dr in da.Rows)
            {
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    num[i] = Convert.ToDouble(dr[i].ToString());
                }
            }
            return num;
        }

        public static double[] Web03Right(string type)
        {
            double[] num = new double[6];
            string sql_ct = StorAge.GetSql.Web03Right(type);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);

            foreach (DataRow dr in da.Rows)
            {
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    num[i] = Convert.ToDouble(dr[i].ToString());
                }
            }

            return num;
        }
        // 成品-LEFT
        public static double[] CPLeft(string site)
        {
            double[] num = new double[4];
            string sql_ct = StorAge.GetSql.CPLeft(site);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);

            foreach (DataRow dr in da.Rows)
            {
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    num[i] = Convert.ToDouble(dr[i].ToString());
                }
            }
            return num;
        }
        //成品RIGHT
        public static double[] CPRight(string site)
        {
            double[] num = new double[6];
            string sql_ct = StorAge.GetSql.CPRight(site);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);

            foreach (DataRow dr in da.Rows)
            {
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    num[i] = Convert.ToDouble(dr[i].ToString());
                }
            }
            return num;
        }
        //半成品LEFT
        public static double[] BCPLeft(string site)
        {
            double[] num = new double[4];
            string sql_ct = StorAge.GetSql.BCPLeft(site);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);

            foreach (DataRow dr in da.Rows)
            {
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    num[i] = Convert.ToDouble(dr[i].ToString());
                }
            }
            return num;
        }
        //半成品RIGHT
        public static double[] BCPRight(string site)
        {
            double[] num = new double[6];
            string sql_ct = StorAge.GetSql.BCPRight(site);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);

            foreach (DataRow dr in da.Rows)
            {
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    num[i] = Convert.ToDouble(dr[i].ToString());
                }
            }
            return num;
        }


        //原料LEFT
        public static double[] YLLeft(string site)
        {
            double[] num = new double[5];
            string sql_ct = StorAge.GetSql.YLLeft(site);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);

            foreach (DataRow dr in da.Rows)
            {
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    num[i] = Convert.ToDouble(dr[i].ToString());
                }
            }
            return num;
        }
        //原料RIGHT
        public static double[] YLRight(string site)
        {
            double[] num = new double[6];
            string sql_ct = StorAge.GetSql.YLRight(site);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);

            foreach (DataRow dr in da.Rows)
            {
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    num[i] = Convert.ToDouble(dr[i].ToString());
                }
            }
            return num;
        }

        //分页专用
        public static int[] GetFY(string site, string type, string dayType)
        {
            int[] pageCount = new int[1];
            string sql_ct = StorAge.GetSql.Web03FY(site, type, dayType);
            DataTable da = StorAge.OracleConfig.queryDataTable(sql_ct);
            foreach (DataRow dr in da.Rows)
            {
                for (int i = 0; i < da.Columns.Count; i++)
                {
                    pageCount[i] = Convert.ToInt32(dr[i].ToString());
                }
            }
            return pageCount;
        }
    }
}