<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChallengeSchemeList.aspx.cs"
    Inherits="Challenge_ChallengeSchemeList"%>

<%@ Register Src="../Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="../Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>擂台方案列表</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />    
    <link rel="stylesheet" type="text/css" href="Style/ring.css" />
    <link href="Style/pagination.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <form id="buy_form" runat="server">
    <uc1:WebHead ID="WebHead1" runat="server" />
    <div class="wrap">
        <!--擂台main-->
        <div class="main_ring">
            <div class="explain_nav">
                <ul>
                    <li class="w88"><a href="Default.aspx">参与竞猜</a></li>
                    <li class="curr"><a href="ChallengeSchemeList.aspx">方案列表</a></li>
                    <li class="w88"><a href="ChallengeRanking.aspx">排行榜</a></li>
                    <li class="w88"><a href="ChallengeHelp.aspx">玩法规则</a></li>
                </ul>
            </div>
            <div class="explain_cont">
                <h1 class="explain_ico">
                </h1>
                <dl>
                    1、用户可以选择2场单选进行投注，用户凭唯一真实姓名每天仅能提交1个竞猜方案。<br />
                    2、用户参与竞猜前，需绑定手机号码，为空或不具唯一性均不能提交竞猜方案。<br />
                    3、赛程根据体彩中心的竞彩胜平负玩法设定，擂台竞猜按日进行。<br />
                    4、每月积分排名前15名的用户奖获得网站提供的现金奖励。&nbsp;&nbsp;<span><a href="/Challenge/ChallengeHelp.aspx">[具体玩法]</a></span><br />
                    <span class="c_999">备注：用户登录后可进入任何擂台方案页对投注选项进行修改，并通过添加投注功能，发起真实的投注方案</span>
                    <div style="width:100%; height:5px"></div>    
                </dl>
                <div class="cl">
                </div>
            </div>
            <!--竞猜对阵-->
            <div class="ringtab">
                <div class="ringtab_nav">
                    <h2>
                        擂台方案</h2>
                </div>
                <div class="topring_search">
                    <table width="100%" border="0">
                        <tbody>
                            <tr valign="middle">
                                <td height="22" colspan="3" class="dtd" style="text-align: left">
                                    方案搜索&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;日期：
                                    <asp:TextBox runat="server" ID="tbYser" Height="20px" name="tbYser" onFocus="WdatePicker({el:'tbYser',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" onblur="checkTime()"></asp:TextBox>
                                    &nbsp;&nbsp; 用户名：
                                    <asp:TextBox runat="server" ID="tbUserName" Height="18px" Style="width: 130px;" onfocus="if(this.value=='输入用户名')this.value='';"
                                        onblur="if (this.value=='')this.value = '输入用户名';"></asp:TextBox>
                                    <asp:Button ID="btnFile" runat="server" class="btn_ringso" Text=" 查 询 " OnClick="btnFile_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="ringtab_cont">
                    <table class="table_c1" cellspacing="1" id="SchemeList" cellpadding="0" width="736" align="center"
                        border="0">
                        <thead>
                            
                            <tr class="tre">
                                <td width="5%">
                                    序
                                </td>
                                <td width="16%">
                                    用户名
                                </td>
                                <td width="13%">
                                    选择场次
                                </td>
                                <td>
                                    方案内容
                                </td>
                                <td width="13%">
                                    过关方式
                                </td>
                                <td width="10%">
                                    详情
                                </td>
                            </tr>
                                                           
                        </thead>
                    </table>
                    <div id="divload" style="top: 50%; right: 50%; padding: 0px; margin: 0px; z-index: 999;clear:both;">
                       <center><img src="../Images/ajax-loader.gif" alt=""/></center>
                    </div>
                    <div  id="EachPageNum" class="AspNetPager" style="width: 98%; text-align: center;">
                        <div id="Pagination" class="yahoo">
                        </div>
                    </div>
                    <div class="cl">
                    </div>
                </div>
            </div>
            <!--擂台最新方案-->
            <!--擂台当天赛事热门投注-->
            <div class="ringtab">
                <div class="ringtab_nav">
                    <h2>
                        擂台当天赛事热门投注</h2>
                </div>
                <div class="ringtab_cont">
                    <table class="foyt" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                        <tbody>
                            <tr class="tre">
                                <td width="10%">
                                    编号
                                </td>
                                <td width="10%">
                                    赛事
                                </td>
                                <td width="12%">
                                    主队
                                </td>
                                <td width="10%">
                                    让球
                                </td>
                                <td width="12%">
                                    客队
                                </td>
                                <td width="10%">
                                    选项
                                </td>
                                <td>
                                    比例
                                </td>
                            </tr>
                            <asp:Repeater runat="server" ID="gHotBet">
                                <ItemTemplate>
                                    <%
                                        if (state_s)
                                        { 
                                    %>
                                    <tr class="sk1">
                                        <%
                                            state_s = false;
                   }
                   else
                   { 
                                        %>
                                        <tr class="sk2">
                                            <%
                                                state_s = true;
                   }
                                            %>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem,"MatchNumber")%>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem,"Game")%>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem,"MainTeam")%>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem,"MainLoseBall")%>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem,"GuestTeam")%>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem,"Result")%>
                                            </td>
                                            <td>
                                                <div style="width: 200px; height: 15px; text-align: left; background-image: url(Images/uk.png);
                                                    background-attachment: scroll; background-position: center center">
                                                    <div>
                                                        <img src="Images/ul.png" height="15" width="<%# DataBinder.Eval(Container.DataItem,"Scale")%>%" /></div>
                                                </div>
                                            </td>
                                            <td>
                                                <font color="red">
                                                    <%# DataBinder.Eval(Container.DataItem,"Scale")%>%</font>
                                            </td>
                                        </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <!--擂台sider-->
        <div class="side_ring">
            <!--擂台公告-->
            <div class="notice_ring">
                <div class="side_nav">
                    <h2>
                        擂台公告</h2>
                </div>
                <div class="notice_ring_list">
                    <ul>
                        <%=NewsHTML %>
                    </ul>
                </div>
            </div>
            <!--我的擂台-->
            <asp:Panel ID="pUserDetails" runat="server">
            <div style="height:10px;"></div>
                <div class="notice_ring">
                    <div class="side_nav">
                        <h2>
                            我的擂台</h2>
                    </div>
                    <div class="eyed_boy_cont">
                        <table width="100%" border="0" align="center" class="game_hot">
                            <tbody>
                                <asp:Repeater runat="server" ID="gUserDetails">
                                    <ItemTemplate>
                                        <table width="100%" cellspacing="4" border="0" height="115">
                                            <tr style="height:25px;">
                                                <td width="100%" colspan="2">
                                                    &nbsp;&nbsp;<img src="images/hot_men.gif" width="16" height="20" align="absmiddle"><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem,"UserId")%>','我的历史记录',470,600)"><%# DataBinder.Eval(Container.DataItem,"Name")%></a>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;">
                                                <td width="100px">
                                                    &nbsp;&nbsp;投注场次：
                                                    <%# DataBinder.Eval(Container.DataItem, "BetCount")%>
                                                </td>
                                                <td>
                                                    命中场次：
                                                    <%# DataBinder.Eval(Container.DataItem, "WinCount")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="100px">
                                                    &nbsp;&nbsp;我的积分：
                                                    <%# DataBinder.Eval(Container.DataItem, "Score")%>
                                                </td>
                                                <td >
                                                    累计获奖：<font style="color: Red">￥<%# Shove._Convert.StrToDouble(DataBinder.Eval(Container.DataItem, "TotalWinMoney").ToString(),0.00).ToString("F2")%></font>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <!--积分榜-->
            <div class="side_tab">
                <div class="sider_tabnav">
                    <h2>
                        近10天积分榜</h2>
                    <dl>
                        <a href="ChallengeRanking.aspx">更多&gt;&gt;</a>
                    </dl>
                </div>
                <div class="side_tabcont">
                    <table class="table_c2" width="100%" border="0" cellpadding="0" cellspacing="1">
                        <tbody>
                            <tr class="new_t2">
                                <td>
                                    排名
                                </td>
                                <td>
                                    用户名
                                </td>
                                <td>
                                    积分
                                </td>
                            </tr>
                            <asp:Repeater ID="gSchemesToDay" runat="server">
                                <ItemTemplate>
                                    <%
                                        if (!state)
                                        {
                                    %>
                                    <tr>
                                        <td height="22" bgcolor="#F2F2F2">
                                            <%# DataBinder.Eval(Container.DataItem, "Ranking")%>
                                        </td>
                                        <td bgcolor="#F2F2F2">
                                            <a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)">
                                                <%# DataBinder.Eval(Container.DataItem, "Name")%></a>
                                        </td>
                                        <td bgcolor="#FFFFFF" class="c_red fb">
                                            <%# DataBinder.Eval(Container.DataItem, "Score")%>
                                        </td>
                                    </tr>
                                    <%
                                        state = true;
                                    }
                                    else
                                    {
                                    %>
                                    <tr>
                                        <td height="22" bgcolor="#FFFFFF">
                                            <%# DataBinder.Eval(Container.DataItem, "Ranking")%>
                                        </td>
                                        <td bgcolor="#FFFFFF">
                                            <a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)">
                                                <%# DataBinder.Eval(Container.DataItem, "Name")%></a>
                                        </td>
                                        <td bgcolor="#FFFFFF" class="c_red fb">
                                            <%# DataBinder.Eval(Container.DataItem, "Score")%>
                                        </td>
                                    </tr>
                                    <% state = false;
                                    } %>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
            <!--积分榜-->
            <div class="side_tab">
                <div class="sider_tabnav">
                    <h2>
                        月排行榜</h2>
                    <dl>
                        <a href="ChallengeRanking.aspx">更多&gt;&gt;</a>
                    </dl>
                </div>
                <div class="side_tabcont">
                    <table class="table_c2" width="100%" border="0" cellpadding="0" cellspacing="1">
                        <tbody>
                            <tr class="new_t2">
                                <td>
                                    排名
                                </td>
                                <td>
                                    用户名
                                </td>
                                <td>
                                    注数
                                </td>
                                <td>
                                    积分
                                </td>
                            </tr>
                            <asp:Repeater runat="server" ID="gSchemesToMonth">
                                <ItemTemplate>
                                    <%
                                        if (!state)
                                        {
                                    %>
                                    <tr>
                                        <td height="22" bgcolor="#F2F2F2">
                                            <%# DataBinder.Eval(Container.DataItem, "Ranking")%>
                                        </td>
                                        <td bgcolor="#F2F2F2">
                                            <a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)">
                                                <%# DataBinder.Eval(Container.DataItem, "Name")%></a>
                                        </td>
                                        <td bgcolor="#F2F2F2">
                                            <%# DataBinder.Eval(Container.DataItem, "Counts")%>
                                        </td>
                                        <td bgcolor="#FFFFFF" class="c_red fb">
                                            <%# DataBinder.Eval(Container.DataItem, "Score")%>
                                        </td>
                                    </tr>
                                    <%
                                        state = true;
                                    }
                                    else
                                    {
                                    %>
                                    <tr>
                                        <td height="22" bgcolor="#FFFFFF">
                                            <%# DataBinder.Eval(Container.DataItem, "Ranking")%>
                                        </td>
                                        <td bgcolor="#FFFFFF">
                                            <a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)">
                                                <%# DataBinder.Eval(Container.DataItem, "Name")%></a>
                                        </td>
                                        <td bgcolor="#FFFFFF">
                                            <%# DataBinder.Eval(Container.DataItem, "Counts")%>
                                        </td>
                                        <td bgcolor="#FFFFFF" class="c_red fb">
                                            <%# DataBinder.Eval(Container.DataItem, "Score")%>
                                        </td>
                                    </tr>
                                    <% state = false;
                                    } %>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
            <!--积分榜-->
            <div class="side_tab">
                <div class="sider_tabnav">
                    <h2>
                        总积分榜（近60天）</h2>
                    <dl>
                        <a href="ChallengeRanking.aspx">更多&gt;&gt;</a>
                    </dl>
                </div>
                <div class="side_tabcont">
                    <table class="table_c2" width="100%" border="0" cellpadding="0" cellspacing="1">
                        <tbody>
                            <tr class="new_t2">
                                <td>
                                    排名
                                </td>
                                <td>
                                    用户名
                                </td>
                                <td>
                                    积分
                                </td>
                            </tr>
                            <asp:Repeater runat="server" ID="gSchemesToMain">
                                <ItemTemplate>
                                    <%
                                        if (!state)
                                        {
                                    %>
                                    <tr>
                                        <td height="22" bgcolor="#F2F2F2">
                                            <%# DataBinder.Eval(Container.DataItem, "Ranking")%>
                                        </td>
                                        <td bgcolor="#F2F2F2">
                                            <a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)">
                                                <%# DataBinder.Eval(Container.DataItem, "Name")%></a>
                                        </td>
                                        <td bgcolor="#FFFFFF" class="c_red fb">
                                            <%# DataBinder.Eval(Container.DataItem, "Score")%>
                                        </td>
                                    </tr>
                                    <%
                                        state = true;
                                    }
                                    else
                                    {
                                    %>
                                    <tr>
                                        <td height="22" bgcolor="#FFFFFF">
                                            <%# DataBinder.Eval(Container.DataItem, "Ranking")%>
                                        </td>
                                        <td bgcolor="#FFFFFF">
                                            <a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)">
                                                <%# DataBinder.Eval(Container.DataItem, "Name")%></a>
                                        </td>
                                        <td bgcolor="#FFFFFF" class="c_red fb">
                                            <%# DataBinder.Eval(Container.DataItem, "Score")%>
                                        </td>
                                    </tr>
                                    <% state = false;
                                    } %>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
            <!--擂台红人-->
            <div class="eyed_boy">
                <div class="side_nav">
                    <h2>
                        擂台红人</h2>
                </div>
                <div class="eyed_boy_cont">
                    <table border="0" align="center" class="game_hot">
                        
                        <asp:Repeater runat="server" ID="gBetHot">
                            <ItemTemplate>
                                <%
                                    if (indexs == 0)
                                    { 
                                
                                %>
                                <tr><td colspan="4"><div style="height:8px"></div></td></tr>
                                <tr>
                                    <%
                                        }
                            indexs++;                            
                                    %>
                                    <td>
                                        <table width="125px" border="0">
                                            <tr>
                                                <td width="20%">
                                                    &nbsp;&nbsp;&nbsp;<img src="images/hot_men.gif" width="16" height="20" align="absmiddle"><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem,"UserId")%>','我的历史记录',470,600)"><%# DataBinder.Eval(Container.DataItem,"Name")%></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="30%">
                                                    &nbsp;投注场次：
                                                    <%# DataBinder.Eval(Container.DataItem, "BetCount")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="30%">
                                                    &nbsp;命中场次：
                                                    <%# DataBinder.Eval(Container.DataItem, "WinCount")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="30%">
                                                   &nbsp;累计获奖：<font style="color: Red">￥<%# Shove._Convert.StrToDouble(DataBinder.Eval(Container.DataItem, "TotalWinMoney").ToString(),0.00).ToString("F2")%></font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <%
                                        if (indexs == 2)
                                        {
                                            indexs = 0;                                                           
                                    %>                   
                                </tr>                                
                                <%
                                    }
                                %>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            <!--最新的奖励信息-->
            <div class="side_tab">
                <div class="sider_tabnav">
                    <h2>
                        最新的奖励信息</h2>
                    <dl>
                        <a href="/Challenge/ChallengeHelp.aspx">详情&gt;&gt;</a>
                    </dl>
                </div>
                <div class="side_tabcont">
                    <table class="table_c2" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#DBDBDB">
                        <tbody>
                            <tr class="new_t2">
                                <td>
                                    名次
                                </td>
                                <td>
                                    奖金
                                </td>
                            </tr>
                            <tr>
                                <td height="22">
                                    第1名
                                </td>
                                <td class="c_org fb">
                                    1200元
                                </td>
                            </tr>
                            <tr>
                                <td height="22">
                                    第2名~ 第3名
                                </td>
                                <td class="c_org fb">
                                    600元
                                </td>
                            </tr>
                            <tr>
                                <td height="22">
                                    第4名~ 第8名
                                </td>
                                <td class="c_org fb">
                                    300元
                                </td>
                            </tr>
                            <tr>
                                <td height="22">
                                    第9名~ 第15名
                                </td>
                                <td class="c_org fb">
                                    50元
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="cl">
        </div>
    </div>
    <uc2:WebFoot ID="WebFoot1" runat="server" />
    <asp:HiddenField ID="HidPageNumber" runat="server" Value="1" />
    <asp:Button ID="btnPaging" runat="server" Text="Button" OnClick="btnPaging_Click"
        Style="display: none;" />
    <asp:DropDownList ID="DropDownList1" AutoPostBack="true" Visible="false" runat="server">
                            </asp:DropDownList>
    <script src="JScript/public.js" type="text/javascript"></script>    
    <script src="JScript/Pagination.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="/Components/My97DatePicker/WdatePicker.js"></script>
    </form>
</body>
</html>
