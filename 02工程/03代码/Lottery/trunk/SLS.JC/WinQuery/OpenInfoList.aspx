<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OpenInfoList.aspx.cs" Inherits="WinQuery_OpenInfoList" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=LotteryName %>第<%=IsusesName%>期开奖查询—<%=_Site.Name %>－买彩票，就上<%=_Site.Name %>！
    </title>
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
    <link href="../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .hei12{font-size: 12px;color: #333333;font-family: "tahoma";line-height: 20px; }
    .hei12 A:link {font-family: "tahoma";color: #333333;text-decoration: none; }
    .hei12 A:active {font-family: "tahoma";color: #333333;text-decoration: none; }
    .hei12 A:visited {font-family: "tahoma";color: #333333;text-decoration: none; }
    .hei12 A:hover {font-family: "tahoma";color: #ff6600;text-decoration: none; }
    .bg_shang {font-size: 12px;color: #333333;font-family: "tahoma";line-height: 20px;background-image: url(../Home/Room/Images/right_bg_line.jpg);background-repeat: repeat-x;background-position: center top;}
    /*布局样式*/
    #head {width: 980px;padding: 0px;overflow: hidden; }
    .content {width: 970px;padding: 0px; overflow: hidden;margin-top: 10px; }
    .content1_l {float: left;width: 202px; margin-left:10px; padding: 0px;overflow: hidden; }
    .content1_770 {float: left;width: 734px;padding: 0px 0px 0px 10px;overflow: hidden; }
    .red0 {color: Red; }
    </style>
    <link rel="shortcut icon" href="../favicon.ico" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#WebHead1_currentMenu").val("mWinRes");
        });
    </script>
</head>
<body class="gybg">
    <form id="form1" runat="server">
    <asp:HiddenField ID="HidLotteryID" runat="server" />
    <asp:HiddenField ID="HidIsuseID" runat="server" />
    <asp:HiddenField ID="HidPlayType" runat="server" />
    <asp:HiddenField ID="HidSearch" runat="server" />
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">



    <div id="box">
        <div class="content">
            <div class="content1_l">
                <table width="202" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <div class="bsc6">
    <div class="title bg1x bgp3 afff"><a style="cursor: hand;" href="../Help/play_5.aspx"><strong>玩法介绍</strong></a></div>
    <p><a href="5-0-0.aspx" onclick="ChangeSelect(5)"><img height="122" width="105" alt="" src="/Images/_New/ben_09.png"></a></p>
    <p><a href="6-0-0.aspx" onclick="ChangeSelect(6)"><img height="122" width="105" alt="" src="/Images/_New/ben_10.png"></a></p>
    <p><a href="13-0-0.aspx" onclick="ChangeSelect(13)"><img height="120" width="116" alt="" src="/Images/_New/ben_11.png"></a></p>
    <p><a href="28-0-0.aspx" onclick="ChangeSelect(28)"><img height="120" width="130" alt="" src="/Images/_New/ben_12.png"></a></p>
</div>
                        </td>
                    </tr>
               </table>
            </div>
            <div class="content1_770">
                <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E3E3E4">
                    <tr>
                        <td height="50" bgcolor="#FFFFFF">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td bgcolor="#FFFFFF" class="bg_shang" style="padding: 15px 20px 15px 20px;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgLogo" runat="server" Height="45px" Width="283px" />
                                                </td>
                                                <td class="blue12">
                                                    第<span class="red14">&nbsp;<asp:Label ID="lbIsuse" runat="server"></asp:Label>
                                                    </span>期
                                                </td>
                                                <td align="right" class="blue12">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                选择期号：
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlIsuses" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIsuses_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="padding-left: 5px;">
                                                                选择玩法：
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlPlayTypes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPlayTypes_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" bgcolor="#FFFFFF" style="padding: 10px;">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="104" align="right" class="blue14">
                                                    开奖结果：
                                                </td>
                                                <td>
                                                    <table border="0" cellspacing="6" cellpadding="0">
                                                        <tr>
                                                            <td id="tbWinNumber" runat="server" height="25">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="207" class="black12">
                                                                                              </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lbWinInfo" runat="server"></asp:Label>
                                        <div class="red12" style="width: 100%; height: 24px; line-height: 24px; padding-left: 30px;">
                                            <a href="<%= BuyUrl %>">
                                                <%=LotteryName %>投注/合买</a>
                                        </div>
                                        <hr style="border-bottom: #cccccc 1px dashed; border-left: #cccccc 1px dashed; width: 100%;
                                            border-top: #cccccc 1px dashed; border-right: #cccccc 1px dashed; height: 1px;" />
                                        <table align="center" width="80%" border="0" height="100px">
                                            <tr>
                                                <td id="LatestOpenInfo" runat="server" class="blue12">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E3E3E4"  style="margin-top: 10px">
                    <tr>
                        <td height="32" class="bg1x bgp5">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="379">
                                    <div class="bg1x bgp1 tc fw" style="width:140px; height:32px; line-height:32px; font-size:14px; color:#fff;">本站中奖情况</div>                                        
                                    </td>
                                    <td width="231" class="blue12">
                                        <asp:TextBox ID="tbSearch" runat="server" class="in_text" size="40" value="(可输入发起人ID或方案编号搜索)"
                                            onfocus="if(this.value=='(可输入发起人ID或方案编号搜索)')this.value='';" onblur="if(this.value=='')this.value='(可输入发起人ID或方案编号搜索)';"></asp:TextBox>
                                    </td>
                                    <td width="83" class="blue12">
                                        <ShoveWebUI:ShoveConfirmButton ID="btnSearch" Style="cursor: pointer;" BackgroupImage="../Home/Room/Images/button_sousuo.jpg"
                                            runat="server" Height="23px" Width="72px" CausesValidation="False" BorderStyle="None"
                                            OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" bgcolor="#FFFFFF" class="bg_x" style="padding: 12px;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#add3ef">
                                <tbody class="blue12" style="background-color: White; text-align: left; height: 25px;
                                    padding-left: 10px;">
                                    <asp:Repeater ID="rptWinDetail" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="15%" height="28" align="right" bgcolor="#ecf5fc">
                                                    发起人：
                                                </td>
                                                <td width="85%" bgcolor="#ecf5fc">
                                                    <table>
                                                        <tr>
                                                            <td width="30%">
                                                                <strong>
                                                                    <%#Eval("InitiateName")%></strong>
                                                            </td>
                                                            <td align="right" width="30%">
                                                                <asp:ImageButton ID="btn_Single" ImageUrl="../Home/Web/Images/btnzzThis.jpg" runat="server"
                                                                    Width="133px" Height="28px" OnClientClick="return CreateLogin(this);" OnClick="btn_Single_Click"
                                                                    CommandArgument='<%#Eval("InitiateUserID")%>' />
                                                                <asp:ImageButton ID="btn_All" ImageUrl="../Home/Web/Images/btnzzAll.jpg" runat="server"
                                                                    Width="133px" Height="28px" OnClientClick="return CreateLogin(this);" OnClick="btn_All_Click"
                                                                    CommandArgument='<%#Eval("InitiateUserID")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="25" align="right">
                                                    方案类型：
                                                </td>
                                                <td>
                                                    <%#Eval("PlayTypeName")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    方案编号：
                                                </td>
                                                <td>
                                                    <a href="../Home/Room/Scheme.aspx?id=<%#Eval("ID") %>" target="_blank">
                                                        <%#Eval("SchemeNumber")%></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    金额：
                                                </td>
                                                <td>
                                                    共
                                                    <%#Eval("Money")%>
                                                    元 共
                                                    <%#Eval("Share")%>
                                                    份，<%#Eval("EachMonney")%>
                                                    元/份
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    中奖情况：
                                                </td>
                                                <td>
                                                    <%#Eval("WinDescription2")%><span class="red">
                                                        <%#Eval("Multiple")%></span> 倍，共计 <span class="red0"><strong>
                                                            <%#Convert.ToDouble(Eval("WinMoneyNoWithTax")).ToString("N")%></strong></span>
                                                    元（税后）
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    每份派奖金额：
                                                </td>
                                                <td>
                                                    <%#Eval("ShareWinMoney")%>
                                                    元/份
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td>
                                                </td>
                                                <td align="right">
                                                    <asp:HyperLink ID="FollowUser" runat="server" NavigateUrl='<%#"FollowFriendSchemeAdd_User.aspx?FollowUserID="+DataBinder.Eval(Container.DataItem,"InitiateUserID").ToString()+"&FollowUserName="+ HttpUtility.UrlEncode(DataBinder.Eval(Container.DataItem,"InitiateName").ToString())+"&LotteryID=-1"%>'><span class="red12_2">定制自动跟单</span></asp:HyperLink>
                                                </td>
                                            </tr>--%>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 12px;">
                                <tbody id="tbPaging" runat="server" enableviewstate="false">
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>


    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>

    <script src="../JScript/Public.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function showPage(p, l, i, t, s) {
            if (String(s) != "") {
                location.href = String(l) + "-" + String(i) + "-" + String(t) + "-" + String(s) + "-" + String(p) + ".aspx";
            }
            else {
                location.href = String(l) + "-" + String(i) + "-" + String(t) + "-0-" + String(p) + ".aspx";
            }
            //location.href = "Default.aspx?PID=" + String(p) + "&LotteryID=" + String(l) + "&IsuseID=" + String(i) + "&PlayTypeID=" + String(t) + "&Search=" + String(s);
        }

        function ChangeSelect(lotteryID) {
            var arr = new Array(72,73,2,15,39,63,64,3,9,70,62,5,6,13,28,29,74,75);
            for (var i = 0; i < arr.length; i++) {
                var ben_id = document.getElementById("span" + arr[i]);
                if (arr[i] == lotteryID && ben_id) {
                    ben_id.className = "red12";
                }else if(ben_id) {
                    ben_id.className = "hei12";
                }
            }
        }

       ChangeSelect(<%=LotteryID %>);
    </script>

    <!--#includefile="../Html/TrafficStatistics/1.htm"-->
</body>
</html>
