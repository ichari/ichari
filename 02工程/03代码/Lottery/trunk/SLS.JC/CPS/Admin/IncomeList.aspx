<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IncomeList.aspx.cs" Inherits="CPS_Admin_IncomeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/css.css" rel="stylesheet" type="text/css" />

    <script src="../../Components/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
    <table width="770" border="0" align="right" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="770" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table width="770" border="0" align="right" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                <tr class="table">
                                    <td height="33" background="../images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="hui">
                                                    推广首页&nbsp;&gt;收入统计&nbsp;&gt;&nbsp;收入明细
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
                                        <strong class="blue12">收入明细</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding: 10px;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="75" height="30" align="right">
                                                    开始日期：
                                                </td>
                                                <td width="122">
                                                    <asp:TextBox runat="server" ID="tbBeginTime" CssClass="hui_cc" Width="100px" onblur="if(this.value=='') this.value=document.getElementById('hBeginTime').value"
                                                        name="tbBeginTime" onFocus="WdatePicker({el:'tbBeginTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                                </td>
                                                <td width="82" align="right">
                                                    截止时间：
                                                </td>
                                                <td width="165">
                                                    <asp:TextBox runat="server" ID="tbEndTime" CssClass="hui_cc" Width="100px" name="tbEndTime"
                                                        onblur="if(this.value=='') this.value=document.getElementById('hEndTime').value"
                                                        onFocus="WdatePicker({el:'tbEndTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                                </td>
                                                <td width="304">
                                                    <asp:Button ID="btnSearch" runat="server" Text="搜索" onclick="btnSearch_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="divIncomeList" runat="server"></div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hBeginTime" runat="server" />
    <asp:HiddenField ID="hEndTime" runat="server" />
    </form>
</body>
</html>
