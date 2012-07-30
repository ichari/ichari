<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dt.aspx.cs" Inherits="JCZC_dt" %>
<%@ Register src="../Home/Room/UserControls/WebHead.ascx" tagname="WebHead" tagprefix="uc1" %>
<%@ Register src="../Home/Room/UserControls/WebFoot.ascx" tagname="WebFoot" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
/* body */
html { margin:0 auto; font-size:14px; font-family: Arial, Helvetica, sans-serif, "宋体"; color:#333;min-height:100%; background:#ffffff;}
* { padding:0; margin:0; }
ul, li, p, h1, dl, dt, dd { margin:0px; padding:0px; list-style:none; }
img { border:0px; }
.cl { clear:both; height:0; font-size: 1px; line-height: 0px; }
.lt { float:left; text-align:left; }
.rt { float:right; text-align: right; }
.ct { text-align:center; width:100%; }
.fb{ font-weight:bold;}
.none { display:none;}
/*----mqin_mhdt-----*/
.mqin_main { width:1000px; height:auto; margin:0 auto; padding-top:20px; color:#333; border:1px solid #c0cfde; background:#fff;}
	.mqin_cont{ clear:both; padding:0 20px;}
	.mqin_cont h2{ clear:both; height:auto; line-height:30px; font-size:18px; text-align:center;}
	.mqin_cont dl{ clear:both; height:auto; line-height:20px; font-size:12px; text-align:center; padding:0 0 20px 0; color:#999;}
	.mqin_cont p{ padding:0 0 10px 0; text-indent:2em; line-height:20px;}
</style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:WebHead ID="WebHead1" runat="server" />
    <!--内容-->
<div class="user_mainboth" style="padding-top:8px;">
	<div class="mqin_main">
	<div class="mqin_cont">
		<h2>模糊设胆功能介绍</h2>
		<dl>日期：2011-08-03 &nbsp;&nbsp; 来源：湖体</dl>
		<p>用户投注"胆拖"投注功能时，常会遇到这样一个问题，有的时候"拖"场次全对了，但"胆"场次却出错了，结果与头奖失之交臂。"模糊设胆"功能正是考虑到用户的实际问题，在"胆拖"投注功能的基础上进行了升级。</p>
		<p>用户可以针对"胆"场次进行容错设置，使得"胆"在错一部分的情况下，依然保证能命中大奖。同时，与"胆拖"投注功能结合，使得用户可以一次中得多注奖金。</p>
		<p class="fb">简单的来说：选择9场赛事，设其中5场为胆，模糊设胆选择"至少含4胆"，过关方式选择5串1，其中"至少含4胆"意思就是5场胆容错随便1场。如果模糊设胆选择"至少含3胆"，意思就是5场胆容错随便2场。</p>
		<p>"模糊设胆"使用说明如下图所示：</p>
		<p>第一步：选择投注赛事</p>
		<p><img src="images/mhdt_pic1.jpg" width="524" height="346" /><br /></p>
		<p>第二步：选择设胆场次</p>
		<p><img src="images/mhdt_pic2.jpg" width="524" height="346" /><br /></p>
		<p>第三步：选择模糊设胆方式</p>
		<p><img src="images/mhdt_pic3.jpg" width="524" height="439" /><br /></p>
		<p>第四步：选择过关方式进行投注</p>
		<p><img src="images/mhdt_pic4.jpg" width="452" height="439" /><br /></p>
		<br /><br /><br />
	</div>
	</div>
</div>
    <uc2:WebFoot ID="WebFoot1" runat="server" />
    </form>
</body>
</html>
