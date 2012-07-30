<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Project_List.aspx.cs" Inherits="Join_Project_List" %>

<%@ Register Src="../Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="../Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>合买</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/hemai.css" rel="stylesheet" type="text/css" />
    <link href="Style/pagination.css" rel="stylesheet" type="text/css" />
</head>
<body class="gybg">
<form id="form1" runat="server">
<uc1:WebHead ID="WebHead1" runat="server" />
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div class="bsc6s">
    	<div class="bsc6">
            <div class="title bg1x bgp3 afff"><strong>推荐方案</strong></div>
            <p><a href="Project_List.aspx?id=5"><img src="/Images/_New/ben_09.png" alt="" width="105" height="122"></a></p>
            <p><a href="Project_List.aspx?id=28"><img src="/Images/_New/ben_12.png" alt="" width="130" height="120"></a></p>
            <p><a href="Project_List.aspx?id=6"><img src="/Images/_New/ben_10.png" alt="" width="105" height="122"></a></p>
            <p><a href="Project_List.aspx?id=13"><img src="/Images/_New/ben_11.png" alt="" width="116" height="120"></a></p>                    
        </div>
        <div class="bsc7s">
        <div class="nav_tit">
    	    <h2><span class="tit"><%= IsuseInfo %></span></h2>
            <dl>
                <!--<asp:HyperLink runat="server" ID="hlUrl" NavigateUrl="/JCZC/buy_spf.aspx">发起合买&gt;&gt;</asp:HyperLink>-->
            </dl>
        </div>
        <!--合买列表-->
        <div class="h_main">
            <div class="TabTitle  bg1x bgp5">
                <ul id="myTab">
                    <li class="active" mid="">全部方案</li>
                    <%= PlayType %>
                </ul>
            </div>
            <!--方案表-->
            <div class="TabContent">
                <div class="f_search">
                    <span class="so">
                        <input type="text" class="soip" value="请输入用户名" id="SerachCondition" name="SerachCondition" onfocus="this.value=='请输入用户名'?this.value='':this.value"
                            onblur="this.value==''?this.value='请输入用户名':this.value"/>
                        <input type="button" id="srearch" name="srearch" class="btn_so" value="搜索" />
                    </span>
                    <span class="f_sel">
                        <select name="state_term" id="state_term" class="m-r">
                            <option value="100">满员</option>
                            <option value="1" selected="selected">未满员</option>
                            <option value="2">已撤单</option>
                            <option value="-1">全部</option>
                        </select>
                    </span>
                </div>
                <div>
                    <table width="100%" cellspacing="0" cellpadding="0" class="tab_hemai" id="SchemeList">
                        <thead>
                            <tr>
                                <th>
                                    序号
                                </th>
                                <th>
                                    发起人
                                </th>
                                <th onclick="Sort('Level','Level');return false;">
                                    发起人战绩<span id="Level"></span>
                                </th>
                                <th>
                                    玩法
                                </th>
                                <th>
                                    倍数
                                </th>
                                <th onclick="Sort('Money','Money');return false;">
                                    方案金额<span id="Money"></span>
                                </th>
                                <th>
                                    每份金额
                                </th>
                                <th onclick="Sort('Schedule','Schedule');return false;">
                                    方案进度<span id="Schedule"></span>
                                </th>
                                <th>
                                    剩余份数
                                </th>
                                <th>
                                    认购份数
                                </th>
                                <th>
                                    操作
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div id="divload" style="top: 38%; right: 52%; position: absolute; padding: 0px; margin: 0px; z-index: 999">
                            <img src="Images/ajax-loader.gif" alt="" />正在加载方案，请稍后...
                        </div>
                </div>
                <div>
                    <div class="numb" id="EachPageNum">
                        单页方案个数：<a title="" style="cursor: pointer;" class="current">20</a><a
                            title="" style="cursor: pointer;">30</a><a title="" style="cursor: pointer;">40</a></div>
                    <div style="float:right;padding-bottom: 3px; padding-top: 3px; margin-right:10px;">
                        <span><input id="govalue" name="Btn_Go" type="text" style="width:30px;" /><input type="button" id="Btn_Go" value="GO" class="goto" /></span></div>
                        <div id="Pagination" class="yahoo2" style="width: auto; float:right;">
                    </div>
                </div>
                <div class="cl">
                </div>
            </div>
        </div>





         </div>

</div>

</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>




    <div class="wrap">
         <!--方案彩种-->
	    <div class="fangan_class">
            <dl class="f_class">
        	    <em>推荐方案：</em>
                 <span mid="72"><a href="?id=72">竞彩足球</a></span>
                 <span mid="73"><a href="?id=73">竞彩篮球</a></span>
                <span mid="74"><a href="?id=74">足彩胜负</a></span>
                <span mid="75"><a href="?id=75">任选9场</a></span>
                <span mid="3"><a href="?id=3">七星彩</a></span>
                <span mid="39"><a href="?id=39">大乐透</a></span>
                <span mid="63"><a href="?id=63">排列三</a></span>
                <span mid="64"><a href="?id=64">排列五</a></span>
                <span mid="2"><a href="?id=2">4场进球</a></span>
                <span mid="15"><a href="?id=15">6场半全</a></span>
                <span mid="5"><a href="?id=5">双色球</a></span>
                <span mid="6"><a href="?id=6">福彩3D</a></span>
                <span mid="13"><a href="?id=13">七乐彩</a></span>
            </dl>
  	    </div>
       
    </div>
    <input type="hidden" id="hidLotteryID" runat="server" />
    <uc2:WebFoot ID="WebFoot1" runat="server" />
    <script src="JScript/Pagination.js" type="text/javascript"></script>
    <script src="JScript/ProjectList.js" type="text/javascript"></script>
    </form>
</body>
</html>
