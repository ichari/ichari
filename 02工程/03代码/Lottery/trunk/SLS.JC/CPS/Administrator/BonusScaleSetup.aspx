<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BonusScaleSetup.aspx.cs" Inherits="Cps_Administrator_BonusScaleSetup" %>
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
    
     <table width="100%">
        <tr>
            <td width="40px">
         
            </td>
            <td width="120px">
         
            </td>
            <td width="120px">
          
            </td>
            <td width="120px">

            </td>
            <td >
            </td>
        </tr>
        <tr>
            <td colspan="5">                           
                    <asp:DataGrid ID="g" runat="server"  BorderStyle="None" AllowSorting="True"
	                    AllowPaging="True" PageSize="30" AutoGenerateColumns="False" CellPadding="2"
	                    BackColor="#CCCCCC" Font-Names="宋体" CellSpacing="1"
	                    GridLines="None" OnItemCommand="g_ItemCommand" DataKeyField="LotteryID" 
                        onitemdatabound="g_ItemDataBound">
	                    <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
	                    <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
		                    BackColor="#E9F1F8" Height="25px" ></HeaderStyle>
	                    <ItemStyle Height="21px"></ItemStyle>
	                    <Columns>
                    		
		                    <asp:BoundColumn DataField="LotteryID" HeaderText="彩种ID">
			                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
		                    </asp:BoundColumn>

		                    <asp:BoundColumn DataField="LotteryName" HeaderText="彩种" >
			                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
		                    </asp:BoundColumn>
		               
                            <asp:TemplateColumn HeaderText="代理商返点比例">
			                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
			                    <ItemTemplate>
				                    <asp:TextBox ID="tbUnionBonusScale" runat="server" Width="90%" ></asp:TextBox>
			                    </ItemTemplate>
		                    </asp:TemplateColumn>
		                    
		                    <asp:TemplateColumn HeaderText="推广员返点比例">
			                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
			                    <ItemTemplate>
				                    <asp:TextBox ID="tbCommenderBonusScale" runat="server" Width="90%" ></asp:TextBox>
			                    </ItemTemplate>
		                    </asp:TemplateColumn>
		                    
                             <asp:ButtonColumn  CommandName="modify"  Text="修改" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />                
	                    </Columns>
	                    <PagerStyle Visible="False"></PagerStyle>
                    </asp:DataGrid>
                    <div style="display:none">
                    <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="20" Visible="false"
                        ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#E0E0E0"
                        HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore" SelectRowColor="#FF9933" />
                    </div>
             </td>
         </tr>        
        </table>         
     
    </form>
    </body>
</html>

