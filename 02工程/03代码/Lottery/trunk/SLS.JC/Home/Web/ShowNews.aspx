<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowNews.aspx.cs" Inherits="Home_Web_ShowNews" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=_Site.Name %>－买彩票就上<%=_Site.Name %></title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。" />
    <meta name="keywords" content="竞彩开奖，彩票走势图，超级大乐透，排列3/5" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <link href="Style/div.css" rel="stylesheet" type="text/css" />
    <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link href="/Style/zx.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
    <div id="box" style="width:1002px; padding-top:10px;">
        <div>
            <table width="1002" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <img src="images/ad_980_2.jpg" width="1002" height="70" style="vertical-align: top;
                            border: 0px;" />
                    </td>
                </tr>
                <tr>
                    <td height="10">
                    </td>
                </tr>
            </table>
            <div class="content">
                <div class="content1_l">
                    <!--左侧-->
                    <div class="sub_right">
            <!--新闻右边图片-->
                        <div class="news_right_ad">
                            <div class="news_Lnav" style="width:271px;">
                                <ul style="padding: 1px 0 0 0;">
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
                        <div class="cl"></div>
                        <div class="jc_period sub">
                            <div class="r_box" style="width:275px;">
                                <div class="r_box_top">
                                    <div class="r_title">
                                        <h2>
                                            <a href="javascript:void(0);" title="最新中奖">最新中奖</a></h2>
                                    </div>
                                </div>
                                <div class="r_content">
                                    <table border="0" cellpadding="0" cellspacing="0" class="zj_table" width="275px;">
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
                                            <%=WinUserList %>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="cl"></div>
                </div>
                <div class="content1_740">
                    <table width="710" border="0" cellpadding="0" cellspacing="1" bgcolor="#9BBFCA" style=" width:710px;">
                        <tr>
                            <td bgcolor="#FFFFFF">
                                <table width="708" border="0" cellpadding="0" cellspacing="0" background="images/bg_title_27.jpg">
                                    <tr>
                                        <td width="28" height="26" align="center" class="blue">
                                            <img src="images/icon_red_line.gif" width="3" height="12" />
                                        </td>
                                        <td width="708" class="hui" style="padding-top: 2px;">
                                            当前位置：<a href="../../Default.aspx">首页</a> &gt;
                                            <asp:HyperLink ID="hlNewsType" runat="server"></asp:HyperLink>
                                            &gt; 新闻详细
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#F6F9FE" style="padding: 15px 20px 15px 20px;table-layout:fixed;word-break;break-all;word-wrap:break-word;">
                                <!--startprint-->
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="table-layout:fixed;word-break;break-all;word-wrap:break-word;">
                                    <tr>
                                        <td height="36" align="center" class="red20">
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
                                            浏览次数：<asp:Label ID="lbCount" runat="server"></asp:Label>
                                            [<a onclick="window.close();" href="#">关闭本页</a>] [<a onclick="doPrint()" href="javascript:;">打印</a>]
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="1" bgcolor="#E1E1E1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="table-layout:fixed;word-break;break-all;word-wrap:break-word;" >
                                            <asp:Image ID="ImgUrl" runat="server"></asp:Image>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="black12">                                           
                                            <%--<div style="table-layout:fixed;word-break;break-all;word-wrap:break-word; width:665px;">--%>
                                            <label style="table-layout:fixed;word-break;break-all;word-wrap:break-word; width:665px;">
                                                <asp:Label ID="lbContent" Width="665px" runat="server" style="table-layout:fixed;word-break;break-all;word-wrap:break-word; font-size:12px;"></asp:Label>
                                            </label>
                                            <%--</div>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr id="ShowInfo" runat="server">
                                        <td>
                                            <table width="100%" align="center" border="0" cellpadding="0" cellspacing="1" style="background-color: #CDCDCD"
                                                id="NoNewsComments" runat="server" visible="false">
                                                <tr>
                                                    <td align="left" style="padding: 5px;" bgcolor="white" height="25px" class="black12">
                                                        暂无此文章的评论信息
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="CommentContents" runat="server">
                                        <td>
                                            <asp:Repeater ID="rptNewsComments" runat="server">
                                                <HeaderTemplate>
                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#CDCDCD">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
                                                                <tr>
                                                                    <td align="right" valign="middle" height="25" width="45%" style="padding-right: 10px;"
                                                                        class="black12">
                                                                        网友：<%# Eval("CommentserName")%>
                                                                    </td>
                                                                    <td align="left" valign="middle" width="55%" class="black12" height="25" class="black12">
                                                                        评论时间：
                                                                        <%# Eval("DateTime")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" valign="top" align="left" style="padding-left: 10px; padding-right: 10px;
                                                                        padding-top: 10px; padding-bottom: 10px;" class="black12">
                                                                        <%# Eval("Content")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr id="Comments" runat="server">
                                        <td>
                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#CDCDCD">
                                                <tr>
                                                    <td style="width: 100%;" valign="top" bgcolor="#FFFFFF">
                                                        <br />
                                                        <table style="width: 100%;" border="0" align="center" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td height="26" align="left" style="padding: 5px;" class="black12">
                                                                    称呼：<asp:TextBox ID="tbCommentserName" runat="server" MaxLength="40">
                                                                    </asp:TextBox>&nbsp;&nbsp;现有 <font color="#F8332D">
                                                                        <asp:Label ID="labNewsComments" runat="server" Text="0"></asp:Label>
                                                                    </font>人对本文发表评论
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="tbContent" TextMode="MultiLine" runat="server" Style="height: 80px;
                                                                        width: 98%;"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" valign="middle" height="30">
                                                                    <ShoveWebUI:ShoveConfirmButton ID="btnComments" runat="server" BackgroupImage="../../images/ShopSite/butt22.gif"
                                                                        Height="21px" OnClientClick="if(!isNull()) return false;" Width="61px" OnClick="btnComments_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <!--endprint-->
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    <asp:HiddenField ID="hID" runat="server" />
    </form>
    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>

<script type="text/javascript" language="javascript">
    function isNull()
    {
        var name= document.getElementById('tbCommentserName');
        var content = document.getElementById('tbContent');
        
        if(name.value=="")
        {
            alert("请输入您的姓名！");
            name.focus();
            
            return false;
        }
        
        if(content.value=="")
        {
            alert("请输入评价的内容！");
            content.focus();
            
            return false;
        }
        return confirm("您确信要发表评论吗？");
       
    }
    function doPrint()
    {
        bdhtml = window.document.body.innerHTML;
        sprnstr="<!--startprint-->";
        eprnstr="<!--endprint-->";
        prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr)+17);
        prnhtml= prnhtml.substring(0,prnhtml.indexOf(eprnstr));
        window.document.body.innerHTML = prnhtml;
        window.print();
        window.document.body.innerHTML = bdhtml;
        window.document.location.reload();
    }
</script>

