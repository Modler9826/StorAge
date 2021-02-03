using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StorAge
{
    public partial class Web03MX : System.Web.UI.Page
    {
        public static StringBuilder sbWeb = new StringBuilder();
        public int pageIndex = 1 ;
        public int pageCount;
        string site ;
        string type;
        string dayType;
        protected void Page_Load(object sender, EventArgs e)
        {
            site = Request.QueryString["asite"].ToString();
            type = Request.QueryString["type"].ToString();
            dayType = Request.QueryString["dayType"].ToString();
            pageCount = StorAge.GetWeb.GetFY(site, type, dayType)[0];
            LMessage.Text = "当前第" + pageIndex + "页,共" + pageCount + "页";
            WebShow();
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
        ///                    ymy 膜 外购
        ///                    ymz 膜 自产
        /// </param>
        /// <param name="dayType">
        ///              a 1-15天
        ///              b 16-30天
        ///              c 31-90天
        ///              d 91-180天
        ///              e >180
        /// </param>
        public void WebShow()
        {
            string title = "";
            if (site != "ALL")
            {
                title = "(" + site + ")";
            }
            switch (type)
            {
                case "cp":
                    title += "成品:";
                    break;
                case "cct":
                    title += "成品-车贴:";
                    break;
                case "cdt":
                    title += "成品-单透:";
                    break;
                case "cpp":
                    title += "成品-PPF:";
                    break;
                case "cwg":
                    title += "成品-外购:";
                    break;
                case "bcp":
                    title += "半成品:";
                    break;
                case "bbj":
                    title += "半成品:背胶大卷";
                    break;
                case "bdt":
                    title += "半成品-单透大卷:";
                    break;
                case "bgz":
                    title += "半成品-硅纸:";
                    break;
                case "bsz":
                    title = "半成品-塑纸:";
                    break;
                case "yl":
                    title += "原料:";
                    break;
                case "yyz":
                    title += "原料-原纸:";
                    break;
                case "yhg":
                    title += "原料-化工:";
                    break;
                case "yjs":
                    title += "原料-胶水:";
                    break;
                case "ymz":
                    title += "原料-膜(自产):";
                    break;
                case "ymw":
                    title += "原料-膜(外购):";
                    break;
                default:
                    title += "";
                    break;

            }
            
            sbWeb.Clear();
            sbWeb.Append("<div class=\"title\"> <h4><a href = \"Web03.aspx\">"+title+ "库龄报表明细</a></h4></div>");
            sbWeb.Append("<div id=\"bo_box\">");
            sbWeb.Append("<table border=\"1\" id=\"ab\">");
            sbWeb.Append("<tr class=\"head\"> ");
            sbWeb.Append("<td>项次</td><td>据点</td><td>仓库编号</td><td>仓库名</td><td>料号</td>");
            sbWeb.Append("<td>品名</td><td>规格</td><td>产品特征</td><td>特征说明</td><td>库存特征</td>");
            sbWeb.Append("<td>数量</td><td>单位</td><td>参考数量</td><td>参考单位</td><td>库龄(天)</td>");
            sbWeb.Append(StorAge.GetWeb.WebMX(site,type,dayType,pageIndex).web);
            sbWeb.Append("</tr></table ></div >");

        }

        protected void BFirst_Click(object sender, EventArgs e)
        {
            pageIndex = 1;
            LMessage.Text = "当前第" + pageIndex + "页,共" + pageCount + "页";
            WebShow();
        }

        protected void BBefore_Click(object sender, EventArgs e)
        {
            pageIndex -= 1;
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            LMessage.Text = "当前第" + pageIndex + "页,共" + pageCount + "页";
            WebShow();
        }

        protected void BNext_Click(object sender, EventArgs e)
        {
            pageIndex += 1;
            if (pageIndex > pageCount)
            {
                pageIndex = pageCount;
            }
            LMessage.Text = "当前第" + pageIndex + "页,共" + pageCount + "页";
            WebShow();
        }

        protected void BLast_Click(object sender, EventArgs e)
        {
            pageIndex = pageCount;
            LMessage.Text = "当前第" + pageIndex + "页,共" + pageCount + "页";
            WebShow();
        }

        protected void BLast_Disposed(object sender, EventArgs e)
        {
            pageIndex = pageCount;
        }
    }
}