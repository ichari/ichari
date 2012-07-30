<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ZX.aspx.cs" Inherits="ZX" %>

<%@ Register Src="Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Title%></title>
    <link href="Style/zx.css" rel="stylesheet" type="text/css" />
    <link href="/Challenge/Style/pagination.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:WebHead ID="WebHead1" runat="server" />
    <div style="padding-top:10px;"></div>
    <div style="height:auto; margin:0 auto; clear:both; font-size:12px; width:1002px;">
    <!--当前位置开始-->
    <div class="location">
        您现在的位置：<span class="sp13"><a href="/">首页</a></span>&gt;<span class="sp13"><%=Title%>资讯</span></div>
    <!--当前位置结束-->
    <!--足球单场资讯部分开始-->
    <div class="container">
        <div class="sub_left">
            <div class="ssq_news">
                <div class="M_box">
                    <div class="M_title">
                        <h2><%=Title%></h2>
                    </div>
                    <div class="M_content">
                        <div class="news_list" id="news_list" style="font-family:微软雅黑; font-size:12px; height:auto;">
                            <%=New%>
                        <div id="Pagination" class="yahoo">
                            <%=pageHtml %>   
                        </div>
                        </div>                        
                    </div>
                </div>
            </div>
        </div>
        <div class="sub_right">
            <!--新闻右边图片-->
            <div class="news_right_ad">
                <div class="news_Lnav">
	                <ul>
		                <li class="icon_1"><a href="/ZX.aspx?NewsName=jczq">竞彩足球资讯</a></li>
		                <li class="icon_2"><a href="/ZX.aspx?NewsName=jclq">竞彩篮球资讯</a></li>
		                <li class="icon_3"><a href="/ZX.aspx?NewsName=dlt">超级大乐透资讯</a></li>
		                <li class="icon_4"><a href="/ZX.aspx?NewsName=szpl">排列3/5资讯</a></li>
		                <li class="icon_5"><a href="/ZX.aspx?NewsName=qxc">七星彩资讯</a></li>
		                <li class="icon_6"><a href="/ZX.aspx?NewsName=22x5">22选5资讯</a></li>
		                <li class="icon_7"><a href="/ZX.aspx?NewsName=ctzc">足彩资讯</a></li>
		                <li class="icon_8"><a href="/ZX.aspx?NewsName=ogzx">欧冠资讯</a></li>
		                <li class="icon_9"><a href="/ZX.aspx?NewsName=yczx">英超资讯</a></li>
		                <li class="icon_10"><a href="/ZX.aspx?NewsName=xjzx">西甲资讯</a></li>
		                <li class="icon_11"><a href="/ZX.aspx?NewsName=yjsz">意甲资讯</a></li>
		                <li class="icon_12"><a href="/ZX.aspx?NewsName=djsz">德甲资讯</a></li>
	                </ul>
	                <div class="cl"></div>
                </div>
            </div>
            <div class="jc_period sub">
                <div class="r_box">
                    <div class="r_box_top">
                        <div class="r_title">
                            <h2>
                                <a title="最新中奖" href="javascript:void(0);">最新中奖</a></h2>
                        </div>
                    </div>
                    <div class="r_content">
                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="zj_table">
                            <thead>
                                <tr>
                                    <th class="text_l">
                                        用户名
                                    </th>
                                    <th>
                                        彩种
                                    </th>
                                    <th>
                                        倍数
                                    </th>
                                    <th class="text_r">
                                        奖金
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <%=WinUserList %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearb">
        </div>
    </div>
    <!--竞彩足球资讯部分结束-->
    </div>
    <asp:HiddenField ID="hi_PageIndex" runat="server" Visible="false" />
    <uc2:WebFoot ID="WebFoot1" runat="server" />
    </form>
</body>
</html>
