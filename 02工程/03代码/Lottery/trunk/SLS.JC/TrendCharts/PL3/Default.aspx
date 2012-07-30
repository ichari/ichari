<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="TrendCharts_PL3_Default" %>


<%@ Register Src="../../Home/Room/UserControls/TrendChartHead.ascx" TagName="TrendChartHead"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>排列3走势图-彩票走势图表和数据分析－<%=_Site.Name %></title>
<meta name="keywords" content="排列3走势图" />
<meta name="description" content="排列3走势图、彩票走势图表和数据分析。" />

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
    </style>
    <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />
     <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

    <div style="padding-top:10px;"></div>
    <uc2:TrendChartHead ID="TrendChartHead1" runat="server" />
    <div id="box1">
   
         <iframe id="mainFrame" name="mainFrame" width="1002px" height="800px" frameborder="0" src="PL3_HMFB.aspx"
                style="overflow-x: auto; overflow-y: hidden; font-size:12px"></iframe>
    </div>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
    
<style type="text/css">
     
        #Charts3
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
        
        #Charts64
        {
        	display:none;           
        }
</style>
</body>
</html>
