<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CpsTry.aspx.cs" Inherits="Cps_Administrator_CpsTry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../../Style/main.css" type="text/css" rel="stylesheet" />
    <style>
        body
        {
            background-image:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 0px; border: 1px solid #CCCCCC; background-color: #E9F1F8; margin-top: 5px; margin-bottom: 5px;">
            <div  style=" padding: 3px;width:100%; height:25px; font-size: 14px; font-weight: bold; color: #455C7E; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-co; text-align: center; border-bottom-color: #C0C0C0;">代理商申请一览表</div> 
             <table cellpadding="0" cellspacing="0">
                <tr>
                    <td height="32px">
                        &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="tbKeyWord" runat="server" Height="16px" Text="请输入代理商ID" onfocus="this.value=''"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnSearch" runat="server" Text="搜索" Height="22px" Width="40px" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                            onclick="btnSearch_Click"  />
                    </td>
                </tr>
             </table>
        </div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center">
                    <asp:DataGrid ID="g" runat="server" CellPadding="0" BackColor="White" 
                        BorderWidth="2px" BorderStyle="None" BorderColor="#E0E0E0" Font-Names="宋体" 
                        PageSize="20" AllowSorting="True" AutoGenerateColumns="False" 
                        AllowPaging="True" Width="100%" 
                        onitemcommand="g_ItemCommand">
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" 
                            VerticalAlign="Middle" BackColor="#E9F1F8" Height="20px"></HeaderStyle>
                        <ItemStyle Height="20px" />
                        <Columns>
                            <asp:BoundColumn DataField="Count" SortExpression="Count" HeaderText="序号">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:HyperLinkColumn DataNavigateUrlField="id" DataTextField="Name" HeaderText="代理商ID" DataNavigateUrlFormatString="CpsTryHandle.aspx?id={0}" SortExpression="Name"></asp:HyperLinkColumn>                       
                            <asp:BoundColumn DataField="DateTime" SortExpression="DateTime" HeaderText="申请时间" DataFormatString="{0:yyyy-MM-dd}">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                             <asp:BoundColumn DataField="CpsTrysName" SortExpression="CpsTrysName" HeaderText="网站名称">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="Url" SortExpression="Url" HeaderText="网址">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                             <asp:BoundColumn DataField="Telephone" SortExpression="Telephone" HeaderText="电话">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Mobile" SortExpression="Mobile" HeaderText="手机号码">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                          
                          <asp:TemplateColumn HeaderText="佣金比例">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                           <asp:TextBox ID="tbBonus" runat="server" onkeyup="value=value.replace(/[^\d\.]/g,'');" Width="50"/>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="操作">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" ForeColor="Red" ></ItemStyle>
                        <ItemTemplate>
                            <ShoveWebUI:ShoveLinkButton ID="btnDetaile" runat="server" CommandName="HandleTry" ForeColor="Red">
                                处理申请</ShoveWebUI:ShoveLinkButton>&nbsp;
                            <ShoveWebUI:ShoveLinkButton ID="ShoveLinkButton1" runat="server" CommandName="NoAccept" ForeColor="Red">
                                审核不通过</ShoveWebUI:ShoveLinkButton>&nbsp;
                            <asp:LinkButton ID="ShoveLinkButton2" runat="server" CommandName="Deletes" ForeColor="Red">
                                删除记录</asp:LinkButton>&nbsp;
                        </ItemTemplate>
                    </asp:TemplateColumn>
                            <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC"></PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="Linen" GridColor="#E0E0E0" HotColor="MistyRose" Visible="False" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore"></ShoveWebUI:ShoveGridPager>
                </td>
            </tr>
        </table>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
