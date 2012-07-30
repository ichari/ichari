<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Head.ascx.cs" Inherits="CPS_UserControls_Head" %>
<style type="text/css">
    .container, .container *
    {
        margin: 0;
        padding: 0;
    }
    .container
    {
        width: 981px;
        height: 204px;
        overflow: hidden;
        position: relative;
        background-color: #FFFA34;
    }
    .slider
    {
        position: absolute;
        height: 204px;
        width: 981px;
    }
    .slider li
    {
        list-style: none;
        display: inline;
        height: 204px;
        width: 981px;
        background-color: #FFFA2D;
    }
    .slider img
    {
        width: 981px;
        height: 204px;
        display: block;
    }
    .slider2
    {
        width: 2000px;
    }
    .slider2 li
    {
        float: left;
    }
    .num
    {
        position: absolute;
        right: 5px;
        bottom: 5px;
    }
    .num li
    {
        float: left;
        color: #FF7300;
        text-align: center;
        line-height: 16px;
        width: 16px;
        height: 16px;
        font-family: Arial;
        font-size: 12px;
        cursor: pointer;
        overflow: hidden;
        margin: 3px 1px;
        border: 1px solid #FF7300;
        background-color: #fff;
    }
    .num li.on
    {
        color: #fff;
        line-height: 21px;
        width: 21px;
        height: 21px;
        font-size: 16px;
        margin: 0 1px;
        border: 0;
        background-color: #FF7300;
        font-weight: bold;
    }
</style>
<div id="box">
    <div id="top">
        <div id="top_logo">
            <table width="1002" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="484" height="111">
                        <a href="<%= ResolveUrl("~/") %>" target="_blank">
                            <img src="<%= ResolveUrl("~/CPS/Images/index-1_01.gif") %>" width="484" height="111"
                                border="0" /></a>
                    </td>
                    <td width="259">
                        <a href="<%= ResolveUrl("~/CPS/") %>" target="_blank">
                            <img src="<%= ResolveUrl("~/CPS/Images/index-1_02.gif") %>" width="372" height="111"
                                border="0" /></a>
                    </td>
                    <td width="259" valign="bottom">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td height="40" align="center">
                                    <span class="hui_2"><a style="cursor: hand" onclick="window.external.AddFavorite(location.href,'兼职推广联盟');"
                                        class="hui12">加入收藏</a></span>&nbsp;<span class="hui_2">|<a style="cursor: hand" onclick="this.style.behavior='url(#default#homepage)';this.setHomePage('http://www.3gcpw.com');">
                                            设为首页 </a></span>
                                </td>
                            </tr>
                            <tr>
                                <td height="70" align="center">
                                    <a href="javascript:;" onclick="try{parent.closeMini();}catch(e){;}this.newWindow = window.open('http://chat10.live800.com/live800/chatClient/chatbox.jsp?companyID=86584&configID=149924&jid=8794095338&enterurl=http%3A%2F%2Flocalhost%3A2003%2FSLS%2EIcaile%2FHome%2FWeb%2FDefault%2Easpx', 'chatbox86584', 'toolbar=0');this.newWindow.focus();this.newWindow.opener=window;return false;">
                                        <img src="<%= ResolveUrl("~/CPS/Images/contact.gif") %>" width="68" height="63" border="0" /></a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<div id="top_men">
    <table width="1002" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <table width="727" border="0" align="left" cellpadding="0" cellspacing="0" id="tbMenu">
                    <tr>
                        <td width="97" height="39" align="center" id="tdTGSY" class="bai14">
                            <p style="padding-top: 2px;">
                                <a href="<%= ResolveUrl("~/CPS/Default.aspx") %>">推广首页</a></p>
                        </td>
                        <td width="6" align="center" style="background-image: url('Images/index-1_08.gif')">
                        </td>
                        <td width="97" height="39" align="center" class="bai14" id="tdXWGG">
                            <p style="padding-top: 2px;">
                                <a href="<%= ResolveUrl("~/CPS/News.aspx") %>">新闻公告</a></p>
                        </td>
                        <td width="6" align="center" style="background-image: url('Images/index-1_08.gif')">
                        </td>
                        <td width="97" height="39" align="center" class="bai14" id="tdSQTG">
                            <p style="padding-top: 2px;">
                                <a href="<%= ResolveUrl("~/CPS/UserRegCPS.aspx") %>">申请推广</a></p>
                        </td>
                        <td width="6" align="center" style="background-image: url('Images/index-1_08.gif')">
                        </td>
                        <td width="97" height="39" align="center" class="bai14" id="tdGGYS">
                            <p style="padding-top: 2px;">
                                <a href="<%= ResolveUrl("~/CPS/ImageCode.aspx") %>">广告样式</a></p>
                        </td>
                        <td width="6" align="center" style="background-image: url('Images/index-1_08.gif')">
                        </td>
                        <td width="97" height="39" align="center" class="bai14" id="tdTGZX">
                            <p style="padding-top: 2px;">
                                <a href="<%= ResolveUrl("~/CPS/Admin/Default.aspx") %>">推广中心</a></p>
                        </td>
                        <td width="6" align="center" style="background-image: url('Images/index-1_08.gif')">
                        </td>
                        <td width="97" height="39" align="center" class="bai14" id="tdZXZX">
                            <p style="padding-top: 2px;">
                                <a href="http://www.3gcpw.com/Club/showforum-14.aspx" target="_blank">在线咨询</a></p>
                        </td>
                        <td width="6" align="center" style="background-image: url('Images/index-1_08.gif')">
                        </td>
                        <td width="97" height="39" align="center" class="bai14">
                            <p style="padding-top: 2px;">
                                <a href="<%= ResolveUrl("~/BotNav/contact.html") %>" target="_blank">
                                    联系我们</a></p>
                        </td>
                        <td width="11" align="center" class="bai14">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<div id="box_1">
    <table width="981" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <div class="container" id="idTransformView">
                    <ul class="slider" id="idSlider">
                        <li>
                            <img src="<%= ResolveUrl("~/CPS/Images/index_banner01.jpg") %>" width="981" height="204" alt=""/></li>
                        <li>
                            <img src="<%= ResolveUrl("~/CPS/Images/index_banner02.jpg") %>" width="981" height="204" alt=""/></li>
                        <li>
                            <img src="<%= ResolveUrl("~/CPS/Images/index_banner03.jpg") %>" width="981" height="204" alt=""/></li>
                    </ul>
                    <ul class="num" id="idNum">
                        <li>1</li>
                        <li>2</li>
                        <li>3</li>
                    </ul>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table width="981" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <img src="<%= ResolveUrl("~/CPS/Images/index-1_19.gif") %>" width="119" height="44" alt=""/>
                        </td>
                        <td>
                            <img src="<%= ResolveUrl("~/CPS/Images/index-1_20.gif") %>" width="113" height="44" alt=""/>
                        </td>
                        <td>
                            <img src="<%= ResolveUrl("~/CPS/Images/index-1_21.gif") %>" width="156" height="44" alt=""/>
                        </td>
                        <td>
                            <img src="<%= ResolveUrl("~/CPS/Images/index-1_22.gif") %>" width="155" height="44" alt=""/>
                        </td>
                        <td>
                            <img src="<%= ResolveUrl("~/CPS/Images/index-1_23.gif") %>" width="155" height="44" alt=""/>
                        </td>
                        <td>
                            <img src="<%= ResolveUrl("~/CPS/Images/index-1_24.gif") %>" width="134" height="44" alt=""/>
                        </td>
                        <td>
                            <img src="<%= ResolveUrl("~/CPS/Images/index-1_25.gif") %>" width="150" height="44" alt=""/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">
    var $ = function (id) {
        return "string" == typeof id ? document.getElementById(id) : id;
    };

    var Class = {
    create: function() {
    return function() {
      this.initialize.apply(this, arguments);
    }
    }
    }

    Object.extend = function(destination, source) {
    for (var property in source) {
	    destination[property] = source[property];
    }
        return destination;
    }

    var TransformView = Class.create();
    TransformView.prototype = {
    //容器对象,滑动对象,切换参数,切换数量
    initialize: function(container, slider, parameter, count, options) {
    if(parameter <= 0 || count <= 0) return;
    var oContainer = $(container), oSlider = $(slider), oThis = this;

    this.Index = 0;//当前索引

    this._timer = null;//定时器
    this._slider = oSlider;//滑动对象
    this._parameter = parameter;//切换参数
    this._count = count || 0;//切换数量
    this._target = 0;//目标参数

    this.SetOptions(options);

    this.Up = !!this.options.Up;
    this.Step = Math.abs(this.options.Step);
    this.Time = Math.abs(this.options.Time);
    this.Auto = !!this.options.Auto;
    this.Pause = Math.abs(this.options.Pause);
    this.onStart = this.options.onStart;
    this.onFinish = this.options.onFinish;

    oContainer.style.overflow = "hidden";
    oContainer.style.position = "relative";

    oSlider.style.position = "absolute";
    oSlider.style.top = oSlider.style.left = 0;
    },
    //设置默认属性
    SetOptions: function(options) {
    this.options = {//默认值
	    Up:			true,//是否向上(否则向左)
	    Step:		5,//滑动变化率
	    Time:		10,//滑动延时
	    Auto:		true,//是否自动转换
	    Pause:		3000,//停顿时间(Auto为true时有效)
	    onStart:	function(){},//开始转换时执行
	    onFinish:	function(){}//完成转换时执行
    	
    };
    Object.extend(this.options, options || {});
    },
    //开始切换设置
    Start: function() {
    if(this.Index < 0){
	    this.Index = this._count - 1;
    } else if (this.Index >= this._count){ this.Index = 0; }

    this._target = -1 * this._parameter * this.Index;
    this.onStart();
    this.Move();
    },
    //移动
    Move: function() {
    clearTimeout(this._timer);
    var oThis = this, style = this.Up ? "top" : "left", iNow = parseInt(this._slider.style[style]) || 0, iStep = this.GetStep(this._target, iNow);
    	
    if (iStep != 0) {
	    this._slider.style[style] = (iNow + iStep) + "px";
	    this._timer = setTimeout(function(){ oThis.Move(); }, this.Time);
    } else {
	    this._slider.style[style] = this._target + "px";
	    this.onFinish();
	    if (this.Auto) { this._timer = setTimeout(function(){ oThis.Index++; oThis.Start(); }, this.Pause); }
    }
    },
    //获取步长
    GetStep: function(iTarget, iNow) {
    var iStep = (iTarget - iNow) / this.Step;
    if (iStep == 0) return 0;
    if (Math.abs(iStep) < 1) return (iStep > 0 ? 1 : -1);
    return iStep;
    },
    //停止
    Stop: function(iTarget, iNow) {
    clearTimeout(this._timer);
    this._slider.style[this.Up ? "top" : "left"] = this._target + "px";
    }
    };

    window.onload=function(){
    function Each(list, fun){
	    for (var i = 0, len = list.length; i < len; i++) { fun(list[i], i); }
    };

    var objs = $("idNum").getElementsByTagName("li");


    var tv = new TransformView("idTransformView", "idSlider",204, 3, {
	    onStart : function(){ Each(objs, function(o, i){ o.className = tv.Index == i ? "on" : ""; }) }//按钮样式
    });

    tv.Start();

    Each(objs, function(o, i){
	    o.onmouseover = function(){
		    o.className = "on";
		    tv.Auto = false;
		    tv.Index = i;
		    tv.Start();
	    }
	    o.onmouseout = function(){
		    o.className = "";
		    tv.Auto = true;
		    tv.Start();
	    }
    })
    }
    var url = window.location.href.toLowerCase();

    var arr = ["tdTGSY", "tdXWGG", "tdSQTG", "tdGGYS", "tdTGZX", "tdZXZX"];

    for (var i = 0; i < arr.length; i++) {
        document.getElementById(arr[i]).background = "";
    }
   
    var j = -1;
    if (url.indexOf("/news.aspx") > -1) { 
        j = 1;
    }
    else if(url.indexOf("/userregcps.aspx")>-1)
    {
        j = 2;
    }
    else if(url.indexOf("/imagecode.aspx")>-1||url.indexOf("/wordcode.aspx")>-1||url.indexOf("/customcode.aspx")>-1)
    {
        j = 3;
    }
    else if(url.indexOf("/admin/")>-1)
    {
        j = 4;
    }
    else 
    {
        j = 0;
    }
    
    document.getElementById(arr[j]).background = <%=ImgUrl %> + "index-1_06.gif";
    document.getElementById(arr[j]).className = "blue14";
</script>

