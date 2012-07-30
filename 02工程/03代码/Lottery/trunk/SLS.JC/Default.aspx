<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" EnableViewState="false" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<%@ Register TagPrefix="uc1" TagName="WebFoot" Src="Home/Room/UserControls/WebFoot.ascx" %>
<%@ Register TagPrefix="uc2" TagName="WebHead" Src="Home/Room/UserControls/WebHead.ascx" %>
<%@ Register TagPrefix="ucDraw" TagName="Draw" Src="~/UserControls/LastestDrawing.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="description" content="福彩" />
    <meta name="keywords" content="福彩" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery-ui-1.8.21.dialog.js"></script>
    <script type="text/javascript" src="/Scripts/slides.min.jquery.js"></script>
    <script type="text/javascript" src="/Scripts/ben_class.js"></script>
    <script type="text/javascript" src="/Scripts/home_page.js"></script>
    <link rel="shortcut icon" href="favicon.ico" />
    <link type="text/css" rel="stylesheet" href="/Style/css/jquery-ui-dialog.css" />
    <title>彩票首页 - 集善网 - 中国银联 中国残疾人福利基金会</title>
</head>
<body class="gybg"> 
<form id="form1" runat="server" onsubmit="return false;">
    <uc2:WebHead ID="WebHead1" runat="server" />
    <div class="w980 png_bg2" style="padding-bottom:0px;"> 
        <!-- 内容开始 -->
        <div class="w970">
            <div class="bs13">
                <div class="b_hd cp">    
                    <div id="slides">
		                <div class="slides_container">
                            <div class="slide">
                                <div id="slide_div" class="slide_div"><%= FocusImage %></div>
                                <ul id="slide_btn" class="btPhoto">
                                    <%= imgbtn %>
                                    <li class="btn03"><a class="btn_l" href="javascript:void(0)"></a></li>
                                    <li class="btn04"><a class="btn_r" href="javascript:void(0)"></a></li>
                                </ul>
                            </div>
			            </div>
                    </div>
                </div>
                <div class="r_info">
                    <!-- 未登陆 -->
                    <div id="uLogin" class="wdl"> <a id="uContLogin" class="btn_big fl" onclick="return CreateLogin('');">用户登录</a> <a class="btn_big fr" href="/UserReg.aspx">立即注册</a> </div>
                    <!-- 未登陆 -->
                    <!-- 已登陆 -->
                    <div id="uInfo" title="我的帐户" class="ydl" onclick='window.location="/Home/Room/ViewAccount.aspx";' style="cursor:pointer;">
                        <div class="fl pr5" style="width:12px;"><img width="16" height="20" src="/Images/hot_men.gif"></div>
                        <div class="fl buInfo" style="width:215px;">
                            <strong>&nbsp;<span id="uContName" class="a000f"></span></strong><strong>&nbsp;您的余额：<span id="uContBala" class="abc0"></span>元</strong>
                        <strong>&nbsp;您上次登录：<span id="uContDate"></span></strong><strong>&nbsp;<span id="uContTime"></span></strong>
                        </div>
                        <%--<div class="fl"><strong> </strong><br />帐户类型： </div>--%>
                    </div>
                    <!-- 已登陆 -->
                    <div class="b_new" style="height:154px;">
       	                <div class="title bg1x bgp7"><a class="c" id="b_tab01" href="#">网站公告</a><a id="b_tab02" href="#">购彩帮助</a></div>
                        <ul class="list" id="b_tabC01"><%= lbSiteAffiches %></ul>
                        <ul class="list" id="b_tabC02" style="display:none">
            	            <li><a class="fl" href="/Help/help_UserReg.aspx">帐户安全</a><span class="fr">05.15</span></li>
                            <li><a class="fl" href="/Help/help_Send.aspx">没有网银如何充值</a><span class="fr">05.15</span></li>
                            <li><a class="fl" href="/Help/help_Prize.aspx">我中奖了，如何领奖</a><span class="fr">05.15</span></li>
                            <li><a class="fl" href="/Help/help_Buy.aspx">如何买彩票</a><span class="fr">05.15</span></li>
                            <li><a class="fl" href="/Help/help_Account_Security.aspx">帐户安全</a><span class="fr">05.15</span></li>
                        </ul>
                    </div>
                    <div class="b_new" style="">
                        <div class="title bg1x bgp7"><a class="d" href="#">中奖名单</a></div>
                        <asp:Table ID="tblWinner" Width="90%" style="margin:10px;" runat="server"></asp:Table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class=" w980 png_bg1"></div>
    <div class="w970 bg1x bgp9 bs14">
        <ucDraw:Draw ID="ucdSSQ" runat="server" LotteryID="5" LotteryName="双色球" LotteryUrl="~/Lottery/Buy_SSQ.aspx" />
        <ucDraw:Draw ID="ucdCQSSC" runat="server" LotteryID="28" LotteryName="时时彩" LotteryUrl="~/Lottery/Buy_CQSSC.aspx" />
        <ucDraw:Draw ID="ucdFC3D" runat="server" LotteryID="6" LotteryName="福彩3D" LotteryUrl="~/Lottery/Buy_3D.aspx" />
        <ucDraw:Draw ID="ucdQLC" runat="server" LotteryID="13" LotteryName="七乐彩" LotteryUrl="~/Lottery/Buy_QLC.aspx" />
    </div>
    <div class="w970 pt10"><img src="/Images/_New/0000000_6.jpg" width="970" height="106" /></div>
    <div class="w970 pt10">
        <!--div class="bs15" style="height:315px;">
            <div class="title bg1x bgp7">合买中奖排名</div>
            <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
    	            <th width="19%">排名</th>
    	            <th width="36%" align="left">用户名</th>
    	            <th width="45%" align="left" class="abc0">中奖金额</th>
  	            </tr>
            </table>
        </div--->
        <div class="bs16" style="height:293px; border-bottom:1px solid #ccc;">
            <div class="title bg1x bgp1 afff"><span class="fs14 fw fl">合买推荐</span><span class="fs12 fr"><a href="/join">进入合买大厅</a></span></div>
            <div id="gbTab" class="tab">
        	    <span class="c" id="b_tab11" mid="5">双色球</span><span id="b_tab14" mid="28">时时彩</span><span id="b_tab12" mid="6">福彩3D</span><span id="b_tab13" mid="13">七乐彩</span>
            </div>
            <div class="cpContent">
                <table width="100%" border="0" border="0" cellspacing="0" cellpadding="0" class="tab_hemai" id="SchemeList">
                    <thead>
                        <tr>
                            <th>序号</th>
                            <th>发起人</th>
                            <th>发起人战绩<span id="Level"></span></th>
                            <th>倍数</th>
                            <th>方案金额<span id="Money"></span></th>
                            <th>每份金额</th>
                            <th>进度<span id="Schedule"></span></th>
                            <th>剩余份数</th>
                            <th>认购份数</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
                <div id="gbLoad" style="top: 50%; right: 50%; padding: 0px; margin: 0px; z-index: 999">
                    <img src="Images/ajax-loader.gif" alt="" />正在加载方案，请稍后...
                </div>
            </div>
        </div>
    </div>
    <div class="w970 bgFFF mt10">
	    <div class="bs17">
    	    <div class="t"><span class="fr">早8:00-凌晨2:00</span></div>
            <div class="list">
        	    <div class="a">
                    <a href="#">网购彩票安全吗</a><br />
                    <a href="#">集善彩票优势</a><br />
                    <a href="#">购彩凭证</a><br />
                    <a href="#">合买代购协议</a>
                </div>
                <div class="b">
                    <a href="Help/help_Buy.aspx">购彩流程</a><br />
                    <a href="Help/help_Send.aspx">如何注册</a><br />
                    <%--<a href="#">如何投注</a><br />--%>
                </div>
                <div class="c">
                    <a href="Help/help_Send.aspx">如何充值</a><br />
                    <a href="#">如何提款</a><br />
                    <a href="#">提款注意事项</a><br />
                    <a href="#">更换提款账户</a>
                </div>
                <div class="d">
                    <a href="#">注册信息</a><br />
                    <a href="#">密码修改</a><br />
                    <a href="#">会员积分</a>
                </div>
                <div class="e">010-85999326<br /></div>
            </div>
        </div>
    </div>
    <div class="w970 bt_in"><span><img src="/Images/_New/ico_02.gif" width="20" height="20" />&nbsp;购彩有风险，投资购彩需谨慎，18岁以下禁止购买。</span></div> 
    
 <%--   <div id="uLoginPn">
        <div class="r_content dl_h">
            <ul class="login_m">
                <li style="position: relative; overflow: visible;">
                    <span class="sp_l">用户名：</span>
                    <span class="sp_r">
                        <input id="r_login_u" type="text" class="login_input" name="u" />&nbsp; 
                        <b id="r_login_err" class="dl_err" style="display: none;"></b>
                        <span id="r_login_tip" class="dl_tip" style="display: none;">您输入的账户名和密码不匹配，请重新输入。</span> 
                    </span>
                </li>
                <li>
                    <span class="sp_l">密&nbsp;&nbsp;码：</span>
                    <span class="sp_r">
                        <input id="r_login_p" type="password" class="login_input" name="p" />
                    </span>
                    <a href="/ForgetPassword.aspx" target="_blank" class="gray" tabindex="-1">忘记密码？</a>
                </li>
            </ul>
            <div class="dl_bg">
                <input id="r_login_btn" type="submit" style="margin-right: 15px;" value="登 录" class="btn_Dora_b" />
            </div>
        </div>
    </div>--%>
<input id="imgCount" type="hidden" value="<%=imgCount %>" />
</form>

</body>
</html>
<script type="text/javascript">
var imgcounts = $("#imgCount").val();
    (function (m, M, w, o, T, h, ul, bt, a, t, d) {
        $(function (p, n) {
            ul = $('#slide_div ul');
            bt = $('#slide_btn .btn02');
            li = $('li', ul);
            a = $('a', ul);
            t = $('#slide_a');
            var g = $("img", a);
            g.attr('src', g.attr('url'));
            p = bt.last().next();
            n = p.next();
            for (i = 1; i < M; i++) {
                li.eq(i).remove()
            }
            d = auto();
            p.click(prev);
            n.click(next);
            ///$('a', p).hideFocus();
           // $('a', n).hideFocus();
            bt.click(pos)
        });
        function auto() {
            return setTimeout(next, o)
        }
        function pos(n, t) {
            if (h) {
                n = m; m = $(this).attr('target') - 0;
                if (m != n) {
                    set(m - n)
                }
            }
        }
        function prev() {
            if (h) {
                m--;
                m = m < 0 ? M - 1 : m;
                set(-1)
            }
        }
        function next() {
            if (h) {
                m++;
                m = m >= M ? 0 : m;
                set(1)
            }
        }
        function set(b, e) {
            h = 0; clearTimeout(d);
            ul.prepend(li.eq(m));
            b = ul.find('li');
            b.eq(1).fadeOut(T);
            b.eq(0).fadeIn(T, function () { b.eq(1).remove(); h = 1; d = auto() });
            bt.removeClass('btn01');
            bt.eq(m).addClass('btn01');
            e = a.eq(m);
            t.attr('href', e.attr('href'));
            t.html(e.attr('txt'));
            var g = $("img", e);
            g.attr('src', g.attr('url'));
        }
    })(0, imgcounts, 450, 1000 * imgcounts, '', 1);

    var myTimer;
    var speed = 5000; //速度毫秒 值越小速度越快
    var stepSpeed = 4; //值越大越快
    $(function () {
        var mybox = $(".scroll_box");
        //向上
        $(".scroll_up").bind("mouseover", function () {
            var nowPos = mybox[boxCount].scrollTop; //当前值
            changePos(mybox, nowPos, 0);
        }).bind("mouseout", function () {
            if (myTimer) { window.clearInterval(myTimer); }
        });
        //向下
        $(".scroll_down").bind("mouseover", function () {
            var nowPos = mybox[boxCount].scrollTop; //当前值
            var maxPos = mybox[boxCount].scrollHeight - mybox.outerHeight(); //最大值
            changePos(mybox, nowPos, maxPos);
        }).bind("mouseout", function () {
            if (myTimer) { window.clearInterval(myTimer); }
        });
    });

</script>
