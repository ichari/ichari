<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Fail.aspx.cs" Inherits="Home_Room_OnlinePay_Fail" %>
<%@ Register Src="../UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>网上支付，手机卡充值-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。"/>
    <meta name="keywords" content="<%=_Site.Name %>提供网银充值，支付宝支付，财付通支付" />
    <link href="../../../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link href="../../../Style/main.css" type="text/css" rel="stylesheet" />
</head><link rel="shortcut icon" href="../../../favicon.ico"/>
<body class="gybg">
    <form id="Form1" method="post" runat="server">
     <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">

    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" />
        </div>
        <div id="menu_right" style="width:736px;">
         <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#9FC8EA">
        <tr>
            <td valign="top" bgcolor="#FFFFFF">
         
                <table id="myIcaileTab" runat="server" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E4E4E5">
                                <tr>
                                    <td height="30" colspan="2" align="center" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                        <strong>支付失败！</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" colspan="2" align="center" bgcolor="#FFFFFF" class="black12">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td colspan="2" align="left" style="padding :15px  25px  15px  25px;">
                                                <br />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lab1" runat="server"></asp:Label><br /><br />
                                                    &nbsp;&nbsp;&nbsp; <font color="red">温馨提示：账户充值失败！如果有问题，请记录下您的支付银行、支付金额等信息后，并与我们联系，我们将在第一时间进行处理，<br />保证您的资金安全。
                                                    <br /><br />&nbsp;&nbsp;&nbsp;
                                            <b>如果是支付宝支付，数据正在处理中，请稍候再查询你的账户明细，谢谢。</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" height="20">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left"  style="padding-left :25px; padding-top :15px; border-top:1px solid #E4E4E5; ">
                                                    <a class="li3" href="../AccountDetail.aspx">【查看我的账户】</a> <a class="li3" href="../Invest.aspx">
                                                        【查看我的投注记录】</a> <a class="li3" href="../Service.aspx" target="_blank">【我要反映问题】</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" height="50">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
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
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
