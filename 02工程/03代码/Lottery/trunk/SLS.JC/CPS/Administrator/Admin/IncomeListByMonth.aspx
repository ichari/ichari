<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IncomeListByMonth.aspx.cs"
    Inherits="CPS_Administrator_Admin_IncomeListByMonth" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
    <table width="770" border="0" align="right" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table width="770" border="0" align="right" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                            <tr class="table">
                                                <td height="33" background="../../images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                    <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td class="hui">
                                                                推广首页&nbsp;&gt;收入统计&nbsp;&gt;月度结算表
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 10px;">
                                        <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                            <tr>
                                                <td height="30" colspan="2" align="center" bgcolor="#EAF2FE">
                                                    <strong class="blue12">月度结算表</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding: 10px;" id="tdList" runat="server">
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
    </form>
</body>
</html>
