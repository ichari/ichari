<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invest.aspx.cs" Inherits="Home_Room_Invest" %>

<%@ Register src="~/Home/Room/UserControls/UserMyIcaile.ascx" tagname="UserMyIcaile" tagprefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=_Site.Name %>中奖查询-我的购彩纪录-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function ShowOrHiddenDiv(id) {
            switch (id) {
                case 'divHistory':
                    document.getElementById("hdCurDiv").value = "divHistory";
                    break;
                case 'divReward':
                    document.getElementById("hdCurDiv").value = "divReward";
            }
        }

        function mOver(obj, type) {
            if (type == 1) {
                obj.style.textDecoration = "underline";
            }
            else {
                obj.style.textDecoration = "none";
            }
        }

        function showSameHeight() {
            if (document.getElementById("menu_left").clientHeight < document.getElementById("menu_right").clientHeight) {
                document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
            }
            else {
                if (document.getElementById("menu_right").offsetHeight >=960) {
                    document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
                }
                else {
                    document.getElementById("menu_left").style.height = "960px";
                }
            }
        }

        function clickTabMenu(obj, backgroundImage, tabId) {
            switch (obj.id) {
                case 'tdHistory':
                    document.getElementById("divHistory").style.display = "";
                    document.getElementById("divReward").style.display = "none";
                    document.getElementById("tdHistory").style.background = "url(images/admin_qh_100_1.jpg)";
                    document.getElementById("tdHistory").className = 'afff fs14';
                    document.getElementById("tdReward").style.background = "url(images/admin_qh_100_2.jpg)";
                    document.getElementById("tdReward").className = '';
                    break;
                case 'tdReward':
                    document.getElementById("divHistory").style.display = "none";
                    document.getElementById("divReward").style.display = "";
                    document.getElementById("tdReward").style.background = "url(images/admin_qh_100_1.jpg)";
                    document.getElementById("tdReward").className = 'afff fs14';
                    document.getElementById("tdHistory").className = '';
                    document.getElementById("tdHistory").style.background = "url(images/admin_qh_100_2.jpg)";
                    break;
            }
        }
    </script>
    <link rel="shortcut icon" href="/favicon.ico" />
</head>
<body onload="showSameHeight();" class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
     <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="cpLotto" />
        </div>
        <div id="menu_right">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="border:#E4E4E5 1px solid;">
        <tr>
            <td width="40" height="30" align="right" valign="middle" class="red14">
                <img src="images/icon_6.gif" width="19" height="16" />
            </td>
            <td valign="middle" class="red14" style="padding-left: 10px;">
                我的彩票记录
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="bg1x bgp5 mt10">
        <tr>
            <td width="100" id="tdHistory" align="center" background="images/admin_qh_100_2.jpg"
                onclick="clickTabMenu(this,'url(images/admin_qh_100_1.jpg)','myIcaileTab');ShowOrHiddenDiv('divHistory');"
                style="cursor: pointer;" class="blue12" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)">
                投注记录
            </td>
            <td width="100" id="tdReward" align="center" background="images/admin_qh_100_2.jpg"
                onclick="clickTabMenu(this,'url(images/admin_qh_100_1.jpg)','myIcaileTab');ShowOrHiddenDiv('divReward');"
                style="cursor: pointer;" class="blue12" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)">
                中奖查询
            </td>
            <td width="10" height="32" align="left">
                &nbsp;
            </td>
            <td >
                &nbsp;
            </td>
        </tr>
    </table>
    <div id="myIcaileTab">

                <div id="divHistory" style="display: none;">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#E4E4E5">
                        <tr>
                            <td height="30" colspan="8" align="left" bgcolor="#F3F3F3" style="padding: 5px 10px 5px 2px;">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="100%" class="black12">
                                            <asp:DropDownList ID="ddlLottery" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlPlayType" runat="server">
                                            </asp:DropDownList>
                                            <ShoveWebUI:ShoveConfirmButton ID="btnGo" runat="server" CssClass="btn_s" Text="查询" OnClick="btnGo_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:DataGrid ID="gHistory" runat="server" width="100%" BorderStyle="None" AllowPaging="True"
                        PageSize="30" AutoGenerateColumns="False" CellPadding="0" BackColor="#DDDDDD"
                        OnSortCommand="g_SortCommand" Font-Names="Tahoma" OnItemDataBound="gHistory_ItemDataBound"
                        CellSpacing="1" GridLines="None">
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#E9F1F8" ForeColor="#0066BA" Height="30px"
                            CssClass="blue12_2"></HeaderStyle>
                        <AlternatingItemStyle BackColor="#F8F8F8" />
                        <ItemStyle BackColor="White" BorderStyle="None" Height="30px" HorizontalAlign="Center"
                            CssClass="black12" />
                        <Columns>
                            <asp:BoundColumn DataField="InitiateName" SortExpression="InitiateName" HeaderText="发起人">
                            </asp:BoundColumn>
                             <asp:BoundColumn DataField="LotteryName" SortExpression="LotteryName" HeaderText="彩种">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PlayTypeName" SortExpression="PlayTypeName" HeaderText="玩法">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SchemeShare" SortExpression="SchemeShare" HeaderText="方案份数">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Money" SortExpression="Money" HeaderText="方案金额"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Share" SortExpression="Share" HeaderText="认购份数"></asp:BoundColumn>
                            <asp:BoundColumn DataField="DetailMoney" SortExpression="DetailMoney" HeaderText="认购金额">
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="所占股份"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SchemeWinMoney" SortExpression="WinMoney" HeaderText="方案奖金">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="WinMoneyNoWithTax" SortExpression="WinMoneyNoWithTax"
                                HeaderText="您的奖金"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="是否中奖"></asp:BoundColumn>
                           <%-- <asp:BoundColumn DataField="DateTime" SortExpression="DateTime" HeaderText="发起时间"
                                DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"></asp:BoundColumn>--%>
                                 <asp:HyperLinkColumn DataNavigateUrlField="SchemeID" DataTextField="DateTime" 
                                DataTextFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="发起时间"  DataNavigateUrlFormatString="Scheme.aspx?id={0}"
                                 SortExpression="DateTime" Target="_blank" 
                                Visible="true"></asp:HyperLinkColumn>
                            <asp:BoundColumn HeaderText="状态"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="SchemeID" SortExpression="SchemeID" HeaderText="SchemeID">
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="QuashStatus" SortExpression="QuashStatus"
                                HeaderText="QuashedStatus"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Buyed" SortExpression="Buyed" HeaderText="Buyed">
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="AssureMoney" SortExpression="AssureMoney"
                                HeaderText="AssureMoney"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BuyedShare" SortExpression="BuyedShare"
                                HeaderText="BuyedShare"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPagerHistory" runat="server" width="100%" PageSize="30"
                        ShowSelectColumn="False" DataGrid="gHistory" AlternatingRowColor="#F8F8F8" GridColor="#E0E0E0"
                        HotColor="MistyRose" Visible="False" OnPageWillChange="gPager_PageWillChange"
                        RowCursorStyle="" AllowShorting="true" />
                    <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
                        <tr>
                            <td width="368" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                本页认购金额总计： <span class="red12">
                                    <asp:Label ID="lblPageBuySum" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                            <td width="407" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                认购金额总计： <span class="red12">
                                    <asp:Label ID="lblBuySum" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                本页方案奖金总计： <span class="red12">
                                    <asp:Label ID="lblPageSumWinMoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                方案奖金总计： <span class="red12">
                                    <asp:Label ID="lblSumWinMoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" bgcolor="#FFFEDF" class="black12" style="padding: 5px 10px 5px 10px;">
                                <span class="blue12">说明：</span><br />
                                1、您可以查询您的帐户在一段时间内的所有交易流水。<br />
                                2、如有其他问题，请联系网站客服：<%= _Site.ServiceTelephone %>。
                            </td>
                        </tr>
                    </table>
                </div>
 
                <div id="divReward">
                    <asp:DataGrid ID="gReward" runat="server" width="100%" BorderStyle="None" AllowPaging="True"
                        PageSize="30" AutoGenerateColumns="False" CellPadding="0" BackColor="#DDDDDD"
                        OnSortCommand="g_SortCommand" Font-Names="Tahoma" OnItemDataBound="gReward_ItemDataBound"
                        CellSpacing="1" GridLines="None">
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#E9F1F8" ForeColor="#0066BA" Height="30px"
                            CssClass="blue12_2"></HeaderStyle>
                        <AlternatingItemStyle BackColor="#F8F8F8" />
                        <ItemStyle BackColor="White" BorderStyle="None" Height="30px" HorizontalAlign="Center"
                            CssClass="black12" />
                        <Columns>
                            <asp:BoundColumn DataField="LotteryName" SortExpression="LotteryName" HeaderText="彩种">
                                <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="IsuseName" SortExpression="IsuseName" HeaderText="期号">
                                <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="LotteryNumber" SortExpression="LotteryNumber" HeaderText="投注内容">
                                <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="DetailMoney" SortExpression="DetailMoney" HeaderText="投注金额">
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SchemeWinMoney" SortExpression="SchemeWinMoney" HeaderText="方案奖金">
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="WinMoneyNoWithTax" SortExpression="WinMoneyNoWithTax"
                                HeaderText="我的奖金">
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="WinMoneyNoWithTax" SortExpression="WinMoneyNoWithTax"
                                HeaderText="盈利指数">
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="是否中奖">
                                <HeaderStyle HorizontalAlign="Center" Width="11%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="SchemeID" SortExpression="SchemeID" HeaderText="SchemeID">
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="Silver"
                            Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPager_Reward" runat="server" width="100%" PageSize="30"
                        ShowSelectColumn="False" DataGrid="gReward" AlternatingRowColor="#F8F8F8" GridColor="#E0E0E0"
                        HotColor="MistyRose" Visible="False" OnPageWillChange="gPager_PageWillChange"
                        RowCursorStyle="" AllowShorting="true" />
                    <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
                        <tr>
                            <td width="385" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                奖金派发笔数： <span class="red12">
                                    <asp:Label ID="lblRewardCount" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                奖金派发收入合计： <span class="red12">
                                    <asp:Label ID="lblRewardMoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#FFFEDF" class="black12" style="padding: 5px 10px 5px 10px;">
                                <span class="blue12">说明：</span><br />
                                1、您可以查询您的帐户在一段时间内的所有交易流水。<br />
                                2、如果你已经充值，银行账户钱扣了，而您的账户还没有加上，请点击<span class="blue12_2">在线客服</span>告诉我们，我们将第一时间为您处理！<br />
                                3、如有其他问题，请联系网站客服：<%= _Site.ServiceTelephone %>。
                            </td>
                        </tr>
                    </table>
                </div>

    </div>
 
    <input type="hidden" id="hdCurDiv" runat="server" value="divReward" />
      </div>
    </div>

</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>

    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
    <script type="text/javascript">
        <%=Script %>
    </script>

    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
