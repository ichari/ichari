<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LastestDrawing.ascx.cs" Inherits="UserControls_LastestDrawing" %>

<div class="fl">
    <span class="a"><asp:HyperLink ID="hlLotto" runat="server"><asp:Label ID="lbLottoName" runat="server" /></asp:HyperLink> <asp:Label ID="lbDrawNumber" runat="server" /> 期</span>
    <br />
    <asp:PlaceHolder ID="phWinNumRed" runat="server" /><asp:PlaceHolder ID="phWinNumBlue" runat="server" />
</div>