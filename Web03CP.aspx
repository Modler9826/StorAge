<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Web03CP.aspx.cs" Inherits="StorAge.Web03CP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>成品类库龄报表分析</title>
    <script src="echarts.min.js"></script>
    <script src="jquery.min.js"></script>
</head>
<body>
    <div style="text-align:center;">
        <form id="form1" runat="server">
            <h1><a href ="Web03.aspx"><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></a>
            </h1>
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
            <asp:HiddenField ID="HiddenField10" runat="server" />
            <asp:HiddenField ID="HiddenField11" runat="server" />

        </form>
    </div>
    <script type="text/javascript">   
        // 基于准备好的dom，初始化echarts实例
        var v_site = $("#HiddenField1").val();

        //数据来源
        var n1 = $("#HiddenField2").val();  //车贴
        var n2 = $("#HiddenField3").val();  //单透
        var n3 = $("#HiddenField4").val();  //PPF
        var n4 = $("#HiddenField5").val();  //外购

        var n5 = $("#HiddenField6").val(); //1-15
        var n6 = $("#HiddenField7").val(); //16-30
        var n7 = $("#HiddenField8").val(); //31- 90
        var n8 = $("#HiddenField9").val(); //90-180
        var n9 = $("#HiddenField10").val(); //>180
        var n10 = $("#HiddenField11").val(); //>其他


        var myChart1 = echarts.init(document.getElementById('pie1'));
        var myChart2 = echarts.init(document.getElementById('pie2'));

        // 指定图表的配置项和数据
        option1 = {
            title: {
                text: '成品生产数据账套比例图',
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
                data: ['1.车贴', '2.单透', '3.PPF','4.外购']
            },
            series: [
                {
                    name: '数据明细',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: [
                        { value: n1, name: '1.车贴' },
                        { value: n2, name: '2.单透' },
                        { value: n3, name: '3.PPF' },
                        { value: n4, name: '4.外购' }
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
                ['#c23531', '#2f4554', '#ff9f7f', '#759aa0',]

        };

        option2 = {
            title: {
                text: '成品生产数据日期比例图',
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
                        { value: n5, name: '1-15天' },
                        { value: n6, name: '16-30天' },
                        { value: n7, name: '31-90天' },
                        { value: n8, name: '91-180天' },
                        { value: n9, name: '181天及以上' },
                        { value: n10, name: '其他' }
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
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart1.setOption(option1);
        myChart1.on('click', function (params) {
            var v_name1 = encodeURIComponent(params.name);
            /*alert(v_name);*/
            if (v_name1.substring(0,1) == '1') {
                v_name1 = "cct";
            } else if (v_name1.substring(0,1) == '2') {
                v_name1 = "cdt";
            } else if (v_name1.substring(0,1) == '3') {
                v_name1 = "cpp";
            }else {
                v_name1 = "cwg";
            }
            window.open('Web03MX.aspx?asite=' + v_site + '&type=' + v_name1 + '&dayType=ALL');
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
            } else {
                v_name2 = "f";
            }
            window.open('Web03MX.aspx?asite=' + v_site + '&type=cp&dayType=' + v_name2);
        });
    </script>
</body>
</html>
