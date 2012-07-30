<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchemeExemple.aspx.cs" Inherits="Home_Room_SchemeExemple" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>方案示例-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。"/>
    <meta name="keywords" content="方案示例" />
     
    <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />
<link rel="shortcut icon" href="../../favicon.ico" /></head>
<body class="gybg">
    <form id="form1" runat="server">
     <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

    <div class="w980 png_bg2">
    <!-- 内容开始 -->
    <div class="w970">

    <div align="center" style="width: 950px; margin:10px auto 0px;">
        <div align="left">
            <asp:Label ID="labContent" runat="server"></asp:Label>
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
