<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowAffiches.aspx.cs" Inherits="Home_Web_ShowAffiches" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=_Site.Name %>-站点公告-竞彩足球，竞彩足球，足彩，数字彩-买彩票就上<%=_Site.Name %></title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。" />
    <meta name="keywords" content="竞彩开奖，彩票走势图，超级大乐透，排列3/5" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <link href="Style/div.css" rel="stylesheet" type="text/css" />
    <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />    
    <link href="/Style/zx.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../../favicon.ico"/>
</head>
<body class="gybg">
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

    <div class="w980 png_bg2">
    <!-- 内容开始 -->
    <div class="w970">


    <div id="box">
        
        <div>
            <table width="100%" style="margin:10px;" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <img src="images/ad_980_2.jpg" />
                    </td>
                </tr>
            </table>
            <div>
                <div style="width:200px; float:left; margin-left:10px;">
                    <!--左侧-->
            <!--新闻右边图片-->
 
                            <div>
                                <div class="afff fs14 pl10 title bg1x bgp1 lh32" style="border:1px solid #E3E3E4;">最新中奖</div>
                                <table  style="border:1px solid #E3E3E4;"cellpadding="0" cellspacing="0" class="zj_table" width="100%">
                                        <thead>
                                            <tr>
                                                <th style="text-align:center; padding-left:5px;">
                                                    用户名 
                                                </th>
                                                <th style="text-align:center;">
                                                    彩种 
                                                </th>
                                                <th style="text-align:center;">
                                                    倍数 
                                                </th>
                                                <th style="text-align:center;">
                                                    奖金 
                                                </th>
                                            </tr>
                                        </thead>
                                            <%=WinUserList %>
                                    </table>
                        </div>
                    </div>
                <div style="width:740px; float:left; margin-left:10px;">
                    <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E3E3E4">
                        <tr>
                            <td bgcolor="#FFFFFF">
                                <table width="100%" class="a000 pl10 title bg1x bgp5 lh32" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="25" height="32" align="center" class="blue">
                                            <img src="images/icon_red_line.gif" width="3" height="12" />
                                        </td>
                                        <td style="padding-top: 2px;">
                                            当前位置：<a href="../../Default.aspx">首页</a> &gt; 站点公告 &gt; 正文
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#F6F9FE" style="padding: 15px 20px 15px 20px;">
                                <!--startprint-->
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="36" align="center" class="red20" style="table-layout:fixed;word-break;break-all;word-wrap:break-word;>
                                            <asp:Label ID="lbTitle" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="1" bgcolor="#E1E1E1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="30" align="right" class="hui" style="padding-right: 20px;">
                                            日期：<asp:Label ID="lbDateTime" runat="server"></asp:Label>
                                            [<a onclick="window.close();" href="#">关闭本页</a>] [<a onclick="doPrint()" href="javascript:;">打印</a>]
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="1" bgcolor="#E1E1E1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="black12">
                                            <asp:Label ID="lbContent" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
      
    </div>


    </div>
    <!-- 内容结束 -->
    </div>
    <div class=" w980 png_bg1"></div>


    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>

<script type="text/javascript" language="javascript">
    function doPrint() {
        bdhtml = window.document.body.innerHTML;
        sprnstr = "<!--startprint-->";
        eprnstr = "<!--endprint-->";
        prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17);
        prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
        window.document.body.innerHTML = prnhtml;
        window.print();
        window.document.body.innerHTML = bdhtml;
        window.document.location.reload();
    }

</script>

