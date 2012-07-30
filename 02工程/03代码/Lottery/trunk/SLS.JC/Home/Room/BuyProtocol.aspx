<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuyProtocol.aspx.cs" Inherits="Home_Room_BuyProtocol" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=_Site.Name %>电话短信投注协议-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
     
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            min-width: 970x;
            font-family: "tahoma"; font-size:12px;
            text-align:center;
        }
    </style>
    <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body class="gybg">
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>


<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">


    <table cellpadding="0" cellspacing="0" style="width:950px; margin:10px auto 0px;">
        <tr>
            <td align="left">
                <div style="border: 1px solid #E3E3E4;padding:20px;">
                    <asp:Label ID="lbAgreement" runat="server">
                    </asp:Label>
                </div>
            </td>
        </tr>
    </table>


</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>



    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
        <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
