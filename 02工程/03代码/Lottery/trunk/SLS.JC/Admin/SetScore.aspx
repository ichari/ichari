<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetScore.aspx.cs" Inherits="Admin_SetScore" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table id="tbWinScoreScale" cellspacing="0" cellpadding="0" width="80%" align="center"
            border="0">
            <tr>
                <td style="height: 30px;"> 请选择
                    <asp:DropDownList ID="ddlLottery" runat="server" Width="140px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hfID" runat="server" Value="-1" />
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tr>
                            <td height="35px">
                                中奖额度：<asp:TextBox ID="txtMoney" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                             <td height="35px">
                                积分比例：<asp:TextBox ID="txtScoreScale" runat="server"></asp:TextBox>(例如:0.03)</td>
                        </tr>
                        <tr>
                             <td height="35px">
                                <ShoveWebUI:ShoveConfirmButton ID="btnOK" runat="server" Width="60px" Height="20px"
                                    BackgroupImage="../Images/Admin/buttbg.gif" Text="确定" AlertText="确定添加此金额吗？"
                                    OnClick="btnOK_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;<ShoveWebUI:ShoveConfirmButton ID="btnCancel" runat="server"
                                    Width="60px" Height="20px" BackgroupImage="../Images/Admin/buttbg.gif" Text="取消"
                                    OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <hr size="1" style="border:1px solid #cccccc;" />
                </td>
            </tr>
            <tr>
                <td align="center" class="STYLE14" height="50">
                    中奖送积分比例详细
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:DataGrid ID="g" runat="server" Width="100%" AutoGenerateColumns="False" CellPadding="1"
                        BackColor="White" BorderWidth="2px" BorderStyle="None" BorderColor="#E0E0E0"
                        Font-Names="宋体" AllowSorting="True" OnItemCommand="g_ItemCommand">
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
                            BackColor="Silver"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="WinTypesName" SortExpression="WinTypesName" HeaderText="奖金等级">
                                <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="WinMoney" SortExpression="WinMoney" DataFormatString="{0:F2}" HeaderText="最大奖金额度">
                                <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ScoreScale" SortExpression="ScoreScale" HeaderText="积分比例">
                                <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="编辑">
                                <HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit">修改</asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="btnDel" runat="server" CommandName="Del" AlertText="您确信要删除此条广告图片吗？">删除</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="ID" SortExpression="ID" Visible="false"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
