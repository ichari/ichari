<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScoringDetail.aspx.cs" Inherits="Home_Room_ScoringDetail" %>

<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我的积分明细-我的积分-<%= _Site.Name %>！</title>
    <meta name="description" content="<%= _Site.Name %> 是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function mOver(obj, type) {
            if (type == 1) {
                obj.style.textDecoration = "underline";
                obj.style.color = "#FF0065";
            }
            else {
                obj.style.textDecoration = "none";
                obj.style.color = "#226699";
            }
        }

        function showSameHeight() {
            if (document.getElementById("menu_left").clientHeight < document.getElementById("menu_right").clientHeight) {
                document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
            }
            else {
                if (document.getElementById("menu_right").offsetHeight >= 860) {
                    document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
                }
                else {
                    document.getElementById("menu_left").style.height = "860px";
                }
            }
        }
    </script>
</head>
<body onload="showSameHeight();"  class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="ptMyPoints" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" style="border:#E4E4E5 1px solid;" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="40" height="30" align="right" valign="middle" class="red14">
                        <img src="images/icon_13.gif" width="19" height="16" />
                    </td>
                    <td valign="middle" class="red14" style="padding-left: 10px;">
                        我的积分
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" class="bg1x bgp5" cellspacing="0" cellpadding="0" background="images/zfb_left_bg_2.jpg" style="margin-top: 10px;">
                <tr>
                    <td width="120" id="tdHistory" align="center" background="images/admin_qh_100_1.jpg" style="cursor: pointer;" class="afff fs14" >
                        我的积分明细
                    </td>
                    <td width="10" height="32" align="left">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%"">
                <tbody style="padding:10px 20px 10px 20px">
                    <tr style="background-color: #ffffff;">
                        <td class="td6" style="height: 65px" valign="middle" align="left">
                            <p><font face="宋体" color="#ff0000">&nbsp;&nbsp;&nbsp;&nbsp; 注：为了减轻服务器的压力，此数据被缓存几分钟。即：你的最后操作的充值、冻结、解冻、投注等信息，如果没有完全显示出来，几分钟后重新打开本页都将会被正确显示。</font></p>
                            <font face="宋体">如果您充值了，银行账户钱扣了，而您的账户还没有加上，请拨打<% =_Site.Telephone %>告诉我们，我们将第一时间为您处理！</font>
                        </td>
                    </tr>
                    <tr style="background-color: #ffffff;">
                        <td style="height: 35px">
                            <font face="宋体">&nbsp;请选择开始时间进行查询
                                <asp:DropDownList ID="ddlYear" runat="server" Width="88px">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:DropDownList ID="ddlMonth" runat="server" Width="80px">
                                </asp:DropDownList>
                                &nbsp;
                                <ShoveWebUI:ShoveConfirmButton ID="btnGO" runat="server" Text="查询" OnClick="btnGO_Click"
                                    BackgroupImage="Images/buttbg.gif" Style="font-size: 9pt; cursor: hand;"
                                    Height="20px" Width="60px" /></font>
                        </td>
                    </tr>
                    <tr style="background-color: #ffffff;">
                        <td valign="top" align="center">
                            <asp:DataGrid ID="g" runat="server" Width="100%" BorderStyle="None" AllowPaging="false"
                                PageSize="25" AutoGenerateColumns="False" CellPadding="0" BackColor="#DDDDDD"
                                Font-Names="Tahoma" BorderWidth="2px" BorderColor="#E0E0E0" OnItemDataBound="g_ItemDataBound"
                                GridLines="None" CellSpacing="1">
                                <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                                <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                                <HeaderStyle HorizontalAlign="Center" BackColor="#E9F1F8" ForeColor="#0066BA" Height="30px"
                                    CssClass="blue12_2"></HeaderStyle>
                                <AlternatingItemStyle BackColor="#F8F8F8" />
                                <ItemStyle BackColor="White" BorderStyle="None" Height="30px" HorizontalAlign="Center"
                                    CssClass="black12" />
                                <Columns>
                                    <asp:BoundColumn DataField="DateTime" HeaderText="时间">
                                        <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Memo" HeaderText="摘要">
                                        <HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Left" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ScoringAdd" HeaderText="增加">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ScoringSub" HeaderText="减少">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Balance" HeaderText="总积分">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="SchemeID" HeaderText="SchemeID"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                                </PagerStyle>
                            </asp:DataGrid>
                            <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="30"
                                Visible="False" ShowSelectColumn="False" DataGrid="g" AllowShorting="False" AlternatingRowColor="Linen"
                                GridColor="#E0E0E0" HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>
<asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
</form>
</body>
</html>
