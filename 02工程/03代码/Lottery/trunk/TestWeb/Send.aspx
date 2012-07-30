<%@ Page Title="主页" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Send.aspx.cs" Inherits="TestWeb.Send" %>



<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="result">执行结果 : 
    <asp:Label runat="server" ID="lblTxt" Text="" CssClass="" ForeColor="Red"></asp:Label>
    <asp:TextBox runat="server" ID="tbResult" ForeColor="Red" TextMode="MultiLine" Height="200" Width="200"></asp:TextBox>
    </div>
    <div class="area">
        彩种ID：<asp:TextBox ID="tbQueryIssueLotTypeId" runat="server"  ></asp:TextBox>
            <br />
        期次号：<asp:TextBox ID="tbQueryIssueNo" runat="server" ></asp:TextBox>
        <asp:Button ID="btnSend" Text="发送奖期查询请求" runat="server" onclick="btnSend_Click" />
        
    </div>

    <div class="area">
        <asp:TextBox ID="tbMoniIssueNotice" runat="server" TextMode="MultiLine"></asp:TextBox>
        <asp:Button runat="server" ID="btnMoniIssueNotice" Text="模拟奖期通知" />
    </div>

    <div class="area">
        <asp:TextBox ID="tbRecieve" runat="server" TextMode="MultiLine" Height="125px" 
            Width="418px"></asp:TextBox>
            <asp:Button ID="btnParse" Text="Parse" runat="server" 
            onclick="btnParse_Click" />
    </div>

    <div class="area">
        彩种ID：<asp:TextBox ID="TextBox3" runat="server"  ></asp:TextBox>
            <br />
        期次号：<asp:TextBox ID="TextBox4" runat="server" ></asp:TextBox>
        <asp:TextBox ID="tbMoni" runat="server" TextMode="MultiLine" Height="125px" 
            Width="418px"></asp:TextBox>

            <asp:Button ID="btnMoni" Text="模拟开奖" runat="server" 
            onclick="btnMoni_Click" />
    </div>
    <div class="area">
        彩种ID：<asp:TextBox ID="TextBox1" runat="server"  ></asp:TextBox>
            <br />
        期次号：<asp:TextBox ID="TextBox2" runat="server" ></asp:TextBox>
        <asp:Button ID="btnSendOpen" Text="向出票商发送开奖请求" runat="server" 
            onclick="btnSendOpen_Click" />
    </div>
    <div class="area">
        彩种ID：<asp:TextBox ID="tbLotoTypeId" runat="server"  ></asp:TextBox>
            <br />
        期次号：<asp:TextBox ID="tbIssueNo" runat="server" ></asp:TextBox>
            <asp:Button ID="btnSniffer" Text="抓取第三方网站开奖" runat="server" 
            onclick="btnSniffer_Click" />
    </div>



    <ul>
        <li><asp:Label ID="lblLotoId" runat="server"></asp:Label></li>
        <li><asp:Label ID="lblIssue" runat="server"></asp:Label></li>
        <li><asp:Label ID="lblSt" runat="server"></asp:Label></li>
        <li><asp:Label ID="lblEt" runat="server"></asp:Label></li>
        <li><asp:Label ID="lblBonusCode" runat="server"></asp:Label></li>
        <li><asp:Label ID="lblStatus" runat="server"></asp:Label></li>
    </ul>
</asp:Content>
