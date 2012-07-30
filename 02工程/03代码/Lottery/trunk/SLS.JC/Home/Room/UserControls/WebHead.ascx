<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebHead.ascx.cs" Inherits="Home_Room_UserControls_WebHead" %>

<script type="text/javascript" src="/JScript/jquery-1.6.2.min.js"></script>
<script type="text/javascript" src="/JScript/head.js"></script>
<script type="text/javascript" src="/JScript/thickbox.js"></script>
<script type="text/javascript" src="/JScript/innerfade.js"></script>
<link type="text/css" rel="stylesheet" href="/Style/pub.css" />
<link type="text/css" rel="stylesheet" href="/Style/public.css" />
<link type="text/css" rel="stylesheet" href="/Style/part_b.css" />
<link type="text/css" rel="stylesheet" href="/Style/top_menu.css" />
<link type="text/css" rel="stylesheet" href="/Style/thickbox.css" />

<div class="b_top">
    <div class="w970">
        <div id="lbefore" class="fr afff"> 
            <a href="<%= System.Configuration.ConfigurationManager.AppSettings["JsHeaderUrl"] %>/account/login?returnUrl=<%=Request.Url.AbsoluteUri %>">登录</a> | <a href="/UserReg.aspx">注册</a>
        </div>
        <!--登录后-->
        <div id="lafter" class="fr afff">
            欢迎您, <span id="topUserName"><%= Name %></span>
            [<a href="/Home/Room/ViewAccount.aspx">我的账户</a>]
            [<a href="/Home/Room/OnlinePay/Alipay02/Send_Default.aspx" class="top_red">充值</a>]
            [<a id="topLoginOut" href="#">退出</a>]
            <a id="user_login_manger" href="/Admin/" style="display: none;">[超级管理]</a>
            <asp:Label ID="lbmessage" runat="server" ForeColor="Red"></asp:Label>
            <input id="topUserBala" type="hidden" />
            <input id="topUserDate" type="hidden" />
            <input id="topUserTime" type="hidden" />
        </div>
    </div>
</div>
<div class="w970 b_btop">
    <h1 class="fl"><img src="/Images/_New/q_logo.png" width="252" height="46" alt="" /></h1>
</div>
<div id="topMenuBar" class="w970 b_navs"> 
    <a id="mHome" href="/">首页</a>
    <a id="mPromo" target="_blank" href="<%= System.Configuration.ConfigurationManager.AppSettings["JsHeaderUrl"] %>/charity">有奖捐赠</a>
    <a id="mJoin" href="/Join">合买跟单</a>
    <%--<a id="mStat" href="/GuoGuan">过关统计</a>--%>
    <%--<a id="mMngm" href="/Top">跟单管家</a>--%>
    <%--<a id="mBattle" href="/Challenge">擂台比拼</a>--%>
    <a id="mWinRes" href="/WinQuery/OpenInfoList.aspx">全国开奖</a> 
    <a id="mChart" href="/TrendCharts">图表走势</a>
    <a id="mYear" href="/LotteryPackage.aspx">包年套餐</a>
    <a id="mHelp" class="last_child" href="/Help/Help_Default.aspx">帮助中心</a> 
</div>
<div class="w970 mt5">
<div class="bs10">
    <div class="af">彩种区</div>
    <div class="afr bg1x bgp8"> 
        <a href="/Lottery/Buy_SSQ.aspx">双色球</a> 
        <a class="a" href="/Lottery/Buy_CQSSC.aspx">时时彩</a> 
        <a class="b" href="/Lottery/Buy_3D.aspx">福彩3D</a> 
        <a class="c" href="/Lottery/Buy_QLC.aspx">七乐彩</a> 
    </div>
</div>
</div>
<input ID="currentMenu" runat="server" type="hidden" />

<div class="textclock" style="display:none;">
    <div class="Gmcptitle_top">
        <img src="/Home/Room/Images/menudh.jpg" alt="" name="Image93" width="230" height="32" border="0" id="Image93" />
        <div class="Gmcpcon_top" id="top_Gmcpcon" style="display:none">
         
          <div class="GmcpA_top"><div class="GmcpIMG_top2"><img src="/images/jc.jpg"  /></div>
          <div class="GmcpAcon2_top">
            <ul>
              <li><a href="/JCZC/buy_spf.aspx"><span style="color:#b54800">竞彩足球</span></a></li>
              <li><a href="/JCLC/buy_sf.aspx" ><span style="color:#b54800">竞彩篮球</span></a></li>
            </ul>
            </div>
          </div>
           <div class="GmcpA_top"><div class="GmcpIMG_top"><img src="/images/hc.jpg"  /></div>
          <div class="GmcpAcon_top">
            <ul>
              <li><a href="/Lottery/Buy_SSQ.aspx" ><span style="color:#b54800">双色球</span></a></li>
              <li><a href="/Lottery/Buy_3D.aspx" ><span style="color:#b54800">福彩3D</span></a></li>
	          <li><a href="/Lottery/Buy_QLC.aspx" ><span style="color:#b54800">七乐彩</span></a></li>              
            </ul>
            </div>
          </div>
          <div class="GmcpA_top">
            <div class="GmcpIMG_top"><img src="/images/gp.jpg" width="25" height="24"  /></div>
            <div class="GmcpAcon_top">
            <ul>
              <li><a href="/Lottery/Buy_SSC.aspx" ><span style="color:#b54800">时时彩</span></a></li>
              <li><a href="/Lottery/Buy_SYYDJ.aspx" ><span style="color:#b54800">十一运夺金</span></a></li>
	          <li><a href="/Lottery/Buy_JX11X5.aspx" ><span style="color:#b54800">11选5</span></a></li>
	          <li><a href="/Lottery/Buy_SSL.aspx" ><span style="color:#b54800">时时乐</span></a></li>
            </ul>
            </div>
          </div>
          <div class="GmcpA_top"><div class="GmcpIMG_top"><img src="/images/ct.jpg" width="27" height="28" /></div>
          <div class="GmcpAcon_top">
            <ul>
              <li><a href="/Lottery/Buy_SFC.aspx" ><span style="color:#b54800">足彩胜负</span></a></li>
              <li><a href="/Lottery/Buy_SFC_9_D.aspx" ><span style="color:#b54800">任选9场</span></a></li>
	          <li><a href="/Lottery/Buy_LCBQC.aspx" ><span style="color:#b54800">6场半全</span></a></li>
              <li><a href="/Lottery/Buy_JQC.aspx" ><span style="color:#b54800">4场进球</span></a></li>
            </ul>
            </div>
          </div>
           <div class="GmcpB_top"><div class="GmcpIMG_top"><img src="/images/sz.jpg" width="25" height="24" /></div>
           <div class="GmcpAcon_top">
            <ul>
              <li><a href="/Lottery/Buy_CJDLT.aspx" ><span style="color:#b54800">超级大乐透</span></a></li>
              <li><a href="/Lottery/Buy_QXC.aspx" ><span style="color:#b54800">七星彩</span></a></li>
              <li><a href="/Lottery/Buy_PL3.aspx"><span style="color:#b54800">排列三</span></a>/<a href="/Lottery/Buy_PL5.aspx"><span style="color:#b54800">五</span></a></li>
              <li><a href="/Lottery/Buy_22X5.aspx" ><span style="color:#b54800">22选5</span></a></li>
            </ul>
            </div>
           
          </div>
        </div>
    </div>
    <div class="scro_top" id="scro_tops">
        <%--<ul id="scroll_top" style="overflow:hidden;">--%>
            <%= sbWin %>
        <%--</ul>--%>
    </div>
</div>
<div id="DivUserinfo">
    <input id="head_HidUserID" name="head_HidUserID" value="-1" type="hidden" runat="server"/>
</div>
<!--header end-->
<!--未登录提示层-->
<div id="loginLay" style="display:none; width:360px;">
    <div class="tips_text">
        <div id="error_tips" class="dl_tips" style="display: none;"><b class="dl_err"></b>您输入的账户名和密码不匹配，请重新输入。</div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="dl_tbl">
            <tr>
                <td style="width: 70px;">用户名：</td>
                <td><input id="lu" name="lu" type="text" class="tips_txt" /></td>
                <td class="t_ar"><a href="/UserReg.aspx" target="_blank" tabindex="-1">免费注册</a></td>
            </tr>
            <tr>
                <td>密&nbsp;&nbsp;码：</td>
                <td><input id="lp" name="lp" type="password" class="tips_txt" /></td>
                <td class="t_ar"><a href="/ForgetPassword.aspx" target="_blank" tabindex="-1">忘记密码</a></td>
            </tr>
            <tr>
                <td>验证码：</td>
                <td colspan="2">
                    <input id="yzmtext" name="c" type="text" class="tips_yzm" />
                    <img id="yzmimg" alt="验证码" src="about:blank" style="cursor:pointer;width:60px;height:25px" />
                    <a id="yzmup" class="kbq" href="#">看不清，换一张</a>
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2"><input id="floginbtn" type="button" class="btn_Dora_b" value="登 录" style="margin-right: 18px" /></td>
                <td></td>
            </tr>
        </table>
    </div>
</div>
<!--投注信息提示层-->
<div class="tab_fangan" style="display:none;" id="tab_box">
	<div class="tab_fangan_cont">
		<dl><span class="name">注数：</span><span class="cont" id="span_zs" name="span_zs"></span></dl>
		<dl><span class="name">倍数：</span><span class="cont" id="span_bs" name="span_bs"></span></dl>
		<dl><span class="name">总金额：</span><span class="cont" id="span_totalmoney" name="span_totalmoney"></span>元</dl>
		<dl><span class="name">总份数：</span><span class="cont" id="span_share" name="span_share"></span></dl>
		<dl><span class="name">每份：</span><span class="cont" id="span_sharemoney" name="span_sharemoney"></span>元</dl>
		<dl><span class="name">保底：</span><span class="cont" id="span_assuremoney" name="span_assuremoney"></span>元</dl>
		<dl><span class="name">购买：</span><span class="cont" id="span_buymoney" name="span_buymoney"></span>元</dl>
	</div>
	<div class="tab_fangan_btn">
		<dl class="tip">按"确定"表示立即提交方案，确定要提交方案吗？</dl>
		<dl class="btn"><span class="btn_tj"><input name="btn_BuyConfirm" type="button" value="确定" id="btn_BuyConfirm" /></span></dl>
	</div>
</div>
<!--充值提示层-->
<div class="tab_fangan2" style="display:none;" id="tab_box2">
	<div class="tab_fangan_nav">
		<h2>提示！</h2>
		<dl><span class="close" onclick="document.getElementById('tab_box2').style.visibility='hidden';document.getElementById('mask').style.visibility='hidden'">关闭</span></dl>
	</div>
	<div class="tab_fangan_cont2">
		<dl>您的余额不足，请充值后再投注！</dl>
		<dt><span style=" padding:0 15px 0 0;"><a href="#">马上充值</a></span><span class="btn_tj"><input name="" type="button" value="确定" onclick="document.getElementById('tab_box2').style.visibility='hidden';document.getElementById('mask').style.visibility='hidden'" /></span></dt>
	</div>
</div>
<!--购买成功提示层-->
<div class="tips_m" style="display:none;" id="dlg_buysuc">
      <div class="tips_info">
<div class="icon_suc" id="dlg_buysuc_content"><div class="txt_suc">您好，恭喜您购买成功!</div>祝您中大奖！</div>
<div class="suc_link">您还可以选择：<a href="javascript:location.reload();" id="dlg_buysuc_back">返回继续购买</a> <br />
查看我的帐户：<a href="/Home/Room/AccountDetail.aspx" target="_blank">帐户明细</a> | <a href="/Home/Room/Invest.aspx" target="_blank">投注记录</a> | <a href="/Home/Room/OnlinePay/Default.aspx" target="_blank">在线充值</a></div>
      </div>
</div>
<!--温馨提示-->
<div id="info_dlg" style="top:400px;position:absolute;width: 550px;display:none;">
    <div class="tips_box">
        <div class="alert_c">
        	<div class="state error">
       		 <div class="stateInfo f14 p_t10" id="info_dlg_content"></div>
   		  </div>
        </div>
        <div class="tips_sbt">
            <input id="info_dlg_ok" type="button" class="btn_Dora_b" value="确 定" />
        </div>
    </div>
</div>
<!--提示确认-->
<div class="tips_m" style="display:none" id="confirm_dlg">
    <div class="tips_box">
        <div class="tips_info"  id="confirm_dlg_content"></div>
        <div class="tips_sbt">
            <input type="button" value="取 消" class="btn_Lora_b"  id="confirm_dlg_no" /><input type="button" value="确 定" class="btn_Dora_b"  id="confirm_dlg_yes" />
        </div>
    </div>
</div>
