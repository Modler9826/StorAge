using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StorAge
{
    public partial class Web03YL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string site = Request.QueryString["asite"].ToString();
            HiddenField1.Value = site;

            //原料类库龄报表分析
            Label1.Text = "原料类库龄报表分析(" + site + ")";

            double[] num = StorAge.GetWeb.YLLeft(site);
            double n1 = num[0];//原纸
            double n2 = num[1];//化工
            double n3 = num[2];//胶水
            double n4 = num[3];//膜(自产)
            double n5 = num[4]; //膜(外购)
            HiddenField2.Value = n1.ToString();
            HiddenField3.Value = n2.ToString();
            HiddenField4.Value = n3.ToString();
            HiddenField5.Value = n4.ToString();
            HiddenField6.Value = n5.ToString();


            double[] num2 = StorAge.GetWeb.YLRight(site);
            double n6 = num2[0]; //  1-15
            double n7 = num2[1]; //  16- 30
            double n8 = num2[2]; //  31 -90
            double n9 = num2[3]; //  91-180
            double n10 = num2[4]; // >180
            double n11 = num2[5]; // >其他

            HiddenField7.Value = n6.ToString();
            HiddenField8.Value = n7.ToString();
            HiddenField9.Value = n8.ToString();
            HiddenField10.Value = n9.ToString();
            HiddenField11.Value = n10.ToString();
            HiddenField12.Value = n11.ToString();
        }
    }
}