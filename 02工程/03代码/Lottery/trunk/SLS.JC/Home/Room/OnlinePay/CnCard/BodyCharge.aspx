<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BodyCharge.aspx.cs" Inherits="Home_Room_OnlinePay_CnCard_BodyCharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
        /*快速充值*/
        .top-up{height: 110px; margin: 0 0 0 0; border: #bcd2e9 solid 1px;}
        .top-up_cont{ margin: 0px 0 0 0; width:210px;}
        .top-up_cont tr{ line-height: 25px;margin: 0 0 0 0;}
        .top-up_cont tr td span{ width: 100px; float: left; text-align: right; font-size:small;}
    </style>
</head>
<body style="margin: 0 0 0 0;">
    <form id="E_FORM" name="E_FORM" action="https://www.cncard.net/purchase/getorder.asp" method="post" target="_top">
    <table cellpadding="0" cellspacing="0" border="0" class="top-up_cont">
        <tr>
            <td>
                <span>会员名：</span><input type="text" size="9" name="UserName"  id="UserName"/>
            </td>
        </tr>
        <tr>
            <td style="font-size:small;">
                <span>充值金额：</span>
                <select id="moneySelect" name="moneySelect" onchange="showMoney(this.options(this.selectedIndex).value)">
                    <option name="money" value="50">选择金额 </option>
                    <option name="money" value="10">10</option>
                    <option name="money" value="20">20</option>
                    <option name="money" value="50">50</option>
                    <option name="money" value="100">100</option>
                </select>元
            </td>
        </tr>
        <tr>
            <td>
                <span>支付类型：</span><select name="">
                    <option>云网支付 </option>
                </select>
            </td>
        </tr>
        <tr>
            <td style="text-align:center;">
                <input type="button" value="马上充值" style="font-size:small" onclick="checkChargeFormat();"/>
            </td>
        </tr>
    </table>
    <input type="hidden" value="<%=c_mid%>" name="c_mid" id="c_mid" />
    <input type="hidden" value="<%=c_order%>" name="c_order" id="c_order"/>
    <input type="hidden" value="<%=c_orderamount%>" name="c_orderamount" id="c_orderamount"/>
    <input type="hidden" value="<%=c_ymd%>" name="c_ymd" id="c_ymd" />
    <input type="hidden" value="<%=c_moneytype%>" name="c_moneytype" id="c_moneytype" />
    <input type="hidden" value="<%=c_retflag%>" name="c_retflag" id="c_retflag" />
    <input type="hidden" value="<%=c_paygate%>" name="c_paygate" id="c_paygate" />
    <input type="hidden" value="<%=c_returl%>" name="c_returl" id="c_returl" />
    <input type="hidden" value="<%=c_memo1%>" name="c_memo1" id="c_memo1" />
    <input type="hidden" value="<%=c_memo2%>" name="c_memo2" id="c_memo2" />
    <input type="hidden" value="<%=c_language%>" name="c_language" id="c_language" />
    <input type="hidden" value="<%=notifytype%>" name="notifytype" id="notifytype" />
    <input type="hidden" value="<%=c_signstr%>" name="c_signstr" id="c_signstr" />
    </form>
    <form id="aaa1" runat="server">
    </form>
</body>
</html>
<script type="text/javascript">
    function checkChargeFormat() {
        var UserName = document.getElementById("UserName").value;

        if (UserName == "") {
        
            alert("会员名不能为空！");
            return false;
        }

        var UserID = Home_Room_OnlinePay_CnCard_BodyCharge.GetUserInfoByName(UserName).value;

        if (isNaN(UserID)) {
            alert(UserID);
            return false;
        }

        var PayMoney = document.getElementById("c_orderamount").value;

        var PayNumber = Home_Room_OnlinePay_CnCard_BodyCharge.GetNewPayNumber(UserID, PayMoney).value;

        if (PayNumber == "") {
            alert("获取订单ID出错，请刷新页面！");
            return false;
        }

        document.getElementById("c_order").value = PayNumber.split('|')[0];
        document.getElementById("c_memo1").value = PayNumber.split('|')[1];

        var c_mid = document.getElementById("c_mid").value;
        var c_order = document.getElementById("c_order").value;
        var c_orderamount = document.getElementById("c_orderamount").value;
        var c_ymd = document.getElementById("c_ymd").value;
        var c_moneytype = document.getElementById("c_moneytype").value;
        var c_retflag = document.getElementById("c_retflag").value;
        var c_returl = document.getElementById("c_returl").value;
        var c_paygate = document.getElementById("c_paygate").value;
        var c_memo1 = document.getElementById("c_memo1").value;
        var c_memo2 = document.getElementById("c_memo2").value;
        var notifytype = document.getElementById("notifytype").value;
        var c_language = document.getElementById("c_language").value;

        document.getElementById("c_signstr").value = Home_Room_OnlinePay_CnCard_BodyCharge.GetSign(c_mid, c_order, c_orderamount, c_ymd, c_moneytype, c_retflag, c_returl,
                                                            c_paygate, c_memo1, c_memo2, notifytype, c_language).value;

        document.E_FORM.submit();
    }

    function showMoney(str) {
        if (str != -1) {
            document.getElementById("c_orderamount").value = str;
        }
    }

</script>