using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StorAge
{
    public partial class Web03 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HiddenField1.Value = ListBox1.SelectedItem.Value.ToString();
            string type = HiddenField1.Value;

            switch (type)
            {
                case "cp":
                    Label1.Text = "库龄报表分析(成品类)";
                    break;
                case "bcp":
                    Label1.Text = "库龄报表分析(半成品类)";
                    break;
                case "yl":
                    Label1.Text = "库龄报表分析(原料类)";
                    break;
            }
            double[] num = StorAge.GetWeb.Web03Left(type);
            double n1 = num[0];//SHNAR
            double n2 = num[1];//NTBN
            HiddenField2.Value = n1.ToString();
            HiddenField3.Value = n2.ToString();

            double[] num2 = StorAge.GetWeb.Web03Right(type);
            double n3 = num2[0];//1-15
            double n4 = num2[1];//16-30
            double n5 = num2[2];//31-90
            double n6 = num2[3];//91-180
            double n7 = num2[4];//>180
            double n8 = num2[5];//其他
            HiddenField4.Value = n3.ToString();
            HiddenField5.Value = n4.ToString();
            HiddenField6.Value = n5.ToString();
            HiddenField7.Value = n6.ToString();
            HiddenField8.Value = n7.ToString();
            HiddenField9.Value = n8.ToString();
        }
    }
}