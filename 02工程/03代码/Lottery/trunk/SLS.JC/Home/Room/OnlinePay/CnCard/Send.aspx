<%@ Page Language="c#" Inherits="Home_Room_OnlinePay_CnCard_Send" CodeFile="Send.aspx.cs" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        无标题页
    </title>
    <link href="../../../../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link href="../../Style/main.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
			A { TEXT-TRANSFORM: none; TEXT-DECORATION: none }
			A:hover { TEXT-DECORATION: underline }
		</style>

    <script type="text/javascript">
		<!--
		function PayMoneyOnPress()
		{
			if (window.event.keyCode < 48 || window.event.keyCode > 57)
				return false;
			return true;
		}
		-->
    </script>

</head>
<body>
    <table cellspacing="0" cellpadding="0" width="393" bgcolor="#f4f4f4" border="0" align="center">
        <tr>
            <td width="15">
            </td>
            <td valign="top" width="370">
                <form name="form1" method="post" runat="server">
                <asp:Panel ID="Panel2" runat="server" Visible="true" Width="100%">
                    <table width="810" border="0" cellpadding="0" cellspacing="0" bgcolor="#C0DBF9">
                    <tr>
                        <td height="10" colspan="2" align="right" bgcolor="#FFFFFF" class="black12">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="93" height="30" align="right" bgcolor="#FFFFFF" class="black12">
                            <span class="red12">*</span>充值金额：
                        </td>
                        <td width="685" align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr bgcolor="#C0DBF9">
                                    <td width="23%" align="left" bgcolor="#FFFFFF" class="black12">
                                        <label>
                                            <asp:TextBox ID="PayMoney" runat="server" MaxLength="8" CssClass="in_text" onblur="CheckMultiple(this);"
                                                Text="2"></asp:TextBox>
                                        </label>
                                    </td>
                                    <td width="77%" align="left" bgcolor="#FFFFFF" class="black12">
                                        元，（<span class="red12">网上充值免手续费</span>，存入金额最少2元）
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="right" valign="top" bgcolor="#FFFFFF" class="black12" style="padding-top: 10px;">
                            <span class="red12">*</span>充值方式：
                        </td>
                        <td height="30" align="left" valign="top" bgcolor="#FFFFFF" class="black12" style="padding: 10px 0px 10px 10px;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" >
                                <tr style="padding-bottom:3px;">
                                    <td>
                                        <table width="150" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                     <table width="150" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="26">
                                                                <label>
                                                                    <asp:RadioButton ID="radCnCard" GroupName="payWay" runat="server" Checked="true" />
                                                                </label>
                                                            </td>
                                                            <td width="124" height="28">
                                                                <img src="../../images/bank_logo/Logo_CnCard.gif" width="112" height="32" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td colspan="3">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 15px;">
                                <tr>
                                    <td>
                                        <ShoveWebUI:ShoveConfirmButton ID="btnNext" Style="cursor: pointer;" runat="server"
                                            Width="109px" Height="33px" CausesValidation="False" BackgroupImage="../../images/bt_next.jpg"
                                            BorderStyle="None" OnClick="btnNext_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                </form>
            </td>
            <td width="72">
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Panel ID="Panel1" runat="server" Visible="False" Width="100%">
                    <form id="E_FORM" name="E_FORM" action="https://www.cncard.net/purchase/getorder.asp" method="post" target="_top">
                    <table width="805" border="0" cellpadding="0" cellspacing="0" bgcolor="#C0DBF9" style="margin-top: 15px;">
                    <tr>
                        <td height="10" colspan="2" align="right" bgcolor="#FFFFFF" class="black12">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="105" height="30" align="right" bgcolor="#FFFFFF" class="black12">
                            <span class="red12">*</span>您的充值金额：
                        </td>
                        <td width="700" align="left" bgcolor="#FFFFFF" class="black12" style="padding-left: 10px;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr bgcolor="#C0DBF9">
                                    <td width="23%" align="left" bgcolor="#FFFFFF" class="black12">
                                        <span class="red12">
                                            <asp:Label ID="lbPayMoney" runat="server"></asp:Label>
                                        </span>元
                                    </td>
                                    <td width="77%" align="left" bgcolor="#FFFFFF" class="black12">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="right" valign="top" bgcolor="#FFFFFF" class="black12" style="padding-top: 10px;">
                            <span class="red12">*</span>选择支付方式：
                        </td>
                        <td height="30" align="left" valign="top" bgcolor="#FFFFFF" style="padding: 10px 0px 10px 10px;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="24%">
                                        <img id="BankImg" src="../../images/bank_logo/Logo_CnCard.gif" width="121" height="27" />
                                    </td>
                                    <td width="76%" class="blue12">
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 15px;">
                                <tr>
                                    <td class="red12">
                                        <input id="Submit" type="submit" value=" 进行网上支付 " name="Submit" style="background-image:url(../../images/bt_wy.jpg);" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;将会在新窗口中打开支付页面。
                                    </td>
                                </tr>
                                <tr>
                                    <td class="red13">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="red99">
                                        <span style="color: #000000; font-weight: normal">注：为了及时到帐，充值完成后【</span>请不要关闭网银或支付窗口<span
                                            style="color: #000000; font-weight: normal">】 ，系统会自动跳转回本网站！！！</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                
                        <input type="hidden" value="<%=c_mid%>" name="c_mid" />
                        <input type="hidden" value="<%=c_order%>" name="c_order" />
                        <input type="hidden" value="<%=c_orderamount%>" name="c_orderamount" />
                        <input type="hidden" value="<%=c_ymd%>" name="c_ymd" />
                        <input type="hidden" value="<%=c_moneytype%>" name="c_moneytype" />
                        <input type="hidden" value="<%=c_retflag%>" name="c_retflag" />
                        <input type="hidden" value="<%=c_paygate%>" name="c_paygate" />
                        <input type="hidden" value="<%=c_returl%>" name="c_returl" />
                        <input type="hidden" value="<%=c_memo1%>" name="c_memo1" />
                        <input type="hidden" value="<%=c_memo2%>" name="c_memo2" />
                        <input type="hidden" value="<%=c_language%>" name="c_language" />
                        <input type="hidden" value="<%=notifytype%>" name="notifytype" />
                        <input type="hidden" value="<%=c_signstr%>" name="c_signstr" />
                        <!-- 可选-->
                        <input type="hidden" name="c_name" value="<%=c_name%>" />
                        <input type="hidden" name="c_address" value="<%=c_address%>" />
                        <input type="hidden" name="c_post" value="<%=c_post%>" />
                        <input type="hidden" name="c_tel" value="<%=c_tel%>" />
                        <input type="hidden" name="c_email" value="<%=c_email%>" />
                        </form>
                </asp:Panel>
            </td>
        </tr>
    </table>
</body>
</html>

<script type="text/javascript">
		<!--
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
		-->
</script>

