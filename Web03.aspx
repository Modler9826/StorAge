<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Web03.aspx.cs" Inherits="StorAge.Web03" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>库龄报表查询</title>
    <script src="echarts.min.js"></script>
    <script src="jquery.min.js"></script>
</head>
<body>
    <div style="text-align:center;">
        <form id="form1" runat="server">           
         <h1><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            </h1>
        <div id="maintitle" style="width: 100%;height: 50px;" class="auto-style1"><asp:ListBox ID="ListBox1" runat="server" Rows="1" AutoPostBack="True">
            <asp:ListItem Selected="True" Value="cp">成品类</asp:ListItem>
            <asp:ListItem Value="bcp">半成品类</asp:ListItem>
            <asp:ListItem Value="yl">原料类</asp:ListItem>
            </asp:ListBox>
            </div>
            <div id="pie1" style="width:440px;height:400px;display:inline-block;padding-right:40px;"></div> 
            <div id="pie2" style="width:440px;height:400px;display:inline-block;"></div>
             <asp:HiddenField ID="HiddenField1" runat="server" />
             <asp:HiddenField ID="HiddenField2" runat="server" />
             <asp:HiddenField ID="HiddenField3" runat="server" />
             <asp:HiddenField ID="HiddenField4" runat="server" />
             <asp:HiddenField ID="HiddenField5" runat="server" />
             <asp:HiddenField ID="HiddenField6" runat="server" />
             <asp:HiddenField ID="HiddenField7" runat="server" />
             <asp:HiddenField ID="HiddenField8" runat="server" />
             <asp:HiddenField ID="HiddenField9" runat="server" />
        </form>
    </div>
    <script type="text/javascript">

        // 基于准备好的dom，初始化echarts实例
        var v_site = "ALL";
        var myChart1 = echarts.init(document.getElementById('pie1'));
        var myChart2 = echarts.init(document.getElementById('pie2'));

        var v_type = $("#HiddenField1").val();

        //数据来源
        var n1 = $("#HiddenField2").val();
        var n2 = $("#HiddenField3").val();

        var n3 = $("#HiddenField4").val();
        var n4 = $("#HiddenField5").val();
        var n5 = $("#HiddenField6").val();
        var n6 = $("#HiddenField7").val();
        var n7 = $("#HiddenField8").val();
        var n8 = $("#HiddenField9").val();
        // 指定图表的配置项和数据
        option1 = {
            title: {
                text: '生产数据账套比例图',
                subtext: '纳尔股份(NRR)',
                left: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: '{a} <br/>{b} : {c} ({d}%)'
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: ['1.上海纳尔', '2.南通百纳']
            },
            series: [
                {
                    name: '数据明细',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: [
                        { value: n1, name: '1.上海纳尔' },
                        { value: n2, name: '2.南通百纳' }
                    ],
                    emphasis: {
                        itemStyle: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ],
            color:
                ['#2f4554', '#c23531']
               // ['#E75A48', '#A46B8B']
        };

        option2 = {
            title: {
                text: '生产数据日期比例图',
                subtext: '纳尔股份(NRR)',
                left: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: '{a} <br/>{b} : {c} ({d}%)'
            },
            legend: {
                orient: 'vertical',
                left: 'right',
                data: ['1-15天', '16-30天', '31-90天', '91-180天', '181天及以上','其他']
            },
            series: [
                {
                    name: '数据明细',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: [
                        { value: n3, name: '1-15天' },
                        { value: n4, name: '16-30天' },
                        { value: n5, name: '31-90天' },
                        { value: n6, name: '91-180天' },
                        { value: n7, name: '181天及以上' },
                        { value: n8, name: '其他' }
                    ],
                    emphasis: {
                        itemStyle: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ],
            color: ['#C23531', '#2F4554', '#D48265', '#91C7AE', '#dd6b66', '#61A0A8']
            //color: ['#ff9f7f', '#759aa0', '#dd6b66', '#91c7ae', '#749f83']
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart1.setOption(option1);
        myChart1.on('click', function (params) {
            var v_name1 = encodeURIComponent(params.name);
            /*alert(v_name);*/
            if (v_name1.substring(0,1) == '1') {
                v_site = "SHNAR";
            } else {
                v_site = "NTBN";
            }

            if (v_type == "cp") {
                window.open('Web03CP.aspx?asite=' + v_site);
            } else if (v_type == "bcp") {
                window.open('Web03BCP.aspx?asite=' + v_site);
            } else {
                window.open('Web03YL.aspx?asite=' + v_site);
            }
            
        });

        myChart2.setOption(option2);
        myChart2.on('click', function (params) {
            var v_name2 = encodeURIComponent(params.name);
            /*alert(v_name);*/
            if (v_name2.substring(0, 4) == '1-15') {
                v_name2 = "a";
            } else if (v_name2.substring(0, 4) == '16-3') {
                v_name2 = "b";
            } else if (v_name2.substring(0, 4) == '31-9') {
                v_name2 = "c";
            } else if (v_name2.substring(0, 4) == '91-1') {
                v_name2 = "d";
            } else if (v_name2.substring(0, 3) == '181') {
                v_name2 = "e";
            }else {
                v_name2 = "f";
            }
            window.open('Web03MX.aspx?asite=ALL&type=' + v_type +'&dayType=' + v_name2);
        });

    </script>
</body>
</html>
