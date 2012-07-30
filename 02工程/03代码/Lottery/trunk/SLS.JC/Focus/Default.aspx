<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Focus_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=_Site.Name %>-焦点赛事</title>
    <link href="../style/dahecp.css" rel="stylesheet" type="text/css" />
    <link href="style/main.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
    <!--图集开始-->
    <div class="FootballBox">
        <div style="background:url(Images/jiaodianzhanbg2.jpg) no-repeat;width:1002px;height:29px;padding-top:7px;">
            <center>
            <div style="width:902px; float:left; line-height:2; text-align:center;">
                <%=innerHtml%>
            </div>
            <div style="width:100px; float:right;">
                <asp:DropDownList ID="ddlYear" runat="server" 
                    onselectedindexchanged="ddlYear_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            </center>
        </div>
        <div>
            <img src="Images/titlebg.png" /></div>
        <div class="bg2">
            <asp:Label id = "lbContent" runat="server"></asp:Label>
        </div>
    </div>
    <!--图集结束-->
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
