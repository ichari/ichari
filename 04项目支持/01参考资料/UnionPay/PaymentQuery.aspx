<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentQuery.aspx.cs" Inherits="Home_Room_OnlinePay_UnionPay_PaymentQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div> 
    <asp:Label ID="lbOrderID" runat="server" Text="请输入要查询的订单号：" /><asp:TextBox ID="txtOrderID" runat="server" Columns="20" />
    <br />
    <asp:Label ID="lbOrderTime" runat="server" Text="请输入要查询的订单时间：" /><asp:TextBox ID="txtOrderTime" runat="server" Columns="20" />
    <br />
    <asp:Button ID="bnSubmit" runat="server" Text="确定" />
    </div>
    </form>
</body>
</html>
