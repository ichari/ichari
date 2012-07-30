<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewMessage.aspx.cs" Inherits="Home_Room_ViewMessage" %>
<%@ Register Src="UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
     <title>查看我的站内信息-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
     
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
<link rel="shortcut icon" href="../../favicon.ico" /></head>
<body class="gybg">
    <form id="form2" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">

    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="40" height="30" align="right" valign="middle" class="red14">
                        <img src="images/icon_6.gif" width="19" height="16" />
                    </td>
                    <td valign="middle" class="red14" style="padding-left: 10px;">
                        我的彩票记录
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;">
                <tr>
                    
                    <td width="100" align="center" background="images/admin_qh_100_1.jpg" class="afff fs14">
                        <strong>查看信息</strong>
                    </td>
                    <td background="images/admin_qh_100_2.jpg" height="32">
                        &nbsp;
                    </td>
                   
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#DDDDDD" style="margin-top: 10px;">
                <tr>
                    <td height="30" align="right" bgcolor="#F8F8F8" class="style1" style="width:120px;">
                        发信人：<span class="red12"></span>
                    </td>
                    <td align="left" style="width:722px" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                        <asp:Label ID="lbAim" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                        <a href="Message.aspx" class="blue12">【返回】</a>
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right" bgcolor="#F8F8F8" class="style1" style="width:120px;">
                        内容：
                    </td>
                    <td align="left" style="width:722px" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                        <asp:Label ID="lbContent" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>


</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>

    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
