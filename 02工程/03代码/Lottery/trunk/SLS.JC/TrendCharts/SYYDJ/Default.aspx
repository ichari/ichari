<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="TrendCharts_SYYDJ_Default" %>

<%@ Register Src="../../Home/Room/UserControls/TrendChartHead.ascx" TagName="TrendChartHead"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>十一运夺金走势图-彩票走势图表和数据分析－<%=_Site.Name %></title>
<meta name="keywords" content="十一运夺金走势图" />
<meta name="description" content="十一运夺金走势图、彩票走势图表和数据分析。" />
<style type="text/css">
        .td
        {
            color: #cc0000;
            font-size: 16px;
        }
        td
        {
            font-size: 12px;
            font-family: tahoma;
            text-align: center;
        }
        body
        {
            margin: 0px;
            margin-left: 10px;
            margin-right: 10px;
        }
        form
        {
            display: inline;
        }
        #box1
        {
            overflow: hidden;
            width: 1002px;
            margin-right: auto;
            margin-left: auto;
            padding: 0px;
        }
        
        /*footer*/
        .footer{ width:1002px; height:auto; margin:0 auto; clear:both; text-align:center; padding:10px 0 20px 0; font-size:12px;}
        .bot_nav { width:auto; padding:5px 0 5px 0; clear:both; color:#999999; border:1px solid #c0cfde; background:#f8f8f8;}
        .bot_nav a{ color:#5b7e9b;}
        .bot_nav a:hover{ color:#ff6600;}

        .bot_copyright{ height: auto; padding:10px 0 0 0; line-height:20px; color:#999999;}
        .bot_linklogo_mrt{ height:auto; clear:both; padding:10px 0 0 0;}
        .bot_linklogo_mrt ul{ width:400px; margin:0 auto;}
        .bot_linklogo_mrt ul li { float:left; height:48px; text-indent:-2000px;}
        .bot_linklogo_mrt ul li a{ width:108px; height:48px; display:block; background:url(/images/bot_linklogo.gif) no-repeat;}
        .bot_linklogo_mrt ul li.b_link01{ width:108px; height:48px; overflow:hidden;}
        .bot_linklogo_mrt ul li.b_link01 a{ width:108px; height:48px; display:block; background-position:0px top;}
        .bot_linklogo_mrt ul li.b_link02{ width:102px; height:48px; overflow:hidden; margin:0 0 0 20px;}
        .bot_linklogo_mrt ul li.b_link02 a{ width:102px; height:48px; display:block; background-position:-136px top;}
        .bot_linklogo_mrt ul li.b_link03{ width:54px; height:48px; overflow:hidden; margin:0 0 0 20px;}
        .bot_linklogo_mrt ul li.b_link03 a{ width:54px; height:48px; display:block; background-position:-270px top;}
        .bot_linklogo_mrt ul li.b_link04{ width:46px; height:48px; overflow:hidden; margin:0 0 0 20px;}
        .bot_linklogo_mrt ul li.b_link04 a{ width:46px; height:48px; display:block; background-position:-345px top;}

    </style>
     <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body>

    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

    <div style="padding-top:10px;"></div>
    <uc2:TrendChartHead ID="TrendChartHead1" runat="server" />
    <div id="box1">
        <a href="<%= _Site.Url %>" target="_blank" style="text-decoration: none; font-size: 14px;
                        padding-left: 10px;"><%=_Site.Name %>首页</a> <a href="<%= ResolveUrl("~/Lottery/Buy_SYYDJ.aspx") %>"
                            target="_blank" style="padding-left: 10px; text-decoration: none; color: Red;">十一运夺金投注/合买</a>
                    <a href="<%= ResolveUrl("~/WinQuery/62-0-0.aspx") %>" target="_blank" style="padding-left: 10px;
                        text-decoration: none; color: Red;">十一运夺金中奖查询</a>
         <iframe id="mainFrame" name="mainFrame" width="1002px" height="800px" frameborder="0" src="SYDJ_HZFB.aspx"
                style="overflow-x: auto; overflow-y: hidden;"></iframe>
    </div>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>    
    </form>

<style type="text/css">

        #Charts3
        {
        	display:none;           
        }
        #Charts5
        {
        	display:none;           
        }
        #Charts6
        {
        	display:none;           
        }
        #Charts61
        {
        	display:none;           
        }
        #Charts63
        {
        	display:none;           
        }
        #Charts64
        {
        	display:none;           
        }
        #Charts9
        {
        	display:none;           
        }
        #Charts39
        {
        	display:none;           
        }
        #Charts29
        {
        	display:none;           
        }
</style>
</body>
</html>
