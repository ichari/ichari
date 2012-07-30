<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LotteryInformation.aspx.cs"
    Inherits="Admin_LotteryInformation" ValidateRequest="false" %>

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
        <table id="Table1" cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
            <tr>
                <td style="height: 30px">
                    彩种
                    <asp:DropDownList ID="ddlLottery" runat="server" Width="145px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <ShoveWebUI:ShoveTabView ID="tv" runat="server" Height="540px" Width="800px" SupportDir="../ShoveWebUI_client"
                        SelectedTabCSSClass="" UnSelectedTabCSSClass="">
                        <ShoveWebUI:ShoveTabPage ID="p1" Text="玩法规则" ScrollBars="Auto" Height="100%" Width="800px"
                            runat="server">
                            <textarea rows="1" cols="1" runat="server" id="tbExplain" name="tbExplain" style="width: 98%;
                                height: 400px;"></textarea>
                            <script type="text/javascript">
                                var editor;
                                KindEditor.ready(function (K) {
                                    editor = K.create('#tbExplain', {
                                        cssPath: '../kindeditor/plugins/code/prettify.css',
                                        uploadJson: '../kindeditor/asp.net/upload_json.ashx',
                                        fileManagerJson: '../kindeditor/asp.net/file_manager_json.ashx',
                                        resizeType: 0,
                                        allowFileManager: true
                                    });
                                });
                            </script>
                        </ShoveWebUI:ShoveTabPage>
                        <ShoveWebUI:ShoveTabPage ID="p2" Text="方案书写规则" ScrollBars="Auto" Height="100%" Width="800px"
                            runat="server">
                            <textarea rows="1" cols="1" runat="server" id="tbLotteryExemple" name="tbLotteryExemple"
                                style="width: 98%; height: 400px;"></textarea>
                            <script type="text/javascript">
                                var editor;
                                KindEditor.ready(function (K) {
                                    editor = K.create('#tbLotteryExemple', {
                                        cssPath: '../kindeditor/plugins/code/prettify.css',
                                        uploadJson: '../kindeditor/asp.net/upload_json.ashx',
                                        fileManagerJson: '../kindeditor/asp.net/file_manager_json.ashx',
                                        resizeType: 0,
                                        allowFileManager: true
                                    });
                                });
                            </script>
                        </ShoveWebUI:ShoveTabPage>
                        <ShoveWebUI:ShoveTabPage ID="p3" Text="用户电话短信投注协议" ScrollBars="Auto" Height="100%"
                            Width="800px" runat="server">
                            <textarea rows="1" cols="1" runat="server" id="tbAgreement" name="tbAgreement" style="width: 98%;
                                height: 400px;"></textarea>
                            <script type="text/javascript">
                                var editor;
                                KindEditor.ready(function (K) {
                                    editor = K.create('#tbAgreement', {
                                        cssPath: '../kindeditor/plugins/code/prettify.css',
                                        uploadJson: '../kindeditor/asp.net/upload_json.ashx',
                                        fileManagerJson: '../kindeditor/asp.net/file_manager_json.ashx',
                                        resizeType: 0,
                                        allowFileManager: true
                                    });
                                });
                            </script>
                        </ShoveWebUI:ShoveTabPage>
                        <ShoveWebUI:ShoveTabPage ID="p4" Text="开奖公告模板" ScrollBars="Auto" Height="100%" Width="800px"
                            runat="server">
                            <textarea rows="1" cols="1" runat="server" id="tbOpenAfficheTemplate" name="tbOpenAfficheTemplate"
                                style="width: 98%; height: 400px;"></textarea>
                            <script type="text/javascript">
                                var editor;
                                KindEditor.ready(function (K) {
                                    editor = K.create('#tbOpenAfficheTemplate', {
                                        cssPath: '../kindeditor/plugins/code/prettify.css',
                                        uploadJson: '../kindeditor/asp.net/upload_json.ashx',
                                        fileManagerJson: '../kindeditor/asp.net/file_manager_json.ashx',
                                        resizeType: 0,
                                        allowFileManager: true
                                    });
                                });
                            </script>
                        </ShoveWebUI:ShoveTabPage>
                    </ShoveWebUI:ShoveTabView>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 60px">
                    <ShoveWebUI:ShoveConfirmButton ID="btnOK" runat="server" AlertText="您确信输入无误，并立即保存吗？"
                        Text="保存" BackgroupImage="../Images/Admin/buttbg.gif" Width="60px" Height="20px" 
                        OnClientClick="editor.sync();" OnClick="btnOK_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
