<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Cps_Administrator_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>超级管理后台</title>
     <link href="../../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../../Style/main.css" type="text/css" rel="stylesheet" />
</head>
    <frameset id="VFrameset" border="1" framespacing="0" rows="80,*" frameborder="NO" cols="*">
        <FRAME name="topFrame" src="DefaultFrameTop.aspx" scrolling="no">
        <FRAMESET id="ClientFrameset" border="2" frameSpacing="0" rows="*" frameBorder="NO" cols="210,*">
            <FRAME id="leftFrame" name="leftFrame" src="DefaultFrameLeft.aspx" noResize="yes" scrolling="no">
            <FRAME id="mainFrame" style="padding-top:15px" name="mainFrame" src="<%=SubPage %>">
        </FRAMESET>

    </frameset>
</html>
