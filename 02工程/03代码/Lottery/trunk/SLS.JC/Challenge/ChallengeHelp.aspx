<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChallengeHelp.aspx.cs" Inherits="Challenge_ChallengeHelp" EnableViewState="false" %>

<%@ Register Src="../Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="../Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
 <title>擂台_玩法规则</title>
 <link href="Style/ring.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">
<uc1:WebHead ID="WebHead1" runat="server" />
<div class="wrap"> 
    <!--擂台main-->
    <div class="main_ring">
        <div class="explain_nav">
            <ul>
                <li class="w88"><a href="Default.aspx">参与竞猜</a></li>
                <li class="w88"><a href="ChallengeSchemeList.aspx">方案列表</a></li>
                <li class="w88"><a href="ChallengeRanking.aspx">排行榜</a></li>
                <li class="curr"><a href="ChallengeHelp.aspx">玩法规则</a></li>
            </ul>
        </div>
        <div class="explain_cont">
            <h1 class="explain_ico"></h1>
            <dl>
                1、用户可以选择2场单选进行投注，用户凭唯一真实姓名每天仅能提交1个竞猜方案。<br />
                2、用户参与竞猜前，需绑定手机号码，为空或不具唯一性均不能提交竞猜方案。<br />
                3、赛程根据体彩中心的竞彩胜平负玩法设定，擂台竞猜按日进行。<br />
                4、每月积分排名前15名的用户奖获得网站提供的现金奖励。&nbsp;&nbsp;<span><a href="#jtwf">[具体玩法]</a></span><br />
                <span class="c_999">备注：用户登录后可进入任何擂台方案页对投注选项进行修改，并通过添加投注功能，发起真实的投注方案</span>
                <div style="width:100%; height:5px"></div>    
            </dl>
            <div class="cl"></div>
        </div>
        
        <a id="jtwf" name="jtwf"></a>
        <!--竞猜对阵-->
        <div class="ringtab">
            <div class="ringtab_nav">
                <h2>擂台玩法规则</h2>
            </div>
            <div class="ringtab_cont">
            	<div class="ring_tipgz"><span class="fb">备注：</span>用户登录后可进入任何擂台方案页对投注选项进行修改，并通过添加投注功能，发起真实的投注方案。</div>
                <div class="ring_help">
                	<h2><img src="images/ga_num1.gif" width="29" height="23" align="absmiddle"> 参与条件：</h2>
                    <p> A.用户必须是注册会员；<br>
                        B.用户参与竞猜前，需绑定手机号码，为空或不具唯一性均不能提交竞猜方案；<br>
                        C.用户凭唯一真实姓名每天仅能提交1个竞猜方案。<br>
                        D.一人只可用一个账号参与擂台竞猜，如有发现一人使用多个账号参与，本网有权取消其参与及领奖资格。 </p>
                    <h2><img src="images/ga_num2.gif" width="29" height="23" align="absmiddle"> 场次设定：</h2>
                    <p>根据体彩中心的竞彩让球胜平负赛程设定，擂台竞猜按日进行。</p>
                    <h2><img src="images/ga_num3.gif" width="29" height="23" align="absmiddle"> 玩法说明：</h2>
                    <p> A.每个用户都可以选择2场赛事进行单选投注；<br>
                        B.每次提交方案时，过关方式只可以选择与场次相对应的串关；<br>
                        C.每一个账号每天可以提交1个竞猜方案，一旦方案成功提交，用户不能对方案进行修改；<br>
                        D.擂台竞猜截止时间按本网的该彩种的复式截止时间执行。<br>
                        E:为了防止部份用户博冷心态，减少运气成份，提高中奖率，凡方案奖金预测超过25元的均限制发起。 </p>
                    <h2><img src="images/ga_num4.gif" width="29" height="23" align="absmiddle"> 积分制度：</h2>
                    <p> A.积分是参与者竞猜的成绩，竞彩足球竞猜擂台的积分将作为周奖的颁发依据；<br>
                        B.根据每个提交方案的中奖情况，给予玩家相应的积分（按用户方案发起时间计入当天的积分）；<br>
                        C.积分奖励，按方案的中奖金额进行计算积分，每一元得一分，只计算到元。<br>
                    </p>
                    <h2><img src="images/ga_num5.gif" width="29" height="23" align="absmiddle"> 积分排名奖励：</h2>
                    <p>竞猜足球竞猜擂台只设立月奖，按照每月的积分排序，积分排名前15名的用户将得网站派发的月奖，直接返还到用户账户中，可提现或进行购彩：<br>
                        A.积分排名第一位：<span class="c_org fb">1200元</span>；<br>
                        B.积分排名第二位至第三位：<span class="c_org fb">600元</span>；<br>
                        C.积分排名第四位至第八位：<span class="c_org fb">300元</span>；<br>
                        D.积分排名第九位至第十五位：<span class="c_org fb">50元</span>。 </p>
                    <h2><img src="images/ga_num6.gif" width="29" height="23" align="absmiddle"> 奖金派发：</h2>
                    <p>竞彩足球竞猜擂台每月奖金派发会在每个周期结束后的10天内将相应的奖金返到用户的帐户中。</p>
                </div>
                <div class="cl"></div>
            </div>
        </div>
    <!--擂台最新方案--><!--擂台当天赛事热门投注--></div>
    <!--擂台sider-->
    <div class="side_ring"> 
        <!--擂台公告-->
        <div class="notice_ring">
            <div class="side_nav">
                <h2>擂台公告</h2>
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
                <h2>近10天积分榜</h2>
                <dl>
                    <a href="ChallengeRanking.aspx">更多&gt;&gt;</a>
                </dl>
            </div>
            <div class="side_tabcont">
                <table class="table_c2" width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tbody>
                        <tr class="new_t2">
                            <td>排名</td>
                            <td>用户名</td>
                            <td>积分</td>
                        </tr>
                        <asp:Repeater ID="gSchemesToDay" runat="server">
                            <ItemTemplate>
                                <%
                                    if (!state)
                                    {
                                     %>
                                <tr>
                                    <td height="22" bgcolor="#F2F2F2"><%# DataBinder.Eval(Container.DataItem, "Ranking")%></td>
                                    <td bgcolor="#F2F2F2"><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)"><%# DataBinder.Eval(Container.DataItem, "Name")%></a></td>
                                    <td bgcolor="#FFFFFF" class="c_red fb"><%# DataBinder.Eval(Container.DataItem, "Score")%></td>
                                </tr>
                                <%
                                    state = true;
                                    }
                                    else
                                    {
                                         %>
                                    <tr>
                                        <td height="22" bgcolor="#FFFFFF"><%# DataBinder.Eval(Container.DataItem, "Ranking")%></td>
                                        <td bgcolor="#FFFFFF"><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)"><%# DataBinder.Eval(Container.DataItem, "Name")%></a></td>
                                        <td bgcolor="#FFFFFF" class="c_red fb"><%# DataBinder.Eval(Container.DataItem, "Score")%></td>
                                    </tr>
            
                                         
                                         <% state=false;} %>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <!--积分榜-->
        <div class="side_tab">
            <div class="sider_tabnav">
                <h2>月排行榜</h2>
<dl>
                    <a href="ChallengeRanking.aspx">更多&gt;&gt;</a>
                </dl>
            </div>
            <div class="side_tabcont">
                <table class="table_c2" width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tbody>
                        <tr class="new_t2">
                            <td>排名</td>
                            <td>用户名</td>
                            <td>注数</td>
                            <td>积分</td>
                        </tr>
                        <asp:Repeater runat="server" ID="gSchemesToMonth">
                            <ItemTemplate>
                            <%
                                    if (!state)
                                    {
                                     %>
                                <tr>
                                    <td height="22" bgcolor="#F2F2F2"><%# DataBinder.Eval(Container.DataItem, "Ranking")%></td>
                                    <td bgcolor="#F2F2F2"><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)"><%# DataBinder.Eval(Container.DataItem, "Name")%></a></td>
                                    <td bgcolor="#F2F2F2"><%# DataBinder.Eval(Container.DataItem, "Counts")%></td>
                                    <td bgcolor="#FFFFFF" class="c_red fb"><%# DataBinder.Eval(Container.DataItem, "Score")%></td>
                                </tr>
                                 <%
                                    state = true;
                                    }
                                    else
                                    {
                                         %>
                                        <tr>
                                    <td height="22" bgcolor="#FFFFFF"><%# DataBinder.Eval(Container.DataItem, "Ranking")%></td>
                                    <td bgcolor="#FFFFFF"><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)"><%# DataBinder.Eval(Container.DataItem, "Name")%></a></td>
                                    <td bgcolor="#FFFFFF"><%# DataBinder.Eval(Container.DataItem, "Counts")%></td>
                                    <td bgcolor="#FFFFFF" class="c_red fb"><%# DataBinder.Eval(Container.DataItem, "Score")%></td>
                                </tr> 
                                          <% state=false;} %>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <!--积分榜-->
        <div class="side_tab">
            <div class="sider_tabnav">
                <h2>总积分榜（近60天）</h2>
				<dl>
                    <a href="ChallengeRanking.aspx">更多&gt;&gt;</a>
                </dl>
            </div>
            <div class="side_tabcont">
                <table class="table_c2" width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tbody>
                        <tr class="new_t2">
                            <td>排名</td>
                            <td>用户名</td>
                            <td>积分</td>
                        </tr>
                        <asp:Repeater runat="server" ID="gSchemesToMain">
                            <ItemTemplate>
                            <%
                                    if (!state)
                                    {
                                     %>
                                <tr>
                                    <td height="22" bgcolor="#F2F2F2"><%# DataBinder.Eval(Container.DataItem, "Ranking")%></td>
                                    <td bgcolor="#F2F2F2"><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)"><%# DataBinder.Eval(Container.DataItem, "Name")%></a></td>
                                    <td bgcolor="#FFFFFF" class="c_red fb"><%# DataBinder.Eval(Container.DataItem, "Score")%></td>
                                </tr>
                                 <%
                                    state = true;
                                    }
                                    else
                                    {
                                         %>
                                         
                                <tr>
                                    <td height="22" bgcolor="#FFFFFF"><%# DataBinder.Eval(Container.DataItem, "Ranking")%></td>
                                    <td bgcolor="#FFFFFF"><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "UserID")%>','玩家历史记录',470,600)"><%# DataBinder.Eval(Container.DataItem, "Name")%></a></td>
                                    <td bgcolor="#FFFFFF" class="c_red fb"><%# DataBinder.Eval(Container.DataItem, "Score")%></td>
                                </tr>
                                
                                          <% state=false;} %>
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
                <h2>最新的奖励信息</h2>
				<dl>
                    <a href="/Challenge/ChallengeHelp.aspx">详情&gt;&gt;</a>
                </dl>
            </div>
         	<div class="side_tabcont">
                <table class="table_c2" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#DBDBDB">
                  <tbody><tr class="new_t2">
                    <td>名次</td>
                    <td>奖金</td>
                  </tr>
                  
                  <tr>
                    <td height="22">第1名</td>
                    <td class="c_org fb">1200元</td>
                  </tr>
                  
                  <tr>
                    <td height="22">第2名~ 第3名</td>
                    <td class="c_org fb">600元</td>
                  </tr>
                  
                  <tr>
                    <td height="22">第4名~ 第8名</td>
                    <td class="c_org fb">470元</td>
                  </tr>
                  
                  <tr>
                    <td height="22">第9名~ 第15名</td>
                    <td class="c_org fb">50元</td>
                  </tr>
                </tbody></table>
            </div>
        </div>
    </div>
<div class="cl"></div>
</div>
<uc2:WebFoot ID="WebFoot1" runat="server" />
<script type="text/javascript" src="JScript/public.js"></script>
</form>
</body>
</html>

