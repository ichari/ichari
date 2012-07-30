<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendTicket.aspx.cs" Inherits="ElectronTicket_SendTicket" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript">
        $().ready(function () {
            $('btpost').click(function () {
                alert('pst');
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="tb" runat="server" TextMode="MultiLine" /><br />
    <asp:TextBox ID="tbOut" runat="server" Columns="150" Rows="10" TextMode="MultiLine" />
    <asp:Button ID="btn" runat="server" Text="go" onclick="btn_Click" /><input id="btpost" type="button" value="post" /><br />
    <asp:TextBox ID="tbx" runat="server" TextMode="MultiLine" /><br />
    <asp:TextBox ID="tbK" runat="server" TextMode="MultiLine" Rows="10" Columns="150" /><br />
    <asp:Button ID="btnK" runat="server" Text="k" OnClick="btnK_Click" />
    </div>
    </form>
</body>
</html>
