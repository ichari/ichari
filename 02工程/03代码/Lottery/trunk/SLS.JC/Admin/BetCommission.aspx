<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BetCommission.aspx.cs" Inherits="Admin_BetCommission" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/Admin.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center;">
        <table width="707" height="34" border="0" align="center" cellpadding="0" cellspacing="1"
            bgcolor="#85BDE2">
            <tr>
                <td height="32" bgcolor="#B0D5EC" class="STYLE14">
                    <div align="left" class="STYLE4">
                        <div align="center">
                            合买佣金设置</div>
                    </div>
                </td>
            </tr>
        </table>
        <table width="707" height="151" border="0" align="center" cellpadding="0" cellspacing="1"
            bgcolor="#85BDE2" style="color: #000000">
            <tr>
                <td>
                    <asp:GridView ID="g" runat="server" Width="707" AutoGenerateColumns="False" BorderStyle="Solid"
                        BorderWidth="1px" CellPadding="0" ForeColor="#333333" GridLines="None">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="彩种" />
                            <asp:TemplateField HeaderText="使用">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbisUsed" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="佣金">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbCommission" runat="server"></asp:TextBox> 【例如：0.02】
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ID"/>
                            <asp:BoundField DataField="Status_ON"></asp:BoundField>
                            <asp:BoundField DataField="Commission"></asp:BoundField>
                        </Columns>
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <shovewebui:shoveconfirmbutton id="btnOK" runat="server" alerttext="确信输入的资料无误，并立即保存资料吗？"
                        backgroupimage="~/Images/Admin/buttbg.gif" borderstyle="None" height="20px"
                        onclick="btnOK_Click" text="修改" width="60px" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
