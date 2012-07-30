<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Options.aspx.cs" Inherits="Admin_Options" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" Style="z-index: 104; left: 32px; position: absolute; top: 30px"
            runat="server" ForeColor="#C00000" Font-Bold="True">系统参数设置</asp:Label>
        <asp:TextBox ID="tbInitiateSchemeBonusScale" runat="server" Style="z-index: 134;
            left: 215px; position: absolute; top: 154px" Width="74px" MaxLength="10"></asp:TextBox>
        <asp:Label ID="Label17" runat="server" Style="z-index: 129; left: 71px; position: absolute;
            top: 154px">方案制作佣金(盈利时)</asp:Label>
        <asp:Label ID="Label46" runat="server" Style="z-index: 129; left: 308px; position: absolute;
            top: 178px">第二种方式：</asp:Label>
        <asp:Label ID="Label47" runat="server" Style="z-index: 129; left: 390px; position: absolute;
            top: 178px">底值金额：</asp:Label>
        <asp:Label ID="Label48" runat="server" Style="z-index: 129; left: 538px; position: absolute;
            top: 178px">底值比例：</asp:Label>
        <asp:Label ID="Label49" runat="server" Style="z-index: 129; left: 697px; position: absolute;
            top: 178px; right: 450px;">顶值金额：</asp:Label>
        <asp:TextBox ID="tbInitiateSchemeLimitLowerScaleMoney" runat="server" Style="z-index: 134;
            left: 447px; position: absolute; top: 178px; right: 685px; width: 75px;" MaxLength="10"></asp:TextBox>
        <asp:TextBox ID="tbInitiateSchemeLimitLowerScale" runat="server" Style="z-index: 134;
            left: 603px; position: absolute; top: 178px" Width="74px" MaxLength="10"></asp:TextBox>
        <asp:TextBox ID="tbInitiateSchemeLimitUpperScaleMoney" runat="server" Style="z-index: 134;
            left: 762px; position: absolute; top: 178px; right: 371px;" Width="74px" MaxLength="10"></asp:TextBox>
        <asp:TextBox ID="tbInitiateSchemeLimitUpperScale" runat="server" Style="z-index: 134;
            left: 920px; position: absolute; top: 178px" Width="74px" MaxLength="10"></asp:TextBox>
        <asp:TextBox ID="tbInitiateSchemeMinBuyAndAssureScale" runat="server" Style="z-index: 134;
            left: 215px; position: absolute; top: 203px" Width="74px" MaxLength="10"></asp:TextBox>
        <asp:Label ID="Label19" runat="server" Style="z-index: 129; left: 71px; position: absolute;
            top: 203px">发起人最少认购(含保底)</asp:Label>
        <asp:TextBox ID="tbInitiateSchemeMaxNum" runat="server" Style="z-index: 134; left: 215px;
            position: absolute; top: 227px" Width="74px" MaxLength="5"></asp:TextBox>
        <asp:Label ID="Label32" runat="server" Style="z-index: 129; left: 71px; position: absolute;
            top: 227px">每人每期最多发起方案</asp:Label>
        <asp:Label ID="Label18" runat="server" Style="z-index: 129; left: 71px; position: absolute;
            top: 178px">发起人最少认购</asp:Label>
        <asp:Label ID="Label13" runat="server" ForeColor="Red" Style="z-index: 129; left: 442px;
            position: absolute; top: 17px">(系统重要参数，请谨慎设置)</asp:Label>
        <asp:TextBox ID="tbScoringOfSelfBuy" runat="server" Style="z-index: 134; left: 215px;
            position: absolute; top: 330px" Width="74px" MaxLength="10"></asp:TextBox>
        <asp:Label ID="Label35" runat="server" Style="z-index: 129; left: 71px; position: absolute;
            top: 330px">会员购彩积分默认值</asp:Label>
        <asp:Label ID="Label37" runat="server" Style="z-index: 129; left: 303px; position: absolute;
            top: 331px">分/元</asp:Label>
        <asp:Label ID="Label38" runat="server" Style="z-index: 129; left: 303px; position: absolute;
            top: 356px">分/元</asp:Label>
        <asp:Label ID="Label41" runat="server" Style="z-index: 129; left: 303px; position: absolute;
            top: 381px">元/分</asp:Label>
        <asp:TextBox ID="tbScoringExchangeRate" runat="server" Style="z-index: 118; left: 215px;
            position: absolute; top: 380px" Width="74px" MaxLength="10"></asp:TextBox>
        <asp:Label ID="Label42" runat="server" Style="z-index: 117; left: 70px; position: absolute;
            top: 380px">积分兑换比率</asp:Label>
        <asp:TextBox ID="tbScoringOfCommendBuy" runat="server" Style="z-index: 118; left: 215px;
            position: absolute; top: 355px" Width="74px" MaxLength="10"></asp:TextBox>
        <asp:Label ID="Label36" runat="server" Style="z-index: 117; left: 70px; position: absolute;
            top: 355px">下级购彩积分默认值</asp:Label>
        <asp:CheckBox ID="cbScoring_Status_ON" runat="server" Style="z-index: 115; left: 212px;
            position: absolute; top: 403px" Text="是否启用积分功能？" />
        <asp:Label ID="Label16" Style="z-index: 125; left: 71px; position: absolute; top: 105px"
            runat="server">登录日志</asp:Label>
        <asp:Label ID="Label11" runat="server" Style="z-index: 119; left: 304px; position: absolute;
            top: 299px">行，将不显示彩票号码，而改用“下载方案”</asp:Label>
        <asp:TextBox ID="tbMaxShowLotteryNumberRows" Style="z-index: 118; left: 215px; position: absolute;
            top: 299px" runat="server" Width="74px" MaxLength="5"></asp:TextBox>
        <asp:Label ID="Label10" Style="z-index: 117; left: 70px; position: absolute; top: 299px"
            runat="server">方案表格中彩票号超过</asp:Label>
        <asp:CheckBox ID="cbisWriteLog" Style="z-index: 103; left: 213px; position: absolute;
            top: 104px" runat="server" Text="是否记录用户登录日志？"></asp:CheckBox>
        <asp:CheckBox ID="cbFullSchemeCanQuash" Style="z-index: 100; left: 306px; position: absolute;
            top: 275px" runat="server" Text="满员方案是否允许用户撤单？" Enabled="False"></asp:CheckBox>
        <ShoveWebUI:ShoveConfirmButton ID="btnOK" Style="z-index: 101; left: 374px; position: absolute;
            top: 455px" BackgroupImage="../Images/Admin/buttbg.gif" runat="server" Width="60px"
            Height="20px" Text="保存" AlertText="确认书写无误吗？" OnClick="btnOK_Click" />
        <asp:Label ID="Label9" Style="z-index: 114; left: 373px; position: absolute; top: 105px"
            runat="server" ForeColor="Red">提示：不记录日志，将不能分析与监控用户登录情况</asp:Label>
        <asp:TextBox ID="tbInitiateFollowSchemeMaxNum" runat="server" Style="z-index: 134;
            left: 215px; position: absolute; top: 252px" Width="74px" MaxLength="5"></asp:TextBox>
        <asp:Label ID="Label39" runat="server" Style="z-index: 129; left: 71px; position: absolute;
            top: 252px">超级发起人最多发起方案</asp:Label>
        <asp:TextBox ID="tbQuashSchemeMaxNum" runat="server" Style="z-index: 134; left: 215px;
            position: absolute; top: 276px" Width="74px" MaxLength="5"></asp:TextBox>
        <asp:Label ID="Label40" runat="server" Style="z-index: 129; left: 71px; position: absolute;
            top: 276px">每期最大撤单次数</asp:Label>
        <asp:Label ID="Label34" runat="server" ForeColor="Red" Style="z-index: 129; left: 308px;
            position: absolute; top: 153px">( 0-1 ) 例如0.03</asp:Label>
        <asp:Label ID="Label50" runat="server" Style="z-index: 129; left: 856px; position: absolute;
            top: 178px">顶值比例：</asp:Label>
    </div>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    </form>
    </body>
</html>
