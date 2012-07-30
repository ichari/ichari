<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChellengeUserSchemesDetails.aspx.cs"
    Inherits="Challenge_ChellengeUserSchemesDetails" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="Style/ring.css" />
</head>
<body onload="Changepage(1)">
    <form id="form1" runat="server">
    <div class="popuptab">
        <table class="table_c1" id="productTable" cellspacing="1" cellpadding="0" width="100%"
            align="center" border="0">
            <tbody>
                <tr>
                    <td colspan="7" class="pop_hh">
                        <strong>
                            <asp:Label runat="server" ID="lbUserName" ForeColor="Red"></asp:Label></strong><strong>
                                的擂台比拼历史战绩</strong>
                    </td>
                </tr>
                <tr class="topring_search">
                    <td height="33" colspan="7" style="text-align: left; padding-left: 10px; border-top: 1px solid #FDE2C6">
                        方案搜索
                        <select name="dropYear" id="dropYear">
                        </select>
                        <select name="dropMonth" id="dropMonth">
                            <option value="1">1月</option>
                            <option value="2">2月</option>
                            <option value="3">3月</option>
                            <option value="4">4月</option>
                            <option value="5">5月</option>
                            <option value="6">6月</option>
                            <option value="7">7月</option>
                            <option value="8">8月</option>
                            <option value="9">9月</option>
                            <option value="10">10月</option>
                            <option value="11">11月</option>
                            <option value="12">12月</option>
                        </select>
                        <input type="button" onclick="Search()" value="查 询" id="btnSearch" class="btn_ringso" />
                    </td>
                </tr>
                <tr class="tre">
                    <td width="13%">
                        日期
                    </td>
                    <td width="9%">
                        选择场数
                    </td>
                    <td width="10%">
                        过关
                    </td>
                    <td width="46%">
                        投注内容
                    </td>
                    <td width="8%">
                        积分
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="divload" style="top: 50%; right: 50%; padding: 0px; margin: 0px; z-index: 999;clear:both;">
                       <center><img src="../Images/ajax-loader.gif" alt=""/></center>
        </div>
        <div style="width: 100%; text-align: center;">
            <img src="images/btn_close.gif" onclick="window.close();return false;" onmouseover="this.style.cursor='pointer'"
                width="87" height="30" style="margin-left: 10px; cursor: pointer;"/>
        </div>
    </div>
    <asp:HiddenField ID="userId" runat="server" Value="" />
    <asp:HiddenField ID="currentPageIndex" runat="server" Value="1" />
    </form>
    <script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="JScript/details.js" type="text/javascript"></script>
</body>
</html>
