<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FocusEventAdd.aspx.cs" Inherits="Admin_FocusEventAdd" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <script src="../kindeditor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
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
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlMonth" runat="server">
                        <asp:ListItem Value="1">1 月</asp:ListItem>
                        <asp:ListItem Value="2">2 月</asp:ListItem>
                        <asp:ListItem Value="3">3 月</asp:ListItem>
                        <asp:ListItem Value="4">4 月</asp:ListItem>
                        <asp:ListItem Value="5">5 月</asp:ListItem>
                        <asp:ListItem Value="6">6 月</asp:ListItem>
                        <asp:ListItem Value="7">7 月</asp:ListItem>
                        <asp:ListItem Value="8">8 月</asp:ListItem>
                        <asp:ListItem Value="9">9 月</asp:ListItem>
                        <asp:ListItem Value="10">10月</asp:ListItem>
                        <asp:ListItem Value="11">11月</asp:ListItem>
                        <asp:ListItem Value="12">12月</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <font face="宋体">标题
                        <asp:TextBox ID="tbTitle" runat="server" Width="520px" MaxLength="100"></asp:TextBox></font>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbIsMaster" runat="server" Checked="true" />是否显示
                </td>
            </tr>
            <tr>
                <td>
                    内容
                </td>
            </tr>
            <tr>
                <td>
                    <textarea rows="1" cols="1" runat="server" id="tbContent" name="tbContent" style="width: 98%;
                        height: 300px;"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                    <font face="宋体">使用一个新的图片
                        <input id="tbImage" style="width: 600px; height: 21px" type="file" size="80" name="tbImage"
                            runat="server"><br />
                        <font color="#ff0000">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            (如果选择了新图片，下面“已存在的图片”选择将无效)</font></font>
                </td>
            </tr>
            <tr>
                <td>
                    <font face="宋体">使用已存在的图片
                        <asp:DropDownList ID="ddlImage" runat="server" Width="250px">
                        </asp:DropDownList>
                    </font>
                </td>
            </tr>
            <tr>
                <td style="height: 49px" align="center">
                    <ShoveWebUI:ShoveConfirmButton ID="btnAdd" runat="server" Width="60px" Height="20px"
                        BackgroupImage="../Images/Admin/buttbg.gif" Text="增加" AlertText="确信输入无误，并立即增加此新闻吗？"
                        OnClientClick="editor.sync();" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    <asp:HiddenField ID="hID" runat="server" />
    </form>
</body>
</html>
