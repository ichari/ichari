<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Challenge_Default" EnableViewState="false" %>

<%@ Register Src="../Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="../Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>竞彩擂台</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="Style/ring.css" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
</head>
<body class="gybg">
    <form id="buy_form" runat="server" ajax="Buy_Challenge_Handler.ashx">
    <uc1:WebHead ID="WebHead1" runat="server" />
    <div class="wrap">
        <!--擂台main-->
        <div class="main_ring">
            <div class="explain_nav">
                <ul>
                    <li class="curr"><a href="Default.aspx">参与竞猜</a></li>
                    <li class="w88"><a href="ChallengeSchemeList.aspx">方案列表</a></li>
                    <li class="w88"><a href="ChallengeRanking.aspx">排行榜</a></li>
                    <li class="w88"><a href="ChallengeHelp.aspx">玩法规则</a></li>
                </ul>
            </div>
            <div class="explain_cont">
                <h1 class="explain_ico"></h1>
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
                    <h2>竞猜对阵(竞彩让球胜平负)：<span class="f12 fn c_999">截止时间：赛前15分钟</span></h2>
                </div>
                <div class="ringtab_cont">
                    <table class="table_t1" cellspacing="1" cellpadding="0" width="738" align="center"
                        border="0">
                        <tbody>
                            <tr class="tre">
                                <td width="59">
                                    编号
                                </td>
                                <td width="86">
                                    赛事
                                </td>
                                <td width="80">
                                    比赛时间
                                </td>
                                <td>
                                    主队
                                </td>
                                <td>
                                    让球
                                </td>
                                <td>
                                    客队
                                </td>
                                <td colspan="4">
                                    <select id="op_col">
                                        <option value="99家平均">99家平均欧指</option>
                                        <option value="威廉希尔">威廉希尔</option>
                                        <option value="立博">立博</option>
                                        <option value="Bet365">Bet365</option>
                                        <option value="澳门彩票">澳门彩票</option>
                                    </select>
                                </td>
                                <td width="60">
                                    胜
                                </td>
                                <td width="60">
                                    平
                                </td>
                                <td width="60">
                                    负
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table class="table_c1"  id="table_vs" name="table_c1" cellspacing="1" cellpadding="0" width="737"
                        align="center" border="0">
                            <asp:Repeater ID="gPassRate" runat="server">
                                <ItemTemplate>
                                    <%
                                        if (state_s)
                                        { 
                                    %>
                                    <tr class="sk1" id="vs<%# DataBinder.Eval(Container.DataItem, "MatchID")%>">
                                        <%
                                            state_s = false;
                                }
                                else
                                {
                                        %>
                                        <tr class="sk1" id="vs<%# DataBinder.Eval(Container.DataItem, "MatchID")%>">
                                            <%
                                                state_s = true;
                                }
                                            %>
                                            <td width="59">
                                                <%# DataBinder.Eval(Container.DataItem, "MatchNumber")%>
                                            </td>
                                            <td width="78" class="team1" style="color: White;" bgcolor="<%# DataBinder.Eval(Container.DataItem, "GameColor")%>">
                                                <%# DataBinder.Eval(Container.DataItem, "Game")%>
                                            </td>
                                            <td width="80">
                                                <span class="o11">
                                                    <%# DataBinder.Eval(Container.DataItem, "MatchDate", "{0:MM-dd hh:mm}")%></span>
                                            </td>
                                            
                                            
                                            <td width="55">
                                                <%# DataBinder.Eval(Container.DataItem, "MainTeam")%>
                                            </td>
                                            <td width="20">
                                                <%# DataBinder.Eval(Container.DataItem, "MainLoseball")%>
                                            </td>
                                            <td width="55">
                                                <%# DataBinder.Eval(Container.DataItem, "GuestTeam")%>
                                            </td>
                                            
                                            <td class="odds" width="36" style="background-color: #edfce9">
                                                <%# DataBinder.Eval(Container.DataItem, "EuropeSSP")%>
                                            </td>
                                            <td class="odds" width="36" style="background-color: #edfce9">
                                                <%# DataBinder.Eval(Container.DataItem, "EuropePSP")%>
                                            </td>
                                            <td class="odds" width="36" style="background-color: #edfce9">
                                                <%# DataBinder.Eval(Container.DataItem, "EuropeFSP")%>
                                            </td>
                                            <td width="57">
                                                <a href="http://hbty.shovesoft.com/zc/sls_MatchReportLastMatch.aspx?MatchID=<%# DataBinder.Eval(Container.DataItem, "MatchID")%>" target="_blank">析</a> 
                                                <a href="http://hbty.shovesoft.com/zc/sls_MatchReportLastMatch.aspx?MatchID=<%# DataBinder.Eval(Container.DataItem, "MatchID")%>" target="_blank">亚</a> 
                                                <a href="http://hbty.shovesoft.com/zc/sls_MatchReportLastMatch.aspx?MatchID=<%# DataBinder.Eval(Container.DataItem, "MatchID")%>" target="_blank">欧</a>
                                            </td>
                                            <td name="tds" class="sp_bg" onclick="CheckedBg(this,'vs<%# DataBinder.Eval(Container.DataItem, "MatchID")%>')"
                                                width="60" title="主胜" style="cursor: pointer;">
                                                <input type="checkbox" rid="1" name="cb" id="<%# DataBinder.Eval(Container.DataItem, "Win","{0:F2}")%>"
                                                    sid="<%# DataBinder.Eval(Container.DataItem, "MatchID")%>" />
                                                <%# DataBinder.Eval(Container.DataItem, "Win","{0:F2}")%>
                                            </td>
                                            <td name="tds" class="sp_bg" onclick="CheckedBg(this,'vs<%# DataBinder.Eval(Container.DataItem, "MatchID")%>')"
                                                width="60" title="平局" style="cursor: pointer;">
                                                <input type="checkbox" rid="2" name="cb" id="<%# DataBinder.Eval(Container.DataItem, "Flat", "{0:F2}")%>"
                                                    sid="<%# DataBinder.Eval(Container.DataItem, "MatchID")%>" />
                                                <%# DataBinder.Eval(Container.DataItem, "Flat", "{0:F2}")%>
                                            </td>
                                            <td name="tds" class="sp_bg" onclick="CheckedBg(this,'vs<%# DataBinder.Eval(Container.DataItem, "MatchID")%>')"
                                                width="60" title="客胜" style="cursor: pointer;">
                                                <input type="checkbox" rid="3" name="cb" id="<%# DataBinder.Eval(Container.DataItem, "Lose", "{0:F2}")%>"
                                                    sid="<%# DataBinder.Eval(Container.DataItem, "MatchID")%>" />
                                                <%# DataBinder.Eval(Container.DataItem, "Lose", "{0:F2}")%>
                                            </td>
                                        </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                    </table>
                    <div class="cl">
                    </div>
                    
            
                    <div class="btn_bar">
                        <dl class="fl lh24">
                            过关方式：<span class="fb c_org">2串1</span>；
                            您选择了 <span id="counts" class="fb c_org">0</span> 
                            场，共<span id="betCount" class="fb c_org">0</span> 注。
                            <span id="betMoney" style="display:none"> (奖金预测：<b><span style="color:Red" id="GuessMoney">0</span></b>)</span>
                        </dl>
                         <span style="float: right; margin-top:-16px">
                            <input type="button" value="保存方案" class="btn_ring2" onclick="dgBtnSubmit('2')"
                                 onmouseover="this.style.cursor='pointer';"/>
                            <input name="" type="button" value="清空选项" class="btn_ring2" onclick="clearSelect();"
                                onmouseover="this.style.cursor='pointer'">
                            <input name="input" type="button" value="确认提交" class="btn_ring2" onclick="dgBtnSubmit('1')"
                                onmouseover="this.style.cursor='pointer';">
                        </span>
                    </div>
                </div>
            </div>
            <!--擂台最新方案-->
            <div class="ringtab">
                <div class="ringtab_nav">
                    <h2>
                        擂台最新方案</h2>
                    <dl>
                        <a href="ChallengeSchemeList.aspx">更多&gt;&gt;</a>
                    </dl>
                </div>
                <div class="ringtab_cont">
                    <table class="table_c1" cellspacing="1" cellpadding="0" width="100%" align="center"
                        border="0">
                        <tbody>
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
                            <asp:DataList ID="gNewBetContent" runat="server" Width="735px" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <%
                                        if (state_s)
                                        { 
                                    %>
                                    <tr class="sk1" align="center">
                                        <%
                                            state_s = false;
                   }
                   else
                   { 
                                        %>
                                        <tr class="sk2" align="center">
                                            <%
                                                state_s = true;
                   }
                                            %>
                                            <td width="5%">
                                                <%# DataBinder.Eval(Container.DataItem, "Ranking")%>
                                            </td>
                                            <td width="16%">
                                                <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                            </td>
                                            <td width="13%">
                                                <%# DataBinder.Eval(Container.DataItem, "BetCount")%>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "LotteryNumber")%>
                                            </td>
                                            <td width="13%">
                                                <%# DataBinder.Eval(Container.DataItem, "BetWay")%>
                                            </td>
                                            <td width="10%">
                                                <a href="javascript:showWinOpen('ChallengeSchemes.aspx?SchemesID=<%# DataBinder.Eval(Container.DataItem, "ID")%>','方案详情',230,600)">
                                                    查看</a>
                                            </td>
                                        </tr>
                                </ItemTemplate>
                            </asp:DataList>
                        </tbody>
                    </table>
                </div>
            </div>
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
                                        <td bgcolor="#FFFFFF"  class="c_red fb">
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
                <div class="eyed_boy_cont" style="padding-left:0px;">
                    <table border="0" cellpadding="0" cellspacing="0">
                        
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
                                                    &nbsp;<img src="images/hot_men.gif" width="16" height="20" align="absmiddle"><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem,"UserId")%>','我的历史记录',470,600)"><%# DataBinder.Eval(Container.DataItem,"Name")%></a>
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
    <uc2:WebFoot runat="server" ID="foot" />
    <input type="hidden" id="ggtypeid" name="ggtypeid" value="AA" />
    <input type="hidden" id="codes" name="codes" />
    <input type="hidden" id="playid" name="playid" value="7201" />
    <input type="hidden" id="playname" name="playname" value="让分胜平负" />
    <input type="hidden" id="lotid" name="lotid" value="72" />
    <input type="hidden" id="odds" name="odds"/>
    <input type="hidden" id="beishu" name="beishu" value="1" />
    
    <script src="JScript/public.js" type="text/javascript"></script>
    <script src="JScript/challenge.js" type="text/javascript"></script>

    </form>
</body>
</html>
