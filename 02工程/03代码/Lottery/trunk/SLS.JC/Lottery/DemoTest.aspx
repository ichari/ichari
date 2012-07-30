<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DemoTest.aspx.cs" Inherits="Lottery_DemoTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script src="../JScript/Public.js" type="text/javascript"></script> 
    <script src="JScript/buy_SSQ.js" type="text/javascript"></script> 
    <link href="style/Buy.css" rel="stylesheet" type="text/css" />
    <link href="../Style/dahecp.css" rel="stylesheet" type="text/css" /> 
    <script src="../JScript/Marquee.js" type="text/javascript"></script>  
    <title></title>
</head>
<body >
    <form id="form1" runat="server">
        <input id="HidLotteryID" name="HidLotteryID" type="hidden" value="<%=LotteryID %>" />
    <div>
           <div id="lastIsuseInfo" style="height: 100%; overflow: hidden;">
                            <img src='../Home/Room/Images/londing.gif' style="position: relative; left: 25%;"
                                alt="" />
                        </div>
    </div>
    </form>
</body>
</html>
