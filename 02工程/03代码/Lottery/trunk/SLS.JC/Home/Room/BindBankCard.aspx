<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BindBankCard.aspx.cs" Inherits="Home_Room_BindBankCard" %>

<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>购彩充值提款银行卡绑定-我的资料-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function NoAllowPaste() {
            var s = clipboardData.getData('text');
            if (!/\D/.test(s)) value = s.replace(/^0*/, '');
            return false;
        }

        function showSameHeight() {
            if (document.getElementById("menu_left").clientHeight < document.getElementById("menu_right").clientHeight) {
                document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
            }
            else {
                if (document.getElementById("menu_right").offsetHeight >= 960) {
                    document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
                }
                else {
                    document.getElementById("menu_left").style.height = "960px";
                }
            }
        }
    </script>
    <style type="text/css">
        .style2
        {
            font-size: 12px;
            color: #CC0000;
            font-family: "tahoma";
            line-height: 20px;
        }
    </style>
    <link rel="shortcut icon" href="/favicon.ico" />
</head>
<body onload="showSameHeight();"class="gybg">
<form id="form1" runat="server">
<asp:HiddenField ID="hfIsBindFlag" runat="server" Value="true" />
<asp:HiddenField ID="hfBankInProvince" runat="server" Value="true" />
<asp:HiddenField ID="hfBankInCity" runat="server" Value="true" />
<asp:HiddenField ID="hfBankTypeName" runat="server" Value="true" />
<asp:HiddenField ID="hfBankName" runat="server" Value="true" />
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" CurrentPage="acBankCard" />
        </div>
        <div id="menu_right">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="border:#E4E4E5 1px solid;">
                <tr>
                    <td width="40" height="30" align="right" valign="middle" class="red14">
                        <img src="images/user_icon_man.gif" width="19" height="16" />
                    </td>
                    <td valign="middle" class="red14" style="padding-left: 10px;">
                        我的资料
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" class="bg1x bgp5" cellpadding="0" style="margin-top: 10px;">
                <tr>
                    <td class="afff fs14 " style="width:100px;height:32px;text-align:center;background-image:url('images/admin_qh_100_1.jpg');">
                        <a href="UserEdit.aspx"><strong>绑定银行</strong></a>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#DDDDDD">
                <tr>
                    <td height="30" align="left" bgcolor="#F3F3F3" class="black12" style="padding: 10px;">
                        <p>
                            尊敬的会员 <span class="red12"><asp:Label ID="Label1" runat="server"></asp:Label></span>： 
                            <span class="red12">( 提示：请勿在公共场所及网吧使用 )</span><br />
                            <asp:Label ID="lbStatus" runat="server"></asp:Label>银行卡，提款时款项将汇至您以下绑定银行卡中，<br />
                            绑定成功后用户不能自行修改，如需修改，请联系网站客服。
                        </p>
                    </td>
                </tr>
            </table>
            <table border="0" cellspacing="0" cellpadding="0" style="width:100%;margin:10px;width:100%;">
                <tr>
                    <td style="width:20%;height:30px;text-align:right;" class="black12">
                        银行卡状态：
                    </td>
                    <td style="width:80%;padding-left:10px;">
                        <asp:Label ID="labBindState" Text="" runat="server" ForeColor="#CC0000"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        用户名：<span class="red12"></span>
                    </td>
                    <td class="black12" style="padding-left:10px;">
                        <asp:Label ID="labName" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <%--<tr bgcolor="#C0DBF9">
                   <td height="30" align="right" bgcolor="#FFFFFF" class="black12">银行卡发卡地区：</td>
                   <td align="left" bgcolor="#FFFFFF" class="black12" style="padding-left:10px;"><label>
                     <input name="radio" type="radio" id="radio" value="radio" checked="checked" />
                   深圳本地 
                   <input type="radio" name="radio2" id="radio2" value="radio2" />
                   其他  </label></td>
                 </tr>--%>
                <tr>
                    <td style="height:30px;text-align:right;vertical-align:text-bottom;" class="black12">
                        <span style="color: Red;">*</span>开户银行：
                    </td>
                    <td style="padding-left:10px;vertical-align:baseline;">
                        <div style="margin-bottom:4px;">
                            <select id="selProvince" name="selProvince" style="width:80px;" onchange="selProvince_onchange(this)">
                                <option></option>
                            </select>
                            (省/直辖)&nbsp;&nbsp;<select id="selCity" name="selCity" style="width:80px;" onchange="selCity_onchange(this)">
                                <option></option>
                            </select>
                            (市)
                        </div>
                        <div style="margin-bottom:4px;">
                            <select id="selBankTypeName" name="selBankTypeName" style="width:153px" onchange="selBankTypeName_onchange(this)">
                                <option></option>
                            </select>
                            (银行)
                        </div>
                        <div style="margin-bottom:4px;">
                            <select id="selBankName" name="selBankName" style="width:250px;">
                                <option></option>
                            </select>
                            (支行名称)
                        </div>
                        <span class="blue12" style="color: Red;">请确认选择了正确的银行卡开户银行地址和支行名称,否则提款将会拒绝受理. </span>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        银行卡号码：
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <asp:TextBox ID="tbBankCardNumber" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                        <asp:Label ID="lbBankCardNumber" runat="server" Text="" Visible="false"></asp:Label>
                        <span class="blue12" style="color: Red;">*</span>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;">&nbsp;</td>
                    <td style="padding-left:10px;">
                        <span class="blue12">工商银行、招商银行、建设银行卡，提款24小时内到帐；其它银行3~5个工作日到帐</span>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        确认银行卡号码：
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <asp:TextBox ID="tbBankCardNumberOK" runat="server" MaxLength="50" onpaste="return false" Width="250px"></asp:TextBox>
                        <asp:Label ID="lbBankCardNumberOK" runat="server" Width="120" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        持卡人真实姓名：
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <asp:TextBox ID="tbBankCardRealityName" runat="server" MaxLength="50" Width="122px"></asp:TextBox>
                        <asp:Label ID="lbBankCardRealityName" runat="server" Text="" Visible="false"></asp:Label>
                        <span style="color:Red;">*</span>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;" colspan="2">
                        <div class="hr_bar"></div>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;">
                        &nbsp;
                    </td>
                    <td class="red12" style="padding-left:10px;">
                        （为了您的账户安全，请您输入正确账户密码进行确认）
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;text-align:right;" class="black12">
                        账户密码：
                    </td>
                    <td style="padding-left:10px;">
                        <asp:TextBox ID="tbVerPwd" runat="server" CssClass="in_text_hui" Width="160" MaxLength="20" TextMode="Password" />
                        <asp:Label ID="lbErrPwd" runat="server" CssClass="red12" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;" colspan="2">
                        <div class="hr_bar"></div>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;">
                        &nbsp;
                    </td>
                    <td style="padding-left:10px;" class="black12">
                        <label>
                            <ShoveWebUI:ShoveConfirmButton ID="ShoveConfirmButton1" runat="server" AlertText="确信输入的银行卡无误，并立即绑定吗？"
                                Text="确定绑定" OnClick="btnOK_Click" Style="cursor: pointer;" CssClass="btn_s" />
                        </label>
                        &nbsp;&nbsp;&nbsp;<span class="blue12" style="color: Red;">提示：带*号标记的项必输入正确填写，否则提款将被拒绝。
                        </span>
                    </td>
                </tr>
            </table>
            <div style="padding:5px 10px 5px 10px;background-color:#FFFEDF;border:1px solid #D8D8D8;">
                <span class="blue14" style="padding: 5px 10px 5px 10px;">如有其他问题，请联系网站客服：
                    <span class="red14"><%= _Site.ServiceTelephone %></span>
                </span>
            </div>
        </div>
    </div>
</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>
<asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
<asp:HiddenField  ID="HidBankName" runat ="server" />
<asp:HiddenField  ID="HidBankName1" runat ="server" />
</form>
</body>
</html>
<script type="text/javascript" language="javascript">
    function OnInit()
    {
        //获取已经绑定的资料
        var curBankInProvince = document.getElementById("hfBankInProvince").value;
        var curBankInCity = document.getElementById("hfBankInCity").value;
        var curBankTypeName = document.getElementById("hfBankTypeName").value;
        var curBankName = document.getElementById("hfBankName").value;
        
        //绑定省份数据列表
        var selProvince = document.getElementById("selProvince");
        while (selProvince.options.length > 0)
        {
            selProvince.options[0] = null;
        }
        var listStr = Home_Room_BindBankCard.GetProvinceList().value;
        var listNames = listStr.split("|");
        for (var i = 0; i < listNames.length; i++)
        {
            selProvince.options[selProvince.options.length] = new Option(listNames[i], listNames[i]);
        }
        selProvince.value = curBankInProvince;
        
        //绑定银行类名称列表
        var selBankTypeName = document.getElementById("selBankTypeName");
        while (selBankTypeName.options.length > 0)
        {
            selBankTypeName.options[0] = null;
        }
        var listBankNameStr = Home_Room_BindBankCard.GetBankTypeList().value;
        var listBankNames = listBankNameStr.split("|");

        for (var i = 0; i < listBankNames.length; i++)
        {
            selBankTypeName.options[selBankTypeName.options.length] = new Option(listBankNames[i], listBankNames[i]);
        }
        selBankTypeName.value = curBankTypeName;

        //绑定市名
        selProvince_onchange(selProvince);
        var selCity = document.getElementById("selCity");
        selCity.value = curBankInCity;

        //显示支行
        GetBankBranchNameList(curBankInCity, curBankTypeName);
        var selBankName = document.getElementById("selBankName");
        selBankName.value=curBankName
    }

    function selProvince_onchange(obj)
    {
        var selProvince = obj;
        var selectProvinceName = selProvince.value;
        //alert(selProvince.value);
        //清空城市
        var selCity = document.getElementById("selCity");
        while (selCity.options.length > 0)
        {
            selCity.options[0] = null;
        }

        var listStr = Home_Room_BindBankCard.GetCityList(selectProvinceName).value;
        //alert(listStr);
        var listNames = listStr.split("|");
        for (var i = 0; i < listNames.length; i++)
        {
            selCity.options[selCity.options.length] = new Option(listNames[i], listNames[i]);
        }
        selCity.value = "";
    }

    function selCity_onchange(objSender)
    {
        var selCityObj = document.getElementById("selCity");
        var selectCityName = selCityObj.value;
        var selBankTypeNameObj = document.getElementById("selBankTypeName");
        var selectBankName = selBankTypeNameObj.value;

        GetBankBranchNameList(selectCityName, selectBankName);
    }

    function selBankTypeName_onchange(objSender)
    {
        var selCityObj = document.getElementById("selCity");
        var selectCityName = selCityObj.value;
        var selBankTypeNameObj = document.getElementById("selBankTypeName");
        var selectBankTypeName = selBankTypeNameObj.value;
        GetBankBranchNameList(selectCityName, selectBankTypeName);
    }

    function GetBankBranchNameList(cityName, bankTypeName)
    {
        //alert(cityName + " | " + bankTypeName);
        //清空银行下拉表
        var selBankName = document.getElementById("selBankName");
        while (selBankName.options.length > 0)
        {
            selBankName.options[0] = null;
        }

        var listStr = Home_Room_BindBankCard.GetBankBranchNameList(cityName, bankTypeName).value;
        //alert(listStr);
        var listNames = listStr.split("|");
        for (var i = 0; i < listNames.length; i++)
        {
            if (listNames[i] != "")
            {
                //支行下拉框
                selBankName.options[selBankName.options.length] = new Option(listNames[i], listNames[i])
            }
        }
        selBankName.value = "";
    }
    OnInit();
</script>