<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiteImageManage.aspx.cs" Inherits="Admin_SiteImageManage"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        body
        {
        	font-family:微软雅黑;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table cellspacing="0" cellpadding="0" width="90%" align="center" border="0" style="line-height: 2;">
          
            <tr valign="middle">
                <td style="height:35px;">
                    <b>请选择要修改的资源文件</b>
                </td>
            </tr>
            <tr valign="middle">
                    <td>
                        <font face="微软雅黑">页面名称：</font>
                        <asp:DropDownList ID="ddlPageName" runat="server" Font-Names="微软雅黑"  Width="240px" AutoPostBack="True" 
                            onselectedindexchanged="ddlPageName_SelectedIndexChanged">                            
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <font face="微软雅黑">资源名称：</font>
                        <asp:DropDownList ID="ddlResourceName" runat="server" AutoPostBack="True" Width="240px" 
                            Font-Names="微软雅黑" onselectedindexchanged="ddlResourceName_SelectedIndexChanged"></asp:DropDownList>
                    </td>
            </tr>       
            <tr>            
                <td>
                   <asp:Label style="width:auto;" runat="server" ID="lblDescriptioin" Visible=false Font-Names="微软雅黑" ForeColor="Red"></asp:Label> &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <ShoveWebUI:ShoveFlashPlayer ID="sfp1" runat="server" Visible="False" />
                    <asp:Image ID="img1" runat="server" Visible="False" />
                    <br />
                    <br />
                </td>
            </tr>            
            <tr valign="middle">
                <td>
                    <font face="微软雅黑">使用一个新的图片
                            <input type="file" ID="uploadFile" runat="server" size="40" style="width: 600px; height: 21px"/>&nbsp;&nbsp; 
                </td>
            </tr>
            <tr>
                <td style="height: 50px" align="center">
                    <asp:Button ID="btnUpdate" runat="server" Text="上传更新文件" onclick="btnUpdate_Click" Enabled="false" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
