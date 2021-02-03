<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Web03MX.aspx.cs" Inherits="StorAge.Web03MX" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>库龄报表明细</title>
    <link href="index.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%Response.Write(sbWeb.ToString()); %>
        </div>
        <div class="btn">
            <asp:Button ID="BFirst" runat="server" OnClick="BFirst_Click" Text="首页" Width="58px" />
            <asp:Button ID="BBefore" runat="server" OnClick="BBefore_Click" Text="上一页" Width="76px" />
            <asp:Button ID="BNext" runat="server" OnClick="BNext_Click" Text="下一页" Width="77px" />
            <asp:Button ID="BLast" runat="server" OnClick="BLast_Click" OnDisposed="BLast_Disposed" Text="尾页" Width="63px" />
            <asp:Label ID="LMessage" runat="server"></asp:Label>
        </div>
        <div id="box">到顶部</div>
    </form>
</body>
  <script type="text/javascript">
    var oDiv = document.getElementById("box")
     oDiv.style.display = "none"
     document.onscroll = function () {
        var scrollTop = document.documentElement.scrollTop
        if (scrollTop > 600) {
        oDiv.style.display = "block"
        } else {
         oDiv.style.display = "none"
        }
        oDiv.onclick = function () {
        document.documentElement.scrollTop = 0
        }
     }
  </script>
</html>
