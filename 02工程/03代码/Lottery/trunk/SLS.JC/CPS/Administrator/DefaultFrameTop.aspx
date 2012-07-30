<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultFrameTop.aspx.cs" Inherits="Cps_Administrator_DefaultFrameTop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    <!--
    body{
	font-size: 12px;
	color: #103875;
    }
    a{
	    font-size: 12px;
	    color: #103875;
    }
    a:link {
	    text-decoration: none;
	    color: #103875;
    }
    a:visited {
	    text-decoration: none;
	    color: #103875;
    }
    a:hover {
	    text-decoration: none;
	    color: #FF6600;
    }
    a:active {
	    text-decoration: none;
	    color: #103875;
    }
    -->
    </style>
</head>
<body style="margin: 0px">
    <form id="form1" runat="server">
    <div>
        <table style=" width:100%; height: 115px;" cellpadding="0" cellspacing="0">
            <tr>
                <td width="484" height="111">
                       <a href="../../Default.aspx" target="_blank"> <img src="<%= ResolveUrl("~/CPS/Images/index-1_01.gif") %>" width="484" height="111" border="0"/></a>
                    </td>
                    
                   <td  style="padding-top:10px; position:absolute; left:500px" valign="top">
                        <a  href="../Default.aspx" target="_parent">【进入CPS首页】</a>
                        <a  href="../../Home/Room/Default.aspx" target="_blank">【进入网站首页】</a> 
                        <asp:LinkButton ID="linkLogout" runat="server" onclick="linkLogout_Click">【安全退出】</asp:LinkButton>
                        <br/><br/><br/>
                        &nbsp;&nbsp;
                        欢迎 <asp:Label ID="labUserName" runat="server"></asp:Label>&nbsp;登录CPS超级管理后台！
                   </td>
         
            </tr>
        </table>
    </div>
    </form>

</body>
</html>
