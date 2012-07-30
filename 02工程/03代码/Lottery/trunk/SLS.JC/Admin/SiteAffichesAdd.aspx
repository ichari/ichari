<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiteAffichesAdd.aspx.cs"
    Inherits="Admin_SiteAffichesAdd" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <script src="../kindeditor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
            <tr>
                <td>
                    <font face="宋体">时间
                            <asp:TextBox ID="tbDateTime" runat="server" Width="200px"></asp:TextBox>&nbsp;
                            <asp:CheckBox ID="cbisShow" runat="server" Text="是否显示" Checked="True"></asp:CheckBox>&nbsp;<asp:CheckBox ID="cbisCommend" runat="server" Text="是否推荐" Checked="False"></asp:CheckBox>&nbsp;<asp:CheckBox ID="cbTitleRed" runat="server" Text="标题加红" Checked="False"></asp:CheckBox></font>
                </td>
            </tr>
            <tr>
                <td>
                    <font face="宋体">标题
                            <asp:TextBox ID="tbTitle" runat="server" Width="520px"></asp:TextBox></font>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        AutoPostBack="True" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                        <asp:ListItem Value="1" Selected="True">地址型</asp:ListItem>
                        <asp:ListItem Value="2">内容型</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trUrl" runat="server">
                <td>
                    <font face="宋体">地址
                            <asp:TextBox ID="tbUrl" runat="server" Width="520px" MaxLength="50"></asp:TextBox></font>
                </td>
            </tr>
            <tbody id="trContent" runat="server" visible="false">
                <tr>
                    <td>
                        内容
                    </td>
                </tr>
                <script type="text/javascript">
                    var editor;
                    KindEditor.ready(function (K) {
                        editor = K.create('#tbContent', {
                            cssPath: '../kindeditor/plugins/code/prettify.css',
                            uploadJson: '../kindeditor/asp.net/upload_json.ashx',
                            fileManagerJson: '../kindeditor/asp.net/file_manager_json.ashx',
                            resizeType: 0,
                            allowFileManager: true
                        });
                    });
                </script>
                <tr>
                    <td>
                        <textarea rows="1" cols="1" runat="server" id="tbContent" name="tbContent" style="width: 98%;
                            height: 300px;"></textarea>
                    </td>
                </tr>
            </tbody>
            <tr>
                <td style="height: 49px" align="center">
                    <ShoveWebUI:ShoveConfirmButton ID="btnAdd" runat="server" Width="60px" Height="20px"
                        BackgroupImage="../Images/Admin/buttbg.gif" Text="增加" AlertText="确信输入无误，并立即增加此公告吗？"
                        OnClientClick="sync_editor();" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
