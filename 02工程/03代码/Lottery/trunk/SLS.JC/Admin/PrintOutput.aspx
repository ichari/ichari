<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintOutput.aspx.cs" Inherits="Admin_PrintOutput" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../JScript/Public.js"></script>
    <script type="text/javascript">
		<!--
		function onUploadClick()
		{
			var o_fileUp = window.document.all["fileUp"];
			if (o_fileUp.value.trim() == "")
			{
				alert("请选择修改好了的彩票标识号 Execl 文件。");
				o_fileUp.focus();
				return false;
			}

			if (!confirm("确信上传的文件格式正确吗？如果选择错误的文件或错误的彩票标识号，将造成不可挽回的后果，请谨慎确认！！"))
				return false;

			return true;
		}
		-->
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table id="Table1" cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
            <tr>
                <td>
                    <font face="宋体">选择彩票类型
                        <asp:DropDownList ID="ddlLottery" runat="server" Width="140px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:DropDownList ID="ddlIsuse" runat="server" AutoPostBack="True" Width="120px"
                            OnSelectedIndexChanged="ddlIsuse_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;
                         <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" Width="120px"
                            OnSelectedIndexChanged="ddlType_SelectedIndexChanged" Visible="false">
                        </asp:DropDownList>
                        &nbsp;
                        <ShoveWebUI:ShoveConfirmButton ID="btnRefresh" BackgroupImage="../Images/Admin/buttbg.gif"
                            runat="server" Width="60px" Height="20px" Text="刷新" OnClick="btnRefresh_Click" /></font>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:DataList ID="g" runat="server" Width="100%" OnItemCommand="g_ItemCommand" >
                        <HeaderTemplate>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="td4" width="10%">
                                        <strong>方案编号</strong>
                                    </td>
                                    <td class="td4" width="15%">
                                        <strong>类别</strong>
                                    </td>
                                    <td class="td4" width="200px">
                                        <strong>购买内容</strong>
                                    </td>
                                    <td class="td4" width="130px">
                                        <strong>截止时间</strong>
                                    </td>
                                    <td class="td4" width="60px">
                                        <strong>倍数</strong>
                                    </td>
                                    <td class="td4" width="80px">
                                        <strong>金额</strong>
                                    </td>
                                    <td class="td4" width="120px;">
                                        <strong>出票</strong>
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td width="10%">
                                        <a href='../Home/Room/Scheme.aspx?id=<%# DataBinder.Eval(Container.DataItem,"id")%>'
                                            target="_blank">
                                            <%# DataBinder.Eval(Container.DataItem,"SchemeNumber")%>
                                        </a>
                                    </td>
                                    <td width="15%">
                                        <%# DataBinder.Eval(Container.DataItem,"PlayTypeName")%>
                                    </td>
                                    <td width="280px" align="center">
                                        <a class="li3" href='../Home/Web/DownloadSchemeFile.aspx?id=<%# DataBinder.Eval(Container.DataItem,"id")%>' target="_blank"><%# Shove._String.Cut(DataBinder.Eval(Container.DataItem,"LotteryNumber").ToString().Trim(), 20)%></a>
                                    </td>
                                    <td width="130px" align="left">
                                        <%# Shove._String.Cut(DataBinder.Eval(Container.DataItem, "SystemEndTime").ToString().Trim(), 10)%>
                                    </td>
                                    <td width="70px">
                                        <%# int.Parse(Eval("Multiple").ToString()) == 0 ? (Eval("LocateWaysAndMultiples")) : (Eval("Multiple"))%>
                                    </td>
                                    <td width="90px">
                                        <font color="#ff0000">
                                            <%# DataBinder.Eval(Container.DataItem,"Money")%>
                                        </font>
                                    </td>
                                    <td>
                                        <ShoveWebUI:ShoveConfirmButton ID="btnBuy" BackgroupImage="../Images/Admin/buttbg.gif"
                                            runat="server" Width="60px" Text="出票" Height="20px" CommandName="btnBuy" AlertText="确认立即出票吗？"/>
                                    </td>
                                    <td width="0%" style="display:none">
                                        <input id="tbSiteID" style="width: 61px; height: 21px" type="hidden" size="4" value='<%# DataBinder.Eval(Container.DataItem,"SiteID")%>'
                                            runat="server"/>
                                        <input id="tbID" style="width: 61px; height: 21px" type="hidden" size="4" value='<%# DataBinder.Eval(Container.DataItem,"ID")%>'
                                            runat="server"/>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList><font face="宋体" color="#ff0000">(注：多张彩票的标识用“,”隔开，如：20060HS01,20060HS01)
                   <asp:Panel ID="panel1" runat="server">
                        <ShoveWebUI:ShoveConfirmButton ID="btnBuyAll" runat="server" BackgroupImage="../Images/Admin/buttbg.gif"
                            AlertText="确认立即全部出票吗？" Height="20px" Text="全部出票" Width="60px"
                            OnClick="btnBuyAll_Click" />
                        (全部出票将不输入彩票标识)</asp:Panel></font>
                </td>
            </tr>
            <tr>
                <td style="height: 30px">
                </td>
            </tr>
            <tr>
                <td class="td4">
                    <font face="宋体"><strong>满员方案批量下载为 Execl 文件，一次性修改“彩票标识”，然后重新上传该 Execl 文件，可以批量出票：
                        <asp:LinkButton ID="btnDownload" runat="server" CssClass="li3" Enabled="False" OnClick="btnDownload_Click">下载 Execl 文件</asp:LinkButton></strong></font>
                </td>
            </tr>
            <tr>
                <td style="height: 71px" align="center">
                    <font face="宋体">上传批量出票 Execl 文件：</font><input id="fileUp" style="width: 448px; height: 21px"
                        type="file" size="55" name="fileUp" runat="server" disabled /><font face="宋体">&nbsp;</font>
                    <ShoveWebUI:ShoveConfirmButton ID="btnUpload" runat="server" Width="104px" Height="21px"
                        Text="上传 Execl 文件" OnClick="btnUpload_Click" OnClientClick="if (!onUploadClick()) return false; this.disabled=true;" /><font
                            face="宋体"><br />
                            <font color="#ff0000">(请遵照下载的 Execl 格式，填写彩票标识号，不要修改 Execl 文件格式。并确信输入无误后再上传！)</font></font>
                </td>
            </tr>
            <tr>
                <td style="height: 30px">
                </td>
            </tr>
            <tr>
                <td class="td4">
                    <font face="宋体"><strong>满员方案批量下载为 txt 文件，供第三方软件分析、打印使用：
                        <asp:LinkButton ID="btnDownload_txt" runat="server" CssClass="li3" Enabled="False"
                            OnClick="btnDownload_txt_Click">下载 txt 文件</asp:LinkButton><asp:LinkButton ID="btnOkoooDownload_txt" runat="server" CssClass="li3"
                            OnClick="btnOkoooDownload_txt_Click">下载 澳客格式 txt 文件</asp:LinkButton></strong></font>
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
