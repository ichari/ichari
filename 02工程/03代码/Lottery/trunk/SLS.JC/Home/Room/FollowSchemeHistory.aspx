<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FollowSchemeHistory.aspx.cs" Inherits="Home_Room_FollowSchemeHistory" %>
<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我的跟单-我的购彩纪录-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function ShowOrHiddenDiv(id) {
            var arrCells = new Array('tdMyFollowSchemes', 'tdSetMyFollowSchemes', 'tdWhoFollowSchemes');
            var divName = id.substring(3);
            for (var i = 0; i < arrCells.length; i++) {
                if (i == 0) {
                    divName = 'td' + divName;
                }

                if (arrCells[i] == divName) {
                    document.getElementById(arrCells[i]).style.fontWeight = "bold";
                }
                else {
                    document.getElementById(arrCells[i]).style.fontWeight = "normal";
                }
            }
            switch (id) {
                case 'divMyFollowSchemes':
                    divMyFollowSchemes.display = "block";
                    divSetMyFollowSchemes.display = "none";
                    divWhoFollowSchemes.display = "none";
                    document.getElementById("hdCurDiv").value = "divMyFollowSchemes";
                    break;
                case 'divSetMyFollowSchemes':
                    divSetMyFollowSchemes.display = "block";
                    divMyFollowSchemes.display = "none";
                    divWhoFollowSchemes.display = "none";
                    document.getElementById("hdCurDiv").value = "divSetMyFollowSchemes";
                    break;
                case 'divWhoFollowSchemes':
                    divWhoFollowSchemes.display = "block";
                    divSetMyFollowSchemes.display = "none";
                    divMyFollowSchemes.display = "none";
                    document.getElementById("hdCurDiv").value = "divWhoFollowSchemes";
                    break;
            }
////            parent.document.getElementById("mainFrame").style.height = document.getElementById(id).scrollHeight + 120;
        }

        function showDialog(url) {
            var dialogWidth = 550;
            var dialogHeight = 250;
            var w = window.showModalDialog(url + "&r=" + Math.random(), "", "dialogWidth:" + dialogWidth + "px;dialogHeight=" + dialogHeight + "px;center:yes;status:no;help:no;");

            if (w == "1") {
                __doPostBack('btnSearch', '');
            }else{
                return false;
            }
        }

        /*function clickTabMenu(obj, backgroundImage, tabId) {
            var tabMenu = obj.offsetParent.cells;
            var tabContent = document.getElementById(tabId).cells;
            for (var i = 0; i < tabMenu.length; i++) {
                if (obj == tabMenu[i]) {
                    obj.style.backgroundImage = backgroundImage;
                    tabContent[i].style.display = "";
                }
                else {
                    tabMenu[i].style.backgroundImage = "";
                    tabContent[i].style.display = "none";
                }
            }
        }*/

        function clickTabMenu(n) {
            var tba=['tdMyFollowSchemes','tdSetMyFollowSchemes','tdWhoFollowSchemes']
            var tec=['divMyFollowSchemes','divSetMyFollowSchemes','divWhoFollowSchemes']
            for(var i=0; i<tba.length; i++){
             document.getElementById(tba[i]).style.background = "url(images/admin_qh_100_2.jpg)";
             document.getElementById(tba[i]).className = '';
             document.getElementById(tec[i]).style.display = "none";
            }
            document.getElementById(tba[n]).style.background = "url(images/admin_qh_100_1.jpg)";
            document.getElementById(tec[n]).style.display = "";
            document.getElementById(tba[n]).className = 'afff fs14';

        
        }
        function mOver(obj, type) {
            if (type == 1) {
                obj.style.textDecoration = "underline";
            }else{
                obj.style.textDecoration = "none";
            }
        }

        function showSameHeight() {
            if (document.getElementById("menu_left").clientHeight < document.getElementById("menu_right").clientHeight) {
                document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
            }else{
                if (document.getElementById("menu_right").offsetHeight >= 960) {
                    document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
                }else {
                    document.getElementById("menu_left").style.height = "960px";
                }
            }
        }

        function isCancelFollowScheme() {
            return confirm("您确认取消此定制跟单吗？");
        }
    </script>
    <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body onload="showSameHeight();" class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="cpFollow" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" style="border:#E4E4E5 1px solid;" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="40" height="32" align="right" valign="middle" class="red14">
                        <img src="images/icon_6.gif" width="19" height="16" />
                    </td>
                    <td valign="middle" class="red14" style="padding-left: 10px;">
                        我的彩票记录
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" class="bg1x bgp5" cellpadding="0" style="margin-top: 10px;">
                <tr>
                    <td width="100" id="tdMyFollowSchemes" class="afff fs14" background="images/admin_qh_100_1.jpg" align="center"
                         onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" onclick="clickTabMenu(0);"
                        style="cursor: pointer;">
                        我的跟单记录
                    </td>

                    <td width="100" id="tdSetMyFollowSchemes" align="center" 
                        onmouseover="mOver(this,1)" onmouseout="mOver(this,2)" 
                        onclick="clickTabMenu(1);"
                        style="cursor: pointer;">
                        自动跟单设置
                    </td>

                    <td width="100" id="tdWhoFollowSchemes" align="center" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)"
                        onclick="clickTabMenu(2);"
                        o style="cursor: pointer;">
                        合买跟单
                    </td>
                    <td>
                        &nbsp;
                    </td>
                   <td width="20" height="32" align="left">
                        &nbsp;
                    </td>
                    <td width="168" class="black12">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="1" colspan="9" bgcolor="#FFFFFF">
                    </td>
                </tr>
                <tr>
                    <td height="2" colspan="9" bgcolor="#6699CC">
                    </td>
                </tr>
            </table>
            <div id="myIcaileTab">
                        <div id="divMyFollowSchemes">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-left: 1px solid #DDDDDD;
                                border-right: 1px solid #DDDDDD;">
                                <tr>
                                    <td height="30" colspan="8" align="left" bgcolor="#F3F3F3" style="padding: 5px 10px 5px 2px;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="100%" class="black12" style="padding-left: 10px;">
                                                    <asp:DropDownList ID="ddlLottery" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                    <asp:DropDownList ID="ddlPlayType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPlayType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="g" runat="server" width="100%" BorderStyle="None" AllowPaging="True"
                                PageSize="30" AutoGenerateColumns="False" CellPadding="0" BackColor="#DDDDDD"
                                OnSortCommand="g_SortCommand" Font-Names="Tahoma" OnItemDataBound="g_ItemDataBound"
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
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="PlayTypeName" SortExpression="PlayTypeName" HeaderText="方案类别">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="方案标题">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="SchemeShare" SortExpression="SchemeShare" HeaderText="方案份数">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Money" SortExpression="Money" HeaderText="方案金额">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Share" SortExpression="Share" HeaderText="认购份数">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DetailMoney" SortExpression="DetailMoney" HeaderText="认购金额">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="所占股份">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="SchemeWinMoney" SortExpression="WinMoney" HeaderText="方案奖金">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="WinMoneyNoWithTax" SortExpression="WinMoneyNoWithTax"
                                        HeaderText="您的奖金">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DateTime" SortExpression="DateTime" HeaderText="发起时间"
                                        DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="状态">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
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
                                        <asp:BoundColumn Visible="False" DataField="Memo" SortExpression="Memo"
                                        HeaderText="Memo"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                                </PagerStyle>
                            </asp:DataGrid>
                            <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" width="100%" PageSize="30" ShowSelectColumn="False"
                                DataGrid="g" AlternatingRowColor="#F8F8F8" GridColor="#E0E0E0" HotColor="MistyRose"
                                Visible="False" OnPageWillChange="gPager_PageWillChange" RowCursorStyle="" AllowShorting="true" />
                            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
                                <tr>
                                    <td width="368" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                        本页认购金额总计： <span class="red12">
                                            <asp:Label ID="lblPageBuyMoney" runat="server" Text=""></asp:Label>
                                        </span>
                                    </td>
                                    <td width="407" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                        认购金额总计： <span class="red12">
                                            <asp:Label ID="lblTotalBuyMoney" runat="server" Text=""></asp:Label>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                        本页方案奖金总计： <span class="red12">
                                            <asp:Label ID="lblPageReward" runat="server" Text=""></asp:Label>
                                        </span>
                                    </td>
                                    <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                        方案奖金总计： <span class="red12">
                                            <asp:Label ID="lblTotalReward" runat="server" Text=""></asp:Label>
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
 
                        <div id="divSetMyFollowSchemes" style="display:none;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-left: 1px solid #DDDDDD;
                                border-right: 1px solid #DDDDDD;">
                                <tr>
                                    <td height="30" colspan="8" align="left" bgcolor="#F3F3F3" style="padding: 5px 10px 5px 2px;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="100%" class="black12" style="padding-left: 10px;">
                                                    <asp:DropDownList ID="ddlLotterySet" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                    <asp:DropDownList ID="ddlPlayTypeSet" runat="server">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp; 好友用户名：
                                                    <asp:TextBox ID="TxtName" runat="server" CssClass="in_text" value="" size="25"></asp:TextBox>
                                                    <ShoveWebUI:ShoveConfirmButton ID="btnSearch" runat="server" CssClass="btn_s" Text="查询" OnClick="btnSearch_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="gSetFollowScheme" runat="server" width="100%" BorderStyle="None"
                                AllowSorting="True" AllowPaging="True" PageSize="22" AutoGenerateColumns="False"
                                CellPadding="2" BackColor="#DDDDDD" Font-Names="Tahoma" OnItemDataBound="gSetFollowScheme_ItemDataBound"
                                CellSpacing="1" OnItemCommand="g_ItemCommand" GridLines="None" BorderColor="#E0E0E0"
                                OnSortCommand="g_SortCommand" BorderWidth="2px">
                                <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                                <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#0066BA" VerticalAlign="Middle"
                                    BackColor="#E9F1F8" Height="25px"></HeaderStyle>
                                <ItemStyle Height="21px"></ItemStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="Name" HeaderText="好友">
                                        <HeaderStyle HorizontalAlign="Center" Width="16%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="战绩">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" ForeColor="Black"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="已定制/可定制">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="所有跟单人">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="我的定制状态">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="定制自动跟单">
                                        <ItemTemplate>
                                            <asp:Label ID="lbEdit" runat="server"></asp:Label>
                                            <asp:LinkButton runat="server" ID="Cancel" CommandName="Cancel" Visible="false" Text="取消"></asp:LinkButton>
                                          <%--  <ShoveWebUI:ShoveLinkButton ID="Cancel" runat="server" CommandName="Cancel" AlertText="您确认取消此定制跟单吗?"
                                                Visible="false">取消</ShoveWebUI:ShoveLinkButton>--%>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle Visible="False"></PagerStyle>
                            </asp:DataGrid>
                            <ShoveWebUI:ShoveGridPager ID="gPagerSetFollowScheme" runat="server" width="100%"
                                PageSize="30" ShowSelectColumn="False" DataGrid="gSetFollowScheme" AlternatingRowColor="#F8F8F8"
                                GridColor="#E0E0E0" HotColor="MistyRose" Visible="False" OnPageWillChange="gPager_PageWillChange"
                                RowCursorStyle="" AllowShorting="true" />
                        </div>

                        <div id="divWhoFollowSchemes" style="display:none;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-left: 1px solid #DDDDDD;
                                border-right: 1px solid #DDDDDD;">
                                <tr>
                                    <td height="30" colspan="8" align="left" bgcolor="#F3F3F3" style="padding: 5px 10px 5px 2px;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="100%" class="black12" style="padding-left: 10px;">
                                                    <asp:DropDownList ID="ddlWhoLottery" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                    <asp:DropDownList ID="ddlWhoPlaytype" runat="server">
                                                    </asp:DropDownList>
                                                    <ShoveWebUI:ShoveConfirmButton ID="btnFollowScheme" runat="server" OnClick="btnFollowScheme_Click" CssClass="btn_s" Text="查询" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="gWhoFollowSchemes" runat="server" width="100%" BorderStyle="None"
                                AllowSorting="True" AllowPaging="True" PageSize="22" AutoGenerateColumns="False"
                                CellPadding="2" BackColor="#DDDDDD" Font-Names="Tahoma" CellSpacing="1" GridLines="None"
                                BorderColor="#E0E0E0" OnSortCommand="g_SortCommand" BorderWidth="2px">
                                <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                                <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#0066BA" VerticalAlign="Middle"
                                    BackColor="#E9F1F8" Height="25px"></HeaderStyle>
                                <ItemStyle Height="21px"></ItemStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="用户名">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%#GetUserName(Eval("UserID"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="彩种">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%#GetLottery(Eval("LotteryID"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="玩法">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%#GetPlayType(Eval("PlayTypeID"))%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="跟单最小金额">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%#Shove._Convert.StrToDouble(Eval("MoneyStart").ToString(),0).ToString("N")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="跟单最大金额">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%#Shove._Convert.StrToDouble(Eval("MoneyEnd").ToString(), 0).ToString("N")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="定制时间">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%#Eval("DateTime")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle Visible="False"></PagerStyle>
                            </asp:DataGrid>
                            <ShoveWebUI:ShoveGridPager ID="gPageWhoFollowSchemes" runat="server" width="100%"
                                PageSize="30" ShowSelectColumn="False" DataGrid="gWhoFollowSchemes" AlternatingRowColor="#F8F8F8"
                                GridColor="#E0E0E0" HotColor="MistyRose" Visible="False" RowCursorStyle="" AllowShorting="true" />
                        </div>

            </div>
            <input type="hidden" id="hdCurDiv" runat="server" value="divMyFollowSchemes" />
        </div>
    </div>

</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>




    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>

    <script type="text/javascript">

      /*  var curDiv = document.getElementById("hdCurDiv").value;

        switch (curDiv) {
            case 'divMyFollowSchemes':
                clickTabMenu(document.getElementById("tdMyFollowSchemes"), "url(images/admin_qh_100_1.jpg)", "myIcaileTab");
                break;
            case 'divSetMyFollowSchemes':
                clickTabMenu(document.getElementById("tdSetMyFollowSchemes"), "url(images/admin_qh_100_1.jpg)", "myIcaileTab");
                break;
            case 'divWhoFollowSchemes':
                clickTabMenu(document.getElementById("tdWhoFollowSchemes"), "url(images/admin_qh_100_1.jpg)", "myIcaileTab");
                break;
            default:
                curDiv = "divMyFollowSchemes";
                clickTabMenu(document.getElementById("tdMyFollowSchemes"), "url(images/admin_qh_100_1.jpg)", "myIcaileTab");
                break;
        }
        ShowOrHiddenDiv(curDiv);*/
    </script>

    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
