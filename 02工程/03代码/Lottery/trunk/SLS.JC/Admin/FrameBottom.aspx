<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrameBottom.aspx.cs" Inherits="Admin_FrameBottom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../Style/Admin.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr background="../Images/Admin/slsht_13.jpg">
                <td width="50"></td>
                <td height="30" width="400">
                    &nbsp;<a href="News.aspx" target="mainFrame">新闻资讯</a>&nbsp; |&nbsp; <a href="SiteAffiches.aspx" target="mainFrame">站点公告</a>&nbsp; |&nbsp; <a href="Users.aspx" target="mainFrame">用户一览</a>&nbsp; |&nbsp; <a href="UserAddMoney.aspx" target="mainFrame">账户充值</a></td>
                <td><asp:LinkButton ID="lbLogout" runat="server" OnClick="lbLogout_Click" Width="100">【安全退出】</asp:LinkButton></td>
                <td></td>
            </tr>
        </table>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
