<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FilterShrink.aspx.cs" Inherits="JCZC_FilterShrink" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <input id="Number" name="Number" value="<%= Number %>" type="hidden" />
    <input id="hinUserID" name="Number" value="<%= ID %>" type="hidden" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div style="width:100%; height:580px;" >
     <asp:Silverlight ID="xaml1" runat="server" Source="~/Silverlight/SLS.SilverLight.FilterShrink.xap" Width="100%" Height="100%"/>
    </div>
    </form>
</body>
</html>
