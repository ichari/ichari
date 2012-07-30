<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BonusScaleView.aspx.cs" Inherits="CPS_Administrator_Admin_BonusScaleView" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../Style/css.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
      
         <br />
         <span style=" font-size: 14px; font-weight: bold;">
         CPS商家>>佣金返点比例</span><br />
         <table>
            <tr>
                
                <td valign="top">
                     <br />
                     
                </td>
                <td valign="top">
                    <asp:DataGrid ID="g" runat="server"  BorderStyle="None" AllowSorting="True"
	                    AllowPaging="True" PageSize="30" AutoGenerateColumns="False" CellPadding="2"
	                    BackColor="#CCCCCC" Font-Names="宋体" CellSpacing="1"
	                    GridLines="None" DataKeyField="LotteryID">
	                    <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
	                    <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
	                    <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
		                    BackColor="#E9F1F8" Height="25px" ></HeaderStyle>
	                    <ItemStyle Height="21px"></ItemStyle>
	                    <Columns>
                    		
		                    <asp:BoundColumn DataField="LotteryID" HeaderText="彩种ID" Visible="false">
			                    <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
		                    </asp:BoundColumn>

		                    <asp:BoundColumn DataField="LotteryName" HeaderText="彩种" >
			                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" Width="300" ></ItemStyle>
		                    </asp:BoundColumn>
		                    <asp:BoundColumn DataField="TypeName" HeaderText="类别"  Visible="false">
			                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
		                    </asp:BoundColumn>
		                    
                            <asp:BoundColumn DataField="BonusScale" HeaderText="返点比例">
			                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			                    <ItemStyle HorizontalAlign="Center" Width="300"></ItemStyle>
		                    </asp:BoundColumn>
		                    
	                    </Columns>
	                    <PagerStyle Visible="False"></PagerStyle>
                    </asp:DataGrid>
                    <div style="display:none">
                    <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="20"
                        ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#E0E0E0"
                        HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore" SelectRowColor="#FF9933" /></div>
                </td>
            </tr>
        </table>
        <div align="center">  
            <a href="AdvermentLink.aspx"><img src="../../Images/return.gif" border="0"/></a>
        </div>
                    
    </form>

    </body>
</html>

