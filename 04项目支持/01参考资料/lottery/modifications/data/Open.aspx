<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Open.aspx.cs" Inherits="Admin_Open"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../JScript/Public.js"></script>
    <script src="../kindeditor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script type="text/javascript">

        function CalcMoneyNoWithTax(sender) {
            var WinMoney = StrToFloat(sender.value);

            var tbMoneyNoWithTax = document.getElementById(sender.id.replace("tbMoney", "tbMoneyNoWithTax"));

            if (!tbMoneyNoWithTax) {
                return;
            }

            if (WinMoney < 10000) {
                tbMoneyNoWithTax.value = WinMoney;
                
                return;
            }

            tbMoneyNoWithTax.value = Round(WinMoney * 0.8, 2);
        }
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table id="Table1" cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
            <tr>
                <td style="height: 30px">
                    <font face="宋体">&nbsp; 请选择
                        <asp:DropDownList ID="ddlLottery" runat="server" Width="140px" AutoPostBack="True" OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:DropDownList ID="ddlIsuse" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddlIsuse_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
            </tr>
            <tr>
                <td style="height: 30px">
                    <table id="WinNumberOther" runat="server" cellspacing="0" cellpadding="0" width="100%"
                        align="center" border="0">
                        <tr>
                            <td style="height: 36px" colspan="2">
                                &nbsp;
                                <asp:Label ID="Label1" runat="server">开奖号码</asp:Label>
                                <asp:TextBox ID="tbWinNumber" runat="server" Width="216px"></asp:TextBox>
                                <asp:Label ID="labTip" runat="server" ForeColor="Red">格式：31032200111220</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%">
                                <font color="#ff0000">【注】</font>三步开奖方式操作说明：<br />
                                1、 第一位管理员录入中奖号码和奖级金额，点击第一步，成功后会提示“请下一位管理员继续开奖”。
                                <br />
                                2、 第二位管理员登陆后，继续录入中奖号码和奖级金额，点击第一步，若录入的数据与第一位管理员完全一致，后会提示“请继续开奖”，依次激活开奖操作第二步、第三步。
                                <br />
                                3、当期的投注方案数量较多时，会提示多次执行第三步，直到提示开奖成功，即完成开奖的全部操作。 </font>
                            </td>
                            <td align="center">
                                请输入各奖级的中奖金额<br />
                                <asp:GridView ID="g" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                                    GridLines="None" OnRowDataBound="g_RowDataBound" Width="500px" BorderStyle="Solid"
                                    BorderWidth="1px" DataKeyNames="DefaultMoney,DefaultMoneyNoWithTax">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="奖级" />
                                        <asp:TemplateField HeaderText="奖金">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbMoney" runat="server" onblur="CalcMoneyNoWithTax(this);" Width="80"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="税后奖金">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbMoneyNoWithTax" runat="server" Width="80"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 39px" align="center">
                    <ShoveWebUI:ShoveConfirmButton ID="btnGO_Step1" runat="server" BackgroupImage="../Images/Admin/buttbg.gif"
                        Width="60px" Height="20px" Text="开奖1" OnClientClick="editor.sync();" OnClick="btnGO_Step1_Click" />
                    &nbsp;
                    <ShoveWebUI:ShoveConfirmButton ID="btnGO_Step2" Enabled="false" runat="server" BackgroupImage="../Images/Admin/buttbg.gif"
                        Width="60px" Height="20px" Text="开奖2" OnClientClick="editor.sync();" OnClick="btnGO_Step2_Click" />
                    &nbsp;
                    <ShoveWebUI:ShoveConfirmButton ID="btnGO_Step3" Enabled="false" runat="server" BackgroupImage="../Images/Admin/buttbg.gif"
                        Width="60px" Height="20px" Text="开奖3" OnClientClick="editor.sync();" OnClick="btnGO_Step3_Click" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    请填写此期的开奖公告<br />
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
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tr>
                            <td align="left">
                                <textarea rows="1" cols="1" runat="server" id="tbOpenAffiche" name="tbOpenAffiche"
                                    style="width: 98%; height: 300px;"></textarea>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <input id="h_SchemeID" type="hidden" runat="server" />
        <br />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
