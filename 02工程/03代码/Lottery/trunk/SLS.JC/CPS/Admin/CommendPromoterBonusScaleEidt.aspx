<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommendPromoterBonusScaleEidt.aspx.cs" Inherits="CPS_Admin_CommendPromoterBonusScaleEidt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../../Style/main.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
         <asp:HiddenField ID="hfCpsID" runat="server" />
         <asp:HiddenField ID="hfOwnerUserID" runat="server" />
         
         <br />
         <span style=" font-size: 14px; font-weight: bold;">
         我推广的商家列表>><asp:Label ID="lblCpsName" runat="server" Text="lblCpsName" ForeColor="#CC0000"></asp:Label>
         佣金返点比例设置
         </span><br />
         <table>
            <tr>
                
                <td valign="top">
                   
                </td>
                <td valign="top">
                    <asp:DataGrid ID="g" runat="server"  BorderStyle="None" AllowSorting="True"
	                    AllowPaging="True" PageSize="30" AutoGenerateColumns="False" CellPadding="2"
	                    BackColor="#CCCCCC" Font-Names="宋体" CellSpacing="1"
	                    GridLines="None" OnItemCommand="g_ItemCommand" DataKeyField="LotteryID" 
                        onitemdatabound="g_ItemDataBound">
	                    <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
	                    <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
	                    <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
		                    BackColor="#E9F1F8" Height="25px" ></HeaderStyle>
	                    <ItemStyle Height="21px"></ItemStyle>
	                    <Columns>
                    		
		                    <asp:BoundColumn DataField="LotteryID" HeaderText="彩种ID">
			                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
		                    </asp:BoundColumn>

		                    <asp:BoundColumn DataField="LotteryName" HeaderText="彩种">
			                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
		                    </asp:BoundColumn>
                          
                            <asp:TemplateColumn HeaderText="返点比例">
			                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
			                    <ItemTemplate>
				                    <asp:TextBox ID="tbBonusScale" runat="server" Width="90%" ></asp:TextBox>
			                    </ItemTemplate>
		                    </asp:TemplateColumn>

                             <asp:ButtonColumn  CommandName="modify"  Text="修改"  HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center"/>                
		                 

		                    <asp:BoundColumn Visible="False" DataField="LotteryID"></asp:BoundColumn>
	                    </Columns>
	                    <PagerStyle Visible="False"></PagerStyle>
                    </asp:DataGrid>
                    <div style="display:none">
                    <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="20"
                        ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#E0E0E0"
                        HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore" SelectRowColor="#FF9933" />
                </div></td>
            </tr>
            <tr>
                
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    统一修改此商家所有彩种返点比例:<asp:TextBox ID="tbAllScale" runat="server"></asp:TextBox>
                    &nbsp;<asp:Button ID="btnModifyAll" runat="server" Text="统一修改" 
                        Width="89px" onclick="btnModifyAll_Click" />
                    <br />
                   <span style="color: #CC0000">提示： 正确格式:0.03,并且不能超过<%=_Site.Name %>分配给自己的返点比例．</span> 
                   </td>
            </tr>
        </table>

                    
    </form>

    </body>
</html>

