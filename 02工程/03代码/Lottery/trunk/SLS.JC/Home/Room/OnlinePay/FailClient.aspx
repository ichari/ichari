<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FailClient.aspx.cs" Inherits="OnlinePay_FailClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript"> 
    function win_onLoad(){ 
    var width = document.all["tblist"].offsetWidth;     
    var height = document.all["tblist"].offsetHeight;  
     
    width = eval(width + 150); 
    height = eval(height + 200); 
         
    window.resizeTo(width,height); 
    } 
    </script> 
</head>
<body onload="win_onLoad();">
    <form id="Form1" method="post" runat="server">
    <br />
    <br />
    <table cellspacing="0" cellpadding="0" width="654" align="center" border="0" id="tblist">
        <tr>
            <td valign="top" align="left">
                <table style="border-right: #dfdfdf 1px solid; border-top: #dfdfdf 1px solid; margin: 3px 0px;
                    border-left: #dfdfdf 1px solid; border-bottom: #dfdfdf 1px solid" cellspacing="0"
                    cellpadding="0" width="100%" bgcolor="#efefef" border="0">
                    <tr>
                        <td bgcolor="#ff6600" height="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="border-right: #acacac 1px solid; border-top: #fff 1px solid; border-left: #fff 1px solid;
                            padding-top: 3px; border-bottom: #acacac 1px solid" valign="middle" align="center"
                            height="20">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="zw4" style="width: 19px" valign="middle" align="left">
                                        <img height="13" src="../images/tt.gif" width="13">
                                    </td>
                                    <td class="zw4" valign="middle" align="left">
                                        <strong>支付失败！</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="20" align="center">
                <font face="宋体">
                    <table cellspacing="0" cellpadding="0" width="96%" border="0">
                        <tr>
                            <td height="10">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 49px">
                                <p align="left">
                                    &nbsp;&nbsp;&nbsp; <font color="#ff0000">温馨提示：账户充值失败！如果有问题，请记录下您的支付银行、支付金额等信息后，并与我们联系，我们将在第一时间进行处理，保证您的资金安全。<br />
                                        <b>如果是支付宝支付，数据正在处理中，请稍候再查询你的账户明细，谢谢。</b></font></p>
                            </td>
                        </tr>
                        <tr>
                            <td height="66" style="height: 66px" valign="top">
                                <p align="right">
                                    <font color="#ff0000">--
                                        <%=_Site.Name%>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; </font>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <img alt="" src="../Images/Error01.gif">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                </br>
                                <input class="button" id="Close" type="button" onclick="javascript:window.close();"
                                    value=" 关闭窗口 " name="Close" />
                                </br>
                                </br>
                            </td>
                        </tr>
                    </table>
                </font>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
