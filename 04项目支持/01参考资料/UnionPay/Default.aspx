<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Home_Room_OnlinePay_UnionPay_Default" %>

<%@ Register Src="../../UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>银联在线支付</title>
    <link href="../../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
    <div id="content">
        <div id="menu_left" style="border: 1px solid #BCD2E9; background-color: #E1EFFC;">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" />
        </div>
        <div id="menu_right">
            <table width="842" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="40" height="30" align="right" valign="middle" class="red14">
                        <img src="../../Images/icon_5.gif" width="19" height="16" />
                    </td>
                    <td valign="middle" class="red14" style="padding-left: 10px;">
                        我的帐户
                    </td>
                </tr>
            </table>
            <table border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;">
                <tr>
                    <td width="20" height="29">
                        &nbsp;
                    </td>
                    <td align="center" class="blue12" style="width:100px;cursor: pointer;background-image:url(/Home/Room/Images/admin_qh_100_2.jpg);">
                        银联在线
                    </td>
                </tr>
            </table>
            <div style="background-color:#FFFFFF;width:100%;height:1px;"></div>
            <div style="background-color:#6699CC;width:100%;height:2px;"></div>
            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#DDDDDD">
                <tr>
                    <td height="30" colspan="2" align="left" bgcolor="#E9F1F8" class="black12" style="padding-left: 20px;">
                        您好，<span class="red12"><%=UserName%></span>！您当前帐户余额为：￥<span class="red12"><%= Balance%></span>元
                    </td>
                </tr>
            </table>
            <div style="height:20px;width:100%;background-color:#FFFFFF"></div>
            <div id="divSend">
                <iframe id="iframeSend" name="iframeSend" width="100%" frameborder="0" scrolling="no" src="/Home/Room/OnlinePay/UnionPay/FrontPay.aspx"
                    onload="document.getElementById('iframeSend').height=iframeSend.document.body.scrollHeight;">
                </iframe>
            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
