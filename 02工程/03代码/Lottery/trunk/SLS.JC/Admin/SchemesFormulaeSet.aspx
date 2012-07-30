<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchemesFormulaeSet.aspx.cs"
    Inherits="Admin_SchemesFormulaeSet" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table cellpadding="0" width="70%" border="0" align="center" cellspacing='1' bgcolor='#666666'>
            <tr style="background-color:#FFFFFF;">
                <td style="height: 30px; padding-left:130px;" colspan="2">
                    请选择
                    <asp:DropDownList ID="ddlLottery" runat="server" AutoPostBack="True" Width="140px"
                        OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;
                </td>
            </tr>
            <tr style="background-color:#FFFFFF;">
                <td align="right" style="width:30%; height:30px;">
                    方案进度+保底最少金额：
                </td>
                <td align="left" style="width:70%; padding-left:5px;">
                    <asp:TextBox ID="tbMoney" runat="server" onkeypress="return InputMask_Number();"></asp:TextBox>
                    <span style="color:Red;">例如：300</span>
                </td>
            </tr>
            <tr style="background-color:#FFFFFF;">
                <td align="right" style="width:30%; height:30px;">
                    方案进度：
                </td>
                <td align="left" style="width:70%; padding-left:5px;">
                    <asp:TextBox ID="tbSchedule" runat="server" onkeypress="return InputMask_Number();"></asp:TextBox>
                    <span style="color:Red;">数值范围 0 至 100 例如：30</span>
                </td>
            </tr>
            <tr style="background-color:#FFFFFF;">
                <td align="right" style="width:30%; height:30px;">
                    方案最少购买比例：
                </td>
                <td align="left" style="width:70%; padding-left:5px;">
                    <asp:TextBox ID="tbMinMoney" runat="server" onkeypress="return InputMask_Number();"></asp:TextBox>
                    <span style="color:Red;">数值范围 0 至 100 例如：30</span>
                </td>
            </tr>
            <tr style="background-color:#FFFFFF; height:30px;">
                <td align="center" colspan="2">
                    <ShoveWebUI:ShoveConfirmButton ID="btnAdd" BackgroupImage="../Images/Admin/buttbg.gif" runat="server" Width="60px" Height="20px" AlertText="确认书写无误吗？" Text="增加" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    <input type="hidden" id="IsSet" runat="server" value="0" />
    </form>
</body>
</html>
<script language="javascript" type="text/javascript">
    function InputMask_Number() {
        if (window.event.keyCode < 48 || window.event.keyCode > 57)
            return false;
        return true;
    }
</script>