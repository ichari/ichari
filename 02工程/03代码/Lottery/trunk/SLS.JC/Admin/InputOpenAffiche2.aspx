<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InputOpenAffiche2.aspx.cs"
    Inherits="Admin_InputOpenAffiche2" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <script src="../kindeditor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script type="text/javascript">
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('#tbOpenAffiche', {
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
        <table id="Table1" cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
            <tr>
                <td style="height: 21px">
                    请选择
                    <asp:DropDownList ID="ddlLottery" runat="server" AutoPostBack="True" Width="140px"
                        OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:DropDownList ID="ddlIsuse" runat="server" AutoPostBack="True" Width="120px"
                        OnSelectedIndexChanged="ddlIsuse_SelectedIndexChanged">
                    </asp:DropDownList>
                    <span style="margin-left: 30px;"><a href="InputOpenAffiche.aspx">录入已开启彩种开奖公告</a></span>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <textarea rows="1" cols="1" runat="server" id="tbOpenAffiche" name="tbOpenAffiche" style="width: 98%;
                        height: 300px;"></textarea>
                </td>
            </tr>
            <tr>
                <td style="height: 21px">
                </td>
            </tr>
            <tr>
                <td style="height: 21px">
                    上传开奖视频文件
                    <input id="fileVideo" runat="server" name="fileVideo" style="width: 500px; height: 21px"
                        type="file" />
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 50px">
                    <ShoveWebUI:ShoveConfirmButton ID="btnOK" runat="server" BackgroupImage="../Images/Admin/buttbg.gif"
                        Width="60px" Height="20px" Text="保存" AlertText="确信输入无误，并立即保存吗？" OnClientClick="editor.sync();" OnClick="btnOK_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
