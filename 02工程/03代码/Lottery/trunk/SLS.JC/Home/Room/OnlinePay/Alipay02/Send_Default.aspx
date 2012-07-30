<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Send_Default.aspx.cs" Inherits="Home_Room_OnlinePay_Alipay02_Send_Default" %>

<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=_Site.Name %>-在线充值</title>
    <link href="../../Style/css.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="/favicon.ico" />
    <style type="text/css">
        A{
            text-transform: none;
            text-decoration: none;
        }
        A:hover{
            text-decoration: underline;
        }
    </style>
</head>

<body class="gybg">
<form id="Form1" name="Form1" method="post" runat="server">
<asp:HiddenField ID="hdBankCode" runat="server" />
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="acDeposit" />
        </div>
        <div id="menu_right">

            <table width="100%" border="0" style="border:#E4E4E5 1px solid;" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="40" height="32" align="right"align="middle" class="red14">
                       <img src="../../Images/icon_5.gif" width="19" height="16" />
                    </td>
                    <td valign="middle" class="red14" style="padding-left: 10px;">
                        我的账户
                    </td>
                </tr>
            </table>
           <table border="0" cellspacing="0" width="100%" cellpadding="0" style="background:url(../../images/admin_qh_100_2.jpg);margin-top: 10px;">
	<tr style="background:url(../../images/admin_qh_100_2.jpg);">
		
        <td runat="server" width="100" id="td_ChinaUnion" onclick="SetPayPage('Alipay')" align="center" background="../../images/admin_qh_100_1.jpg" class="afff fs14" style="cursor: pointer;"> 银联在线 </td>
		<td runat="server" width="4" id="td_ChinaUnion1">&nbsp;</td>
        <td runat="server" width="100" id="td_Alipay" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" onclick="SetPayPage('Alipay')" align="center" background="../../images/admin_qh_100_2.jpg" class="blue12" style="cursor: pointer;"> 支付宝 </td>
		<td runat="server" width="4" id="td_Alipay1">&nbsp;</td>
		<td runat="server" width="100" id="td_99Bill" align="center" background="../../images/admin_qh_100_2.jpg" onclick="SetPayPage('99Bill')" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" class="blue12" style="cursor: pointer;"> 快钱 </td>
		<td runat="server" width="4" id="td_99Bill1">&nbsp;</td>
		<td runat="server" width="100" id="td_Yeepay" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" align="center" onclick="SetPayPage('Yeepay')" background="../../images/admin_qh_100_2.jpg" class="blue12" style="cursor: pointer;"> 易宝 </td>
		<td runat="server" width="4" id="td_Yeepay1">&nbsp;</td>
		<td runat="server" width="100" id="td_SZX" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" align="center" onclick="SetPayPage('SZX')" background="../../images/admin_qh_100_2.jpg"  class="blue12" style="cursor: pointer;"> 神州行充值卡 </td>
		<td runat="server" width="4" id="td_SZX1">&nbsp;</td>
		<td runat="server" width="100" id="td_CFT" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"  align="center" onclick="SetPayPage('CFT')" background="../../images/admin_qh_100_2.jpg" class="blue12" style="cursor: pointer;"> 财付通 </td>
		<td>&nbsp;</td>
        <td width="20" height="32">&nbsp;</td>
		<td runat="server" width="100" id="td_CnCrad" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" align="center" onclick="SetPayPage('CnCrad')" background="../../images/admin_qh_100_2.jpg" class="blue12" style="cursor: pointer;"> 云网 </td>
		<td runat="server" width="4" id="td_CnCrad1">&nbsp;</td>
	</tr>
</table>
            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#E4E4E5">
                <tr>
                    <td height="30" colspan="2" align="left" bgcolor="#E9F1F8" class="black12" style="padding-left: 20px;">
                        您好，<span class="red12"><%=UserName%></span>！您当前帐户余额为：￥<span class="red12"><%= Balance%>
                        </span>元
                    </td>
                </tr>
            </table>
            <div id="divSend">
                <iframe id="iframeSend" name="iframeSend" width="100%" frameborder="0" scrolling="no" src="<%=DefaultSendPage %>"
                    onload="document.getElementById('iframeSend').height=iframeSend.document.body.scrollHeight;">
                </iframe>
            </div>
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

<script type="text/javascript">
    function reinitIframe() {
        var iframe = document.getElementById("iframeSend");
        try {
            var bHeight = iframe.contentWindow.document.body.scrollHeight;
            var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
            var height = Math.max(bHeight, dHeight);
            iframe.height = height;
        } catch (ex) { }
    }
    window.setInterval("reinitIframe()", 200); 
    
    function SetPayPage(id) {
        var arr = new Array('ChinaUnion','Alipay', '99Bill', 'Yeepay',  'SZX', 'CFT', 'CnCrad');
        for (var i = 0; i < arr.length; i++) {
            if (id == arr[i]) {
                switch (id) {
                    case 'ChinaUnion':
                        document.getElementById("iframeSend").src = 'Send_ChinaUnion.aspx';
                        break;
                    case 'Alipay':
                        document.getElementById("iframeSend").src = 'Send_Alipay.aspx';
                        break;
                    case '99Bill':
                        document.getElementById("iframeSend").src = 'Send_99Bill.aspx';
                        break;
                    case 'Yeepay':
                        document.getElementById("iframeSend").src = 'Send_YeePay.aspx';
                        break;
                    case 'SZX':
                        document.getElementById("iframeSend").src = '../007ka/Default.aspx';
                        break;
                    case 'CFT':
                        document.getElementById("iframeSend").src = 'Send_CFT.aspx';
                        break;
                    case 'CnCrad':
                        document.getElementById("iframeSend").src = '../CnCard/Send.aspx';
                        break;
                }
            }
        }
    }

    function mOver(obj, styleClass) {
        if (obj != lastCheckMenu) {
            obj.className = styleClass;
        }
    }

    function mOut(obj, styleClass) {
        if (obj != lastCheckMenu) {
            obj.className = styleClass;
        }
    }

    function mOver(obj, type) {
        if (type == 1) {
            obj.style.textDecoration = "none";
            obj.style.color = "#FF0065";
        }
        else {
            obj.style.textDecoration = "none";
            obj.style.color = "#226699";


        }
    }
    
</script>