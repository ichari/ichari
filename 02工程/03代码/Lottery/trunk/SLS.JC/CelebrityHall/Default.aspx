<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="CelebrityHall_Default"%>

<%@ Register Src="../Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>名人堂</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="Style/mingren.css"/>
    <link href="/Style/pub.css" rel="stylesheet" type="text/css" />
    <link href="/Style/thickbox.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form runat="server">
<div>
    <div class="user_topbar">
        <!--登录前-->
        <div class="top_link" style="display: none" id="lbefore">
            <ul>
                <li class="dl_txt"><a href="/UserLogin.aspx?RequestLoginPage=/CelebrityHall/Default.aspx">请登录</a></li>
                <li class="gray_fg">|</li>
                <li><a href="/UserReg.aspx" rel="nofollow" title="" class="c_btn" target="_blank">免费注册</a></li>
            </ul>
        </div>
        <!--登录后-->
        <div class="top_link" style="display: none" id="lafter">
            <ul>
                <li>欢迎您，<em><span id="topUserName"></span></em>! &nbsp; <a href="/Home/Room/ViewAccount.aspx">我的账户</a></li>
                <li>[<a href="/Home/Room/OnlinePay/Alipay02/Send_Default.aspx" class="c_org">充值</a>]</li>
                <li>[<a href="#" id="topLoginOut">退出</a>]</li>
                <li><a href="/Admin/" id="user_login_manger" style="display: none;">[超级管理]</a></li>
            </ul>
        </div>
        <dl><a href="/">首页</a> | <a href="/bbs/">彩民论坛</a> | <a href="/Help/Help_Default.aspx">帮助中心</a> | <a href="/BotNav/about1.html">关于我们</a></dl>
    </div>
</div>
<div style="background:#ffefdb url(../images/m_topbg.gif) repeat-x center top;">
    <div class="m_header">
    </div>
</div>
<div class="wrap">
	<!--本期推荐-->
    <div class="top_cp">
    	<div class="top_cpname">
        	<h2><img src="images/icon.gif" width="58" height="58" /></h2>
            <h3><a href="../Home/Web/Score.aspx?id=<%=UserID %>&LotteryID=<%=UserLotteryID %>" target="_blank"><%=UserName%></a></h3>
            <dl>
            	<dt>
            	   <asp:ImageButton  runat="server" ID="btn_Single" ImageUrl="images/btn_gendan.gif"
                         Width="84px" Height="26px" OnClientClick="return CreateLogin('');" OnClick="btn_Single_Click" />
                </dt>
                <dt><a href="#">
                    </a>
                </dt>
            </dl>
        </div>
        <div class="top_cplist">
        	<dl>胜负彩赢利探索者，大奖的不懈追求者 </dl>
            <h2>大奖回顾</h2>
            <ul>
                <asp:Repeater ID="gRecommend" runat="server">
                    <ItemTemplate>
                        <li><%# DataBinder.Eval(Container.DataItem, "LotteryName")%>第<%# DataBinder.Eval(Container.DataItem, "IsuseName")%>期：喜中<%# Shove._Convert.StrToDouble(DataBinder.Eval(Container.DataItem, "WinMoney").ToString(),0).ToString("F2")%></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
    <!--潜力推荐-->
	<div class="top_pot">
    	<table class="table_pot" cellspacing="1" cellpadding="0" width="458" align="center" border="0">
        	<tr align="center">
            	<th>序号</th>
                <th>期号</th>
                <th>彩种</th>
                <th>潜力发单人</th>
                <th>方案金额</th>
                <th>本期方案</th> 
            </tr>
            <asp:Repeater ID="gPotential" runat="server">
                <ItemTemplate>
                    <tr>
            	        <td><%# DataBinder.Eval(Container.DataItem, "Ranking")%></td>
                        <td>第<%# DataBinder.Eval(Container.DataItem, "IsuseName")%>期</td>
                        <td><a href="../Home/Room/Scheme.aspx?id=<%# DataBinder.Eval(Container.DataItem, "SchemeID")%>" target="_blank"><%# DataBinder.Eval(Container.DataItem, "Name")%></a></td>
                        <td><a href="../Home/Room/Scheme.aspx?id=<%# DataBinder.Eval(Container.DataItem, "SchemeID")%>" target="_blank"><%# DataBinder.Eval(Container.DataItem, "UserName")%></td>
                        <td><%# Shove._Convert.StrToDouble(DataBinder.Eval(Container.DataItem, "Money").ToString(),0).ToString("F2")%></a></td>
                        <td><a href="../Home/Room/Scheme.aspx?id=<%# DataBinder.Eval(Container.DataItem, "SchemeID")%>" target="_blank">查看</a></td> 
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="cl"></div>
    <div class="main_mrhm">
    	<div class="m_tit">
        	<div class="icon_ming"></div>
            <h2>名人合买</h2>
            <dl>
                <a href="/Join/Default.aspx">更多合买&gt;&gt;</a>
            </dl>
        </div>
        <%=innerHTML %>
        
    </div>
    
    
    <div class="cl"></div>
    <div class="siderbar">
    	<div class="mr_hot">
        	<h2><span>名人新闻：</span></h2>
            <ul>
               <%=NewsHTML %>                            
            </ul>
        </div>
        <div class="mr_hero">
        	<h2><span>大奖英雄榜：</span></h2>
            <h4><span class="w30">排名</span><span class="w70">用户名</span><span class="w60">彩种</span><span class="w90">中奖金额</span></h4>
            <ol id="roll">
                <asp:Repeater ID="gHero" runat="server">
                    <ItemTemplate>
                        <li><span class="w30"><%# DataBinder.Eval(Container.DataItem, "Ranking")%></span><span class="w70"><%# DataBinder.Eval(Container.DataItem, "UserName")%></span><span><%# DataBinder.Eval(Container.DataItem, "Name")%></span><span class="w90 c_org">￥<%# Shove._Convert.StrToDouble(DataBinder.Eval(Container.DataItem, "Money").ToString(),0).ToString("F2")%></span></li>
                    </ItemTemplate>                    
                </asp:Repeater>            	
            </ol>
            
        </div>
        
    </div>
    <!--right mr_main-->
    <div class="mr_main">
    	<div class="m_banner"><img src="images/mr_banner.jpg" width="732" height="80" /></div>
        <div class="mr_renlist">
        	<h2>名人汇聚</h2>
        	<%=innerHTMLStar %>
        </div>
    </div>
    
</div>
<!--footer-->
<div class="footer">
    <uc2:WebFoot runat="server" ID="foot" />
</div>
<asp:HiddenField ID="hfUserID" runat="server" Visible="false" />
<asp:HiddenField ID="hffLotteryID" runat="server" Visible="false" />
<!--未登录提示层-->
<div style="display: none; width: 360px;" id="loginLay">
    <div>
        <div class="tips_text">
            <div class="dl_tips" id="error_tips" style="display: none;">
                <b class="dl_err"></b>您输入的账户名和密码不匹配，请重新输入。</div>
            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="dl_tbl">
                <tr>
                    <td style="width: 70px;">
                        用户名：
                    </td>
                    <td>
                        <input type="text" class="tips_txt" id="lu" name="lu" />
                    </td>
                    <td class="t_ar">
                        <a href="/UserReg.aspx" target="_blank" tabindex="-1">免费注册</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        密&nbsp;&nbsp;码：
                    </td>
                    <td>
                        <input type="password" class="tips_txt" id="lp" name="lp" />
                    </td>
                    <td class="t_ar">
                        <a href="/ForgetPassword.aspx" target="_blank" tabindex="-1">忘记密码</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        验证码：
                    </td>
                    <td colspan="2">
                        <input type="text" class="tips_yzm" id="yzmtext" name="c" /><img alt="验证码" src="about:blank"
                            id="yzmimg" style="cursor: pointer; width: 60px; height: 25px"><a class="kbq" href="#"
                                id="yzmup">看不清，换一张</a>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <input type="button" class="btn_Dora_b" value="登 录" id="floginbtn" style="margin-right: 18px" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<div id="DivUserinfo">
    <input id="head_HidUserID" name="head_HidUserID" value="-1" type="hidden"/>
</div>

<script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<script src="/JScript/innerfade.js" type="text/javascript"></script>
<script src="/JScript/thickbox.js" type="text/javascript"></script>
<script src="/JScript/head.js" type="text/javascript"></script>
<script type="text/javascript" src="JScript/hero.js"></script>
<script type="text/javascript" src="JScript/ScrollPic.js"></script>
<script language="javascript" type="text/javascript">

    var scrollPic_02 = new ScrollPic();
    scrollPic_02.scrollContId = "ISL_Cont_1"; //内容容器ID
    scrollPic_02.arrLeftId = "LeftArr"; //左箭头ID
    scrollPic_02.arrRightId = "RightArr"; //右箭头ID

    scrollPic_02.frameWidth = 960; //显示框宽度
    scrollPic_02.pageWidth = 240; //翻页宽度

    scrollPic_02.speed = 10; //移动速度(单位毫秒，越小越快)
    scrollPic_02.space = 10; //每次移动像素(单位px，越大越快)
    scrollPic_02.autoPlay = false; //自动播放
    scrollPic_02.autoPlayTime = 4; //自动播放间隔时间(秒)

    if ($("#_mr_info .mr_info").length > 4) {
        scrollPic_02.initialize(); //初始化
    }

</script>
</form>
</body>
</html>
