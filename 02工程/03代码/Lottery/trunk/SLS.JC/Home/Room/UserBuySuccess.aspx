<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserBuySuccess.aspx.cs" Inherits="Home_Room_UserBuySuccess" %>

<%@ Register Src="UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>祝贺购彩成功！-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
     
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            font-size: 12px;
            color: #226699;
            font-family: "tahoma";
            line-height: 20px;
            height: 28px;
        }
        A
        {
            text-decoration: none;
        }
        A:hover
        {
            color: #CC0000;
            text-decoration: underline;
        }
    </style>
<link rel="shortcut icon" href="../../favicon.ico" /></head>
<body class="gybg">
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">

    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8">
                <tr>
                    <td align="left" bgcolor="#FFFFDD" class="black12" style="padding: 5px 10px 5px 10px;">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="9%">
                                    <img src="images/icon_cg222.jpg" width="73" height="52" />
                                </td>
                                <td width="91%" class="red14_2">
                                    <asp:Label runat="server" ID="lbName" />，祝贺您<asp:Label runat="server" ID="lbType"
                                        Text="投注"></asp:Label>成功！ <span id="time" style="color: Red; padding-left: 20px">5</span><font
                                            color="black"> 秒后自动跳转！</font>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="36" bgcolor="#F5F5F5" class="black12" style="padding: 5px 10px 5px 10px;">
                        <div style="padding-bottom: 10px">
                            本次获得积分：<asp:Label runat="server" ID="lbScore" CssClass="red12" />分</div>
                        您好！
                        <asp:Label runat="server" ID="lbName1" CssClass="red12" />
                        您的账户余额：<asp:Label runat="server" ID="lbBalance" CssClass="red12" />
                        元&nbsp;&nbsp;&nbsp; <a href="" runat="server" id="Buy">
                            <asp:Label runat="server" ID="lbType1" CssClass="blue12" Text="[继续投注]" /></a><a href=""
                                runat="server" id="Look" style="padding-left: 10px" target="_blank"><span class="blue12">[查看方案]</span></a>
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

<script language="javascript" type="text/javascript">
    var timerID;
    
    function DisplayTimer() {
        var seconds = parseInt(time.innerHTML)>0 ? parseInt(time.innerHTML)-1:0;
        time.innerHTML = seconds.toString();
        if (seconds == 0) {
            clearInterval(timerID);
            window.top.location.href ="<%=URL %>";
        }
    }

    timerID = setInterval('DisplayTimer()', 1000);
</script>