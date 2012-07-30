<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WinNotice1.aspx.cs" Inherits="Home_Room_WinNotice1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Style/NewPage.css" rel="stylesheet" type="text/css" />

    <script src="../../JScript/jsScroller.js" type="text/javascript"></script>

    <script type="text/javascript">
        var scroller = null;
        window.onload = function() {
            var el = document.getElementById("Scroller-1");
            scroller = new jsScroller(el, 100, 200);
        }

        function setTab(name, cursel, n) {
            for (i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("zjj_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
        }

        var $ = function(d) {
            typeof d == "string" && (d = document.getElementById(d));
            return $.fn.call(d);
        };
        $.fn = function() {//附加2个方法
            this.$ADD = function(fn) { CLS.add(this, fn) };
            this.addEvent = function(sEventType, fnHandler) {
                if (this.addEventListener) { this.addEventListener(sEventType, fnHandler, false); }
                else if (this.attachEvent) { this.attachEvent("on" + sEventType, fnHandler); }
                else { this["on" + sEventType] = fnHandler; }
            }
            this.removeEvent = function(sEventType, fnHandler) {
                if (this.removeEventListener) { this.removeEventListener(sEventType, fnHandler, false); }
                else if (this.detachEvent) { this.detachEvent("on" + sEventType, fnHandler); }
                else { this["on" + sEventType] = null; }
            }
            return this;
        };
        var Class = { create: function() { return function() { this.initialize.apply(this, arguments); } } };
        var Bind = function(obj, fun, arr) { return function() { return fun.apply(obj, arr); } }
        var Marquee = Class.create();
        Marquee.prototype = {
            initialize: function(id, name, out, speed) {
                this.name = name;
                this.box = $(id);
                this.out = out;
                this.speed = speed;
                this.d = 1;
                this.box.style.position = "relative";
                this.box.scrollTop = 0;
                var _li = this.box.firstChild;
                while (typeof (_li.tagName) == "undefined") _li = _li.nextSibling;
                this.lis = this.box.getElementsByTagName(_li.tagName);
                this.len = this.lis.length;
                for (var i = 0; i < this.lis.length; i++) {//计算该复制多少节点，保证无缝滚动，没必要的就不复制
                    var __li = document.createElement(_li.tagName);
                    __li.innerHTML = this.lis[i].innerHTML;
                    this.box.appendChild(__li);
                    if (this.lis[i].offsetTop >= this.box.offsetHeight) break;
                }
                this.Start();
                this.box.addEvent("mouseover", Bind(this, function() { clearTimeout(this.timeout); }, []));
                this.box.addEvent("mouseout", Bind(this, this.Start, []));
            },
            Start: function() {
                clearTimeout(this.timeout);
                this.timeout = setTimeout(this.name + ".Up()", this.out * 1000)
            },
            Up: function() {
                clearInterval(this.interval);
                this.interval = setInterval(this.name + ".Fun()", 10);
            },
            Fun: function() {
                this.box.scrollTop += this.speed;
                if (this.lis[this.d].offsetTop <= this.box.scrollTop) {
                    clearInterval(this.interval);
                    this.box.scrollTop = this.lis[this.d].offsetTop;
                    this.Start();
                    this.d++;
                }
                if (this.d >= this.len + 1) {
                    this.d = 1;
                    this.box.scrollTop = 0;
                }
            }
        };
    </script>

</head>
<body style="margin: 0px;">
    <form id="form1" runat="server">
    <div id="jiang" style="margin: 0px">
        <h2>
            最新开奖</h2>
        <div id="tab_jiang">
            <div class="Menubox11">
                <ul>
                    <li id="eleven1" onmouseover="setTab('eleven',1,2)">数字彩</li>
                    <li id="eleven2" onmouseover="setTab('eleven',2,2)">竞技彩</li>
                </ul>
            </div>
            <div class="Contentbox11">
                <div id="zjj_eleven_1">
                    <div class="Container">
                        <img src="../../Images/btn_up.gif" class="Scrollbar-Up" width="114" height="26" onmouseover="scroller.startScroll(0, 5);"
                            onmouseout="scroller.stopScroll();" />
                        <img src="../../Images/btn_down.gif" class="Scrollbar-Down" onmouseover="scroller.startScroll(0, -5);"
                            onmouseout="scroller.stopScroll();" />
                        <div id="Scroller-1" style="margin-top: 10px; overflow: hidden; height: 326px;">
                            <div class="Scroller-Container">
                                <h3>
                                    <span class="blue1">福彩3D</span>第
                                    <asp:Label ID="lblFC3D" runat="server"></asp:Label>期
                                </h3>
                                <ul>
                                    <li>
                                        <span class="winnumber" runat="server" id="spFCWinNumber"></span>  <span class="blue1">试机号：</span><span runat="server" id="spFCTest" class="red_bold"></span>
                                    </li>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_3D.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/6-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">双色球</span>第<asp:Label ID="lbSSQ" runat="server"></asp:Label>期</h3>
                                <ul>
                                    <span class="winnumber" runat="server" id="spSSQWinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_SSQ.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/5-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">七乐彩</span>第
                                    <asp:Label ID="lbQLC" runat="server"></asp:Label>
                                    期</h3>
                                <ul>
                                    <span class="winnumber" runat="server" id="spQLCWinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_QLC.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/13-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">河南福彩22选5</span>第
                                    <asp:Label ID="lbHNFC22X5" runat="server"></asp:Label>
                                    期</h3>
                                <ul>
                                    <span class="winnumber" runat="server" id="spHNFC22X5WinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_15X5.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/59-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">体彩大乐透</span>第
                                    <asp:Label ID="lbTCDTL" runat="server"></asp:Label>
                                    期</h3>
                                <ul>
                                    <span class="winnumber" runat="server" id="spTCDTLWinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_CJDLT.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/39-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">排列3</span>第
                                    <asp:Label ID="lbPl3" runat="server"></asp:Label>
                                    期</h3>
                                <ul>
                                    <span class="winnumber" runat="server" id="spPl3WinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_PL3.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/63-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">排列5</span>第
                                    <asp:Label ID="lbPl5" runat="server"></asp:Label>
                                    期</h3>
                                <ul>
                                    <span class="winnumber" runat="server" id="spPl5WinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_PL5.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/64-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">七星彩</span>第
                                    <asp:Label ID="lbQxC" runat="server"></asp:Label>
                                    期</h3>
                                <ul>
                                    <span class="winnumber" runat="server" id="spQxCWinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_QXC.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/3-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">体彩22选5</span>第
                                    <asp:Label ID="lbTC22_5" runat="server"></asp:Label>
                                    期</h3>
                                <ul>
                                    <span class="winnumber" runat="server" id="spTC22_5WinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_22X5.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/9-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="zjj_eleven_2" style="display: none">
                    <div class="Container">
                        <img src="../../Images/btn_up.gif" class="Scrollbar-Up" width="114" height="26" onmouseover="scroller.startScroll(0, 5);"
                            onmouseout="scroller.stopScroll();" />
                        <img src="../../Images/btn_down.gif" class="Scrollbar-Down" onmouseover="scroller.startScroll(0, -5);"
                            onmouseout="scroller.stopScroll();" />
                        <div id="Scroller-2" style="margin-top: 10px; overflow: hidden;">
                            <div class="Scroller-Container">
                                <h3>
                                    <span class="blue1">胜负彩(任九)</span>第<asp:Label ID="lblSFCRJ" runat="server"></asp:Label> 期
                                </h3>
                                <ul>
                                    <span class="myspan" runat="server" id="spSFCRJWinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_SFC.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/1-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">六场半全场</span>第<asp:Label ID="lblLCBQC" runat="server"></asp:Label> 期</h3>
                                <ul>
                                <span class="myspan" runat="server" id="spLCBQCWinNumber"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_LCBQC.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/15-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                                <h3>
                                    <span class="blue1">四场进球彩</span>第<asp:Label ID="lblSCJQC" runat="server"></asp:Label> 期</h3>
                                <ul>
                                <span class="myspan" runat="server" id="spSCJQC"></span>
                                    <li>
                                        <h5>
                                            <span><a href="../../Lottery/Buy_JQC.aspx" target="_blank">我要购买</a></span> <span><a href="../../WinQuery/2-0-0.aspx" target="_blank">
                                                中奖查询</a></span></h5>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
