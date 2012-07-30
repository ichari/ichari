<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendClient.aspx.cs" Inherits="OnlinePay_Alipay02_SendClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../../../../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        A
        {
            text-transform: none;
            text-decoration: none;
        }
        A:hover
        {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="Form1" name="Form1" method="post" runat="server">
    <br />
    <br />
        <asp:HiddenField ID="hdBankCode" runat="server" />
    <div>
            <table border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;">
                <tr>
                    <td width="20" height="29">
                        &nbsp;
                    </td>
                    <td runat="server" width="100" id="td_Alipay" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        onclick="SetPayPage('Alipay')" align="center" background="../../images/admin_qh_100_2.jpg"
                        class="blue12" style="cursor: pointer;">
                        支付宝
                    </td>
                    <td runat="server" width="4" id="td_Alipay1">
                        &nbsp;
                    </td>
                    <td runat="server" width="100" id="td_99Bill" align="center" background="../../images/admin_qh_100_2.jpg"
                        onclick="SetPayPage('99Bill')" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        class="blue12" style="cursor: pointer;">
                        快钱
                    </td>
                    <td runat="server" width="4" id="td_99Bill1">
                        &nbsp;
                    </td>
                    <td runat="server" width="100" id="td_Yeepay" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        align="center" onclick="SetPayPage('Yeepay')" background="../../images/admin_qh_100_2.jpg"
                        class="blue12" style="cursor: pointer;">
                        易宝
                    </td>
                    <td runat="server" width="4" id="td_Yeepay1">
                        &nbsp;
                    </td>
                    <td runat="server" width="100" id="td_SZX" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        align="center" onclick="SetPayPage('SZX')" background="../../images/admin_qh_100_2.jpg"
                        class="blue12" style="cursor: pointer;">
                        神州行充值卡
                    </td>
                    <td runat="server" width="4" id="td_SZX1">
                        &nbsp;
                    </td>
                    <td runat="server" width="100" id="td_CFT" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        align="center" onclick="SetPayPage('CFT')" background="../../images/admin_qh_100_2.jpg"
                        class="blue12" style="cursor: pointer;">
                        财付通
                    </td>
                    <td width="4">
                        &nbsp;
                    </td>
                    <td runat="server" width="100" id="td_CnCrad" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        align="center" onclick="SetPayPage('CnCrad')" background="../../images/admin_qh_100_2.jpg"
                        class="blue12" style="cursor: pointer;">
                        云网
                    </td>
                    <td runat="server" width="4" id="td_CnCrad1">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table style="padding-top: 0px;" cellpadding="0" cellspacing="0">
                <tr>
                    <td height="1" bgcolor="#FFFFFF">
                    </td>
                </tr>
                <tr>
                    <td height="2" bgcolor="#6699CC">
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" cellspacing="1" cellpadding="0" bgcolor="#DDDDDD">
                <tr>
                    <td height="30" colspan="2" align="left" bgcolor="#E9F1F8" class="black12" style="padding-left: 20px;">
                        您好，<span class="red12"><%=UserName%></span>！您当前帐户余额为：￥<span class="red12"><%= Balance%>
                        </span>元
                    </td>
                </tr>
            </table>
            <div id="divSend">
                <iframe id="iframeSend" name="iframeSend" width="100%" frameborder="0" scrolling="no" src="<%=DefaultSendPage %>"
                    onload="document.getElementById('iframeSend').style.height=iframeSend.document.body.scrollHeight;">
                </iframe>
            </div>
    </div>
    <br />
    </form>
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
        var arr = new Array('Alipay', '99Bill', 'Yeepay', 'SZX', 'CFT', 'CnCrad');
        for (var i = 0; i < arr.length; i++) {
            if (id == arr[i]) {
                switch (id) {
                    case 'Alipay':
                        document.getElementById("iframeSend").src = '../Alipay02/Send_Alipay.aspx';
                        break;
                    case '99Bill':
                        document.getElementById("iframeSend").src = '../Alipay02/Send_99Bill.aspx';
                        break;
                    case 'Yeepay':
                        document.getElementById("iframeSend").src = '../Alipay02/Send_YeePay.aspx';
                        break;
                    case 'SZX':
                        document.getElementById("iframeSend").src = '../007ka/Default.aspx';
                        break;
                    case 'CFT':
                        document.getElementById("iframeSend").src = '../Alipay02/Send_CFT.aspx';
                        break;
                    case 'CnCrad':
                        document.getElementById("iframeSend").src = '../CnCard/Send.aspx';
                        break;
                }
            }
        }
    }

    function mOver(obj, type) {
        if (type == 1) {
            obj.style.textDecoration = "underline";
            obj.style.color = "#FF0065";
        }
        else {
            obj.style.textDecoration = "none";
            obj.style.color = "#226699";
        }
    }

    function PayMoneyOnPress() {
        if (window.event.keyCode < 48 || window.event.keyCode > 57)
            return false;
        return true;
    }
</script>
