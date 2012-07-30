<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChallengeRanking.aspx.cs" Inherits="Challenge_ChallengeRanking" %>
<%@ Register Src="../Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="../Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>擂台_排行榜</title>
 <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
 <link rel="stylesheet" type="text/css" href="Style/ring.css"/>
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
                <li class="curr"><a href="ChallengeRanking.aspx">排行榜</a></li>
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
            <div class="cl"></div>
        </div>
        <!--排行榜-->
        <div class="ring_topnav">
            <ul id="myTab0">
                <li class="active" onclick="nTabs(this,0);">总排行榜</li>
				<li style="display:none" class="normal" onclick="nTabs(this,1);">近60天榜</li>				
            </ul>
            <div class="fansearch"><span>方案搜索：</span>
                
                <asp:TextBox runat="server" ID="tbUserName" Text="输入用户名" Width="130px" Height="17px" ForeColor="Gray" onfocus="if(this.value=='输入用户名')this.value='';" onblur="if(this.value=='')this.value='输入用户名';"></asp:TextBox>
                <span class="dtd" style="text-align:left">
                    <asp:Button ID="btnFile" class="btn_ringso"  name="dropMonth" runat="server" Text="查 询" onclick="btnFile_Click" />                
                </span>
            </div>
        </div>
        <div class="TabContent">
			<div id="myTab0_Content0">
                <div class="ringtab" style="margin-top:0;">
                    <table class="table_c1" cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                        <tbody>
                            <tr class="tre">
                                <td width="5%">排名</td>
                                <td width="10%">用户名</td>
                                <td width="9%">投注场次</td>
                                <td width="9%">命中场次</td>
                                <td width="10%">命中率</td>
                                <td width="10%">2串1中奖</td>
                                <td width="10%">积分</td>
                                <td width="8%">奖励金额</td>
                                <td width="10%">历史战绩</td>
                            </tr>
                            <asp:Repeater runat="server" ID ="gRanking">
                                <ItemTemplate>
                                               <%
                   if (state)
                   { 
                    %>
                          <tr class="sk1">    
                                         <%
                                             state = false;
                   }
                   else { 
                        %>
                            
                        <tr class="sk2" >    
                        
                        <%
                            state = true;
                   }
                        %>   
                                        <td><%# DataBinder.Eval(Container.DataItem, "Ranking")%></td>
                                        <td><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "ID")%>','历史战绩',470,660)" class="dl">
                                            <%# DataBinder.Eval(Container.DataItem, "Name")%></a>
                                        </td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "BetCount")%></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "WinCount")%></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "Scale")%>%</td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "sumMoney")%></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "Score")%></td>
                                        <td><span class="bred2">￥<%# Shove._Convert.StrToDouble(DataBinder.Eval(Container.DataItem, "TotalWinMoney").ToString(),0.00).ToString("F2")%></span></td>
                                        <td><a href="javascript:showWinOpen('ChellengeUserSchemesDetails.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "ID")%>','历史战绩',470,660)" class="dl">查看</a></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                           
                        </tbody>
                    </table>
                </div>
            </div>
            <!---->
        </div>
    </div>
    <!--擂台sider-->
    <div class="side_ring"> 
        <!--擂台公告-->
        <div class="notice_ring">
            <div class="side_nav">
                <h2>擂台公告</h2>
            </div>
            <div class="notice_ring_list">
                <ul>
                    <%=newsHTML %>
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
                    <td class="c_org fb">300元</td>
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