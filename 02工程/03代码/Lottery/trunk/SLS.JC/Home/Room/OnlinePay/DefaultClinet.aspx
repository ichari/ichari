<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultClinet.aspx.cs" Inherits="OnlinePay_DefaultClinet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
<link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
    <!--
    .STYLE1 {
	    font-size: 12px;
	    color: #FF0000;
    }
    .STYLE2 {
	    font-size: 14px;
	    font-weight: bold;
    }
    -->
    </style>
</head>
<body>
    <%--Response.Redirect("http://www.hb-win.com/Room", true);--%>
    <form id="Form1" method="post" runat="server">
        <%--<font face="宋体">
            <br />
            <br />
            <table width="725" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td><img src="../images/ShopSite/slszifu_03.gif" width="725" height="42" alt=""></td>
              </tr>
              <asp:Panel ID="Panel_Alipay" runat="server">
              <tr>
                <td height="73" background="../images/ShopSite/slszifu_05.gif">
	            <table width="720" height="73" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="168" rowspan="2" align="center"><a href="<%=OnlinePay_Alipay_HomePage%>" target="<%=OnlinePay_Alipay_Target%>"><img src="Alipay\logo.jpg" border="0" alt="本支付通过支付宝接口支付"></a></td>
                    <td width="364" rowspan="2"><span class="STYLE2">支付宝在线支付，支持国内大多数银行卡，快捷方便，安全性高。</span></td>
                    <td height="59" colspan="2" align="center"><a href="<%=OnlinePay_Alipay_HomePage%>" target="<%=OnlinePay_Alipay_Target%>"><img src="Alipay\button.gif" border="0" alt="本支付通过支付宝接口支付"></a></td>
                  </tr>
                  <tr>
                    <td width="28" height="18">&nbsp;</td>
                    <td width="160"><span class="STYLE1">本支付通过支付宝接口支付</span></td>
                  </tr>
                </table>
	            </td>
              </tr>
              </asp:Panel>
              <asp:Panel ID="Panel_99Bill" runat="server">
              <tr>
                <td valign="top"><img src="../images/ShopSite/slszifu_15.gif" width="725" height="1" alt=""></td>
              </tr>
              <tr>
                <td height="73" background="../images/ShopSite/slszifu_05.gif">                
	            <table width="720" height="73" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="168" rowspan="2" align="center"><a href="<%=OnlinePay_99Bill_HomePage%>" target="<%=OnlinePay_99Bill_Target%>"><img src="99Bill\logo.jpg" border="0" alt="本支付通过快钱接口支付"></a></td>
                    <td width="364" rowspan="2"><span class="STYLE2">通过快钱在线支付，快钱账户间转帐及时到帐。支持国内大多数银行卡，快捷方便，安全性高。</span></td>
                    <td height="59" colspan="2" align="center"><a href="<%=OnlinePay_99Bill_HomePage%>" target="<%=OnlinePay_99Bill_Target%>"><img src="99Bill\button.gif" border="0" alt="本支付通过快钱接口支付"></a></td>
                  </tr>
                  <tr>
                    <td width="28" height="18">&nbsp;</td>
                    <td width="160"><span class="STYLE1">本支付通过快钱接口支付</span></td>
                  </tr>
                </table>
	            </td>
              </tr>
              </asp:Panel>
              <asp:Panel ID="Panel_Tenpay" runat="server">
              <tr>
                <td valign="top"><img src="../images/ShopSite/slszifu_15.gif" width="725" height="1" alt=""></td>
              </tr>
              <tr>
                <td height="73" background="../images/ShopSite/slszifu_05.gif">
	            <table width="720" height="73" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="168" rowspan="2" align="center"><a href="<%=OnlinePay_Tenpay_HomePage%>" target="<%=OnlinePay_Tenpay_Target%>"><img src="Tenpay\logo.jpg" border="0" alt="本支付通过腾讯接口支付"></a></td>
                    <td width="364" rowspan="2"><span class="STYLE2">财付通在线支付，支持国内大多数银行卡，快捷方便，安全性高。</span></td>
                    <td height="59" colspan="2" align="center"><a href="<%=OnlinePay_Tenpay_HomePage%>" target="<%=OnlinePay_Tenpay_Target%>"><img src="Tenpay\button.gif" border="0" alt="本支付通过腾讯接口支付"></a></td>
                  </tr>
                  <tr>
                    <td width="28" height="18">&nbsp;</td>
                    <td width="160"><span class="STYLE1">本支付通过腾讯接口支付</span></td>
                  </tr>
                </table>
	            </td>
              </tr>
              </asp:Panel>
              <asp:Panel ID="Panel_CBPayMent" runat="server">
              <tr>
                <td valign="top"><img src="../images/ShopSite/slszifu_15.gif" width="725" height="1" alt=""></td>
              </tr>
              <tr>
                <td height="73" background="../images/ShopSite/slszifu_05.gif">
	            <table width="720" height="73" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="168" rowspan="2" align="center"><a href="<%=OnlinePay_CBPayMent_HomePage%>" target="<%=OnlinePay_CBPayMent_Target%>"><img src="CBPayMent\logo.jpg" border="0" alt="本支付通过网银在线接口支付"></a></td>
                    <td width="364" rowspan="2"><span class="STYLE2">通过网银在线支付，支持国内大多数银行卡，快捷方便，安全性高。</span></td>
                    <td height="59" colspan="2" align="center"><a href="<%=OnlinePay_CBPayMent_HomePage%>" target="<%=OnlinePay_CBPayMent_Target%>"><img src="CBPayMent\button.gif" border="0" alt="本支付通过网银在线接口支付"></a></td>
                  </tr>
                  <tr>
                    <td width="28" height="18">&nbsp;</td>
                    <td width="160"><span class="STYLE1">本支付通过网银在线接口支付</span></td>
                  </tr>
                </table>
	            </td>
              </tr>
              </asp:Panel>
              <tr>
                <td valign="top"><img src="../images/ShopSite/slszifu_28.gif" width="725" height="23" alt=""></td>
              </tr>
            </table>        
        </font>--%>
    </form>
</body>
</html>

