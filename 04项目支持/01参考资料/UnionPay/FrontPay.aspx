<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrontPay.aspx.cs" Inherits="Home_Room_OnlinePay_UnionPay_FrontPay" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Home/Room/Style/css.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        window.onload = function () {
            var dURL = top.location.protocol + "//" + top.location.host + "/Home/Room/OnlinePay/UnionPay/Default.aspx";
            dURL = dURL.toLowerCase();
            if (top.location.toString().toLowerCase() != dURL)
                window.location = "/Home/Room/OnlinePay/UnionPay/Default.aspx";
        }
        function CheckMultiple(sender) {
            var money = StrToFloat(sender.value);
            sender.value = money;

            if (money < 2) {
                if (confirm("充值金额不正确，按“确定”重新输入，按“取消”自动更正为 2 元，请选择。")) {
                    sender.focus();
                    return;
                }
                else {
                    sender.value = 2;
                }
            }
        }
        function StrToFloat(str) {
            var NewStr = "";
            for (var i = 0; i < str.length; i++) {
                if (str.charAt(i) != "," && str.charAt(i) != " ")
                    NewStr += str.charAt(i);
            }

            if (NewStr == "")
                return 0;

            var f = parseFloat(NewStr);
            if (isNaN(f))
                return 0;

            return f;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
                <tr>
                    <td width="93" height="30" align="right" bgcolor="#FFFFFF" class="black12">
                        <span class="red12">*</span>充值金额：
                    </td>
                    <td width="685" align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                        <span class="black12">
                            <label>
                                <asp:TextBox ID="PayMoney" runat="server" MaxLength="8" CssClass="in_text" onblur="CheckMultiple(this);" Text="2"></asp:TextBox>
                            </label>
                        </span>
                        <span class="black12">
                            元，（<span class="red12">网上充值免手续费</span>，存入金额最少2元）
                        </span>
                    </td>
                </tr>
                <tr>
                    <td height="30" valign="top" bgcolor="#FFFFFF" class="black12" style="padding-top:10px;text-align:right;width:93px;">
                        <span class="red12">*</span>充值方式：
                    </td>
                    <td valign="top" bgcolor="#FFFFFF" class="black12" style="text-align:left;height:30px;padding:10px 0px 10px 10px;">
                        <label>
                            <asp:RadioButton ID="radUPOP" runat="server" Checked="true" /><span style="margin-left:10px;"></span><img src="/Home/Room/OnlinePay/UnionPay/Images/unionpay_logo.gif" alt="UnionPay" />
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="width:93px;"></td>
                    <td style="padding:15px 0px 10px 10px;">
                        <ShoveWebUI:ShoveConfirmButton ID="btnNext" Style="cursor: pointer;" runat="server" Width="109px" Height="33px" CausesValidation="False" BackgroupImage="/Home/Room/Images/bt_wy.jpg"
                            BorderStyle="None" OnClick="btnNext_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
