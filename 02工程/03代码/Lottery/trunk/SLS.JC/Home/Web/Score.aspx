<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Score.aspx.cs"
    Inherits="Home_Web_Score" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>历史战绩方案-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。"/>
    <meta name="keywords" content="竞彩开奖，彩票走势图，超级大乐透，排列3/5" />
    <script type="text/javascript">
        var lotteryID = "<%=LotteryID %>";
        function mOver(obj, type) {
            if (type == 1) {
                obj.style.textDecoration = "none";
                obj.style.color = "#FF0065";
            }
            else {
                obj.style.textDecoration = "none";
                obj.style.color = "#226699";
            }
        }
        function ShowOrHiddenDiv(id,rb) {
            var checkedName = "";
            var arrCells = new Array("tdFC","tdZC", "tdTC", "tdJC");
            for (var i = 0; i < arrCells.length; i++) {
                if (arrCells[i] == id) {
                    //document.getElementById(arrCells[i]).style.fontWeight = "bold";
                    //document.getElementById(id).style.color = "#FF0065";
                    document.getElementById("table" + arrCells[i].substring(2)).style.display = "block";
                }
                else {
                    //document.getElementById(arrCells[i]).style.fontWeight = "normal";
                    //document.getElementById(id).style.color = "#226699";
                    document.getElementById("table" + arrCells[i].substring(2)).style.display = "none";
                }
            }
            if (rb != undefined&&rb.length>2) {
                document.getElementById(rb).checked = true;
                return;
            }
            else {
                if (id == "tdFC") {
                    document.getElementById("rb5").checked = true;
                    __doPostBack("rb5");
                    return;
                }
                if (id == "tdTC") {
                    document.getElementById("rb39").checked = true;
                    __doPostBack("rb39");
                    return;
                }
                if (id == "tdZC") {
                    document.getElementById("rb74").checked = true;
                    __doPostBack("rb1");
                    return;
                }
                if (id == "tdJC") {
                    document.getElementById("rb72").checked = true;
                    __doPostBack("rb72");
                    return;
                }
            }

        }
        function __doPostBack(eventTarget, eventArgument) {
            var theform = document.form1;
            theform.__EVENTTARGET.value = eventTarget;
            theform.__EVENTARGUMENT.value = eventArgument;
            theform.submit();
        }
        function clickTabMenu(obj, backgroundImage) {
            if (obj == undefined) {
                obj = document.getElementById("tdZC");
            }
            var tabMenu = obj.offsetParent.cells;
            for (var i = 0; i < tabMenu.length; i++) {
                if (obj == tabMenu[i]) {
                    obj.style.backgroundImage = backgroundImage;
                }
                else {
                    tabMenu[i].style.backgroundImage = "";
                }
            }
        }
        function load() {
           // var temp = document.getElementById("td"+document.getElementById("rb" + lotteryID).parentNode.parentNode.parentNode.parentNode.id.substring(5));
           // clickTabMenu(temp, 'url(../Room/images/admin_qh_100_1.jpg)');
            //ShowOrHiddenDiv(temp.id, "rb" + lotteryID);
        }
    </script>
    <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />
</head><link rel="shortcut icon" href="../../favicon.ico"/>
<body class="gybg">
    <form id="form1" runat="server">
    <asp:PlaceHolder id="phHead" runat="server"></asp:PlaceHolder>

<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">

    <div id="box" style="margin:10px; width:950px;">
        <div style="font-size:12px; font-family:Tahoma;">
            <table cellspacing="0" cellpadding="0" width="100%" align="center" bgcolor="#ffffff" border="0">
                <tr>
                    <td valign="top" align="center">
                    <table width="100%">
                    <tr>
                    <td>
                        用户类型：
                        <asp:Label ID="labUserType" runat="server" ForeColor="Red"></asp:Label>&nbsp;&nbsp; 用户注册时间：
                        <asp:Label ID="labUserRegisterTime" runat="server" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;<strong>用户</strong>
                        <asp:Label ID="labUserName" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label><strong>&nbsp;的历史战绩</strong>
                        &nbsp;&nbsp;<asp:CheckBox ID="cbShowWin" Text="显示中奖方案" runat="server" 
                            AutoPostBack="True" oncheckedchanged="cbShowWin_CheckedChanged" 
                            Checked="True" />
                    </td>
                    <td style="width:280px; margin-right:0px;">
                        <asp:ImageButton ID="btn_Single" ImageUrl="images/btnzzThis.jpg" runat="server" Width="133px" Height="28px" OnClientClick="return CreateLogin(this);" OnClick="btn_Single_Click" />
                        <asp:ImageButton ID="btn_All" ImageUrl="images/btnzzAll.jpg" runat="server" Width="133px" Height="28px" OnClientClick="return CreateLogin(this);" OnClick="btn_All_Click" />
                    </td>
                    </tr>
                    </table>
                    </td>
                </tr>
                <tr>
                      <td>
                         <table id="myIcaileTab" width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;">
                            <tr>
                                
                                <td width="100" id="td1"  align="center" background="../Room/images/admin_qh_100_1.jpg"  class="afff fs14">
                                    福彩
                                </td>
                                
                                <td width="100" id="td2" background="../Room/images/admin_qh_100_2.jpg">
                                    <!---体彩-->
                                </td>
                                
                                <td width="100" id="td3" background="../Room/images/admin_qh_100_2.jpg">
                                    <!---足彩-->
                                </td>
                               
                                <td width="100" id="td4" background="../Room/images/admin_qh_100_2.jpg">
                                    <!---竞彩-->
                                </td>
                                <td height="32" align="left" background="../Room/images/admin_qh_100_2.jpg">
                                    &nbsp;
                                </td>
                                
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td height="1" colspan="9" bgcolor="#FFFFFF">
                                </td>
                            </tr>
                            <tr>
                                <td height="2" colspan="9" bgcolor="#6699CC">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td id="LotteryList">
                    <asp:Panel ID="rbList" runat="server">
                        <table  id="tableFC">
                            <tr style="height: 30px;">
                                <td width="20px">
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb5" runat="server" Checked="true" GroupName="rbLottery" Text="双色球"
                                      AutoPostBack="True"/>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb6" runat="server" GroupName="rbLottery" Text="福彩3D"
                                      AutoPostBack="True"/>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb13" runat="server" GroupName="rbLottery" Text="七乐彩"
                                      AutoPostBack="True"/>                                    
                                </td>
                                <td>
                                <asp:RadioButton ID="rb61" runat="server" GroupName="rbLottery" Text="时时彩"
                                  AutoPostBack="True"/> 
                                </td>
                                <!--td>
                                <asp:RadioButton ID="rb29" runat="server" GroupName="rbLottery" Text="时时乐"
                                  AutoPostBack="True"/> 
                                </td-->
                                <td>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 800px; display:none;" id="tableZC">
                            <tr style="height: 30px;">
                                <td width="20px">
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb74" runat="server" Checked="true" GroupName="rbLottery" Text="胜负彩"
                                      AutoPostBack="True"/>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb75" runat="server" GroupName="rbLottery" Text="任九场"
                                      AutoPostBack="True"/>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb15" runat="server" GroupName="rbLottery" Text="六场半全场"
                                      AutoPostBack="True"/>                                    
                                </td>
                                <td>
                                <asp:RadioButton ID="rb2" runat="server" GroupName="rbLottery" Text="四场进球彩"
                                  AutoPostBack="True"/> 
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 800px; display:none;" id="tableTC">
                            <tr style="height: 30px;">
                                <td width="20px">
                                </td>
                                <td>
                                <asp:RadioButton ID="rb39" runat="server" GroupName="rbLottery" Text="超级大乐透"
                                  AutoPostBack="True" Checked="true"/> 
                                </td>
                                <td>
                                <asp:RadioButton ID="rb3" runat="server" GroupName="rbLottery" Text="七星彩"
                                  AutoPostBack="True"/> 
                                </td>
                                <td>
                                <asp:RadioButton ID="rb9" runat="server" GroupName="rbLottery" Text="22选5"
                                  AutoPostBack="True"/> 
                                </td>
                                <td>
                                <asp:RadioButton ID="rb63" runat="server" GroupName="rbLottery" Text="排列3"
                                  AutoPostBack="True"/> 
                                </td>
                                <td>
                                <asp:RadioButton ID="rb64" runat="server" GroupName="rbLottery" Text="排列5"
                                  AutoPostBack="True"/> 
                                </td>
                                <td>
                                <asp:RadioButton ID="rb70" runat="server" GroupName="rbLottery" Text="11选5"
                                  AutoPostBack="True"/> 
                                </td>
                                <td>
                                <asp:RadioButton ID="rb62" runat="server" GroupName="rbLottery" Text="十一运夺金"
                                  AutoPostBack="True"/> 
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 800px; display:none;" id="tableJC">
                            <tr style="height: 30px;">
                                <td width="20px">
                                </td>
                                <td>
                                <asp:RadioButton ID="rb72" runat="server" GroupName="rbLottery" Text="竞彩足球"
                                  AutoPostBack="True" Checked="true"/> 
                                </td>
                                <td>
                                <asp:RadioButton ID="rb73" runat="server" GroupName="rbLottery" Text="竞彩篮球"
                                  AutoPostBack="True"/> 
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                      </td>
                </tr> 
                <tr>
                    <td valign="top" align="center">
                        <asp:DataGrid ID="g" runat="server" Width="100%" BorderStyle="None" AllowSorting="True"
                            AllowPaging="True" PageSize="30" AutoGenerateColumns="False" CellPadding="4"
                            BackColor="#9FC8EA" Font-Names="Tahoma" OnItemDataBound="g_ItemDataBound" CellSpacing="1"
                            GridLines="None" BorderColor="#E0E0E0" BorderWidth="2px">
                            <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                            <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                            <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
                                BackColor="#E9F1F8" Height="21px"></HeaderStyle>
                            <ItemStyle Height="17px"></ItemStyle>
                            <Columns>
                                <asp:BoundColumn DataField="IsuseName" HeaderText="期号">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="SchemeNumber" HeaderText="方案号">
                                    <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" ForeColor="Blue"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="PlayTypeName" HeaderText="方案类型">
                                    <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Title" HeaderText="方案标题">
                                    <HeaderStyle HorizontalAlign="Center" Width="220px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Money" HeaderText="方案金额">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="WinMoney" HeaderText="税前奖金">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="是否成功">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="QuashStatus"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Buyed"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Schedule"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="id"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Source" Visible="False"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                            </PagerStyle>
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="30"
                            ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#E0E0E0"
                            HotColor="MistyRose" Visible="False" OnPageWillChange="gPager_PageWillChange"
                            OnSortBefore="gPager_SortBefore" RowCursorStyle="" />
                    </td>
                </tr>
                <tr>
                    <td align="center" height="50">
                        【<a onclick="window.close();" href="#">关闭窗口</a>】
                    </td>
                </tr>
            </table>
        </div>
       
    </div>



</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>

    <asp:PlaceHolder id="phFoot" runat="server"></asp:PlaceHolder>
    </form>
    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
            
<script>load();//ShowOrHiddenDiv('tdFC');</script>