<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CoBuy.aspx.cs" Inherits="Home_Room_CoBuy" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>参与合买-<%=_Site.Name %>-买彩票，就上<%=_Site.Name %>！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="参与合买" />
    <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../../favicon.ico" />
    <script src="../../JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <style type="text/css">
    
/*--------重置图片元素--------*/ 
img{ border:0px;vertical-align:middle;}  
.home_dl{ margin-right:10px; float:left;}
.M_box{}
.M_title{height:30px; overflow:hidden; line-height:32px;}
.M_title h2 {font-size:12px; color:red; float:left; text-indent:10px; display:inline; padding:0; margin:0;}
.M_title .more { color:#fff; font-size:12px; float:right; margin-right:10px; display:inline;}
.M_title h2 a,.M_title .more a,.M_title h2 a:hover{color:#fff;}
.M_content{border:1px solid #b1c7df; border-top:0; background:#fff; zoom:1}

/*合买*/
.hemai .M_title{padding-top:2px; line-height:28px; overflow:visible; height:28px;}
.hemai .M_content{overflow:hidden; background:#f8f8f8;}

/*合买指引*/
.guide .r_content2{height:47px; overflow:hidden; padding:10px 9px;}
.guide ul li{background:url(../images/list_bg.gif) no-repeat 2px 10px; padding-left:10px; width:45%; line-height:24px; float:left;}

/*toolTips*/
.toolTips{display:inline-block; zoom:1;margin:7px 0 0 5px; margin:0px 0 0 5px; _margin:7px 0 0 5px;}
.toolTips{display:inline; float:left;}
.toolTips{height:14px;width:14px;position:relative;z-index:1;}
.toolTips a{text-decoration:none;}
/*合买帮助*/
.notifyicon{position:absolute;width:230px;z-index:10000;top:14px; top:21px; _top:14px;left:-9999px;filter: progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#999999');
-ms-filter: "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#999999')"
}
.notifyicon_content{
-moz-box-shadow: 2px 3px 4px #666;
-webkit-box-shadow: 2px 3px 4px #666;
background:#FFFFE1;border:1px solid #666666;padding:10px;font:12px/1.5 verdana;-moz-border-radius:8px;-webkit-border-radius:8px;border-radius:8px;}
.notifyicon h5{margin:0 0 10px 0;padding:0 0 0 20px;font:bold 12px/1.5 verdana;background:url(../images/tip.gif) no-repeat 0 0;}
.notifyicon p{margin:0 0 10px 0;padding:0;}
.notifyicon_content a{color:#4100FC}
.notifyicon_content a:hover{text-decoration:none;}
.notifyicon_arrow{width:18px;height:18px;overflow:hidden;position:absolute;z-index:9;left:15px;top:1px;}
.notifyicon_space{width:15px;height:18px;overflow:hidden;}
.notifyicon_arrow s, .notifyicon_arrow em{display:block;width:0;height:0;overflow:hidden;border-style:dashed dashed dashed solid;top:0;position:absolute;}
.notifyicon_arrow s{border-width:18px;border-color:transparent transparent transparent #666666;left:0;}
.notifyicon_arrow em{border-width:16px;border-color:transparent transparent transparent #FFFFE1;left:1px;top:2px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="HidIsuseID" runat="server" />
    <asp:HiddenField ID="HidLotteryID" runat="server" Value="39" />
    <asp:HiddenField ID="HidNumber" runat="server" />
    <div style="padding-bottom: 8px;">
        <table cellpadding="0" cellspacing="0" style="width: 100%;height: 50px" bgcolor="#F2F2F2">
            <tr>
                <td style="width: 90px;">
                    <a href="javascript:newCoBuy();" style="font-weight: bold; color: #FF0065; padding-left: 10px">
                        发起合买方案</a>
                </td>
                <td style="padding-left: 5px; width: 80px">
                    <asp:TextBox ID="TxtName" runat="server" CssClass="in_text" value="输入用户名" size="18"
                        onfocus="if(this.value=='输入用户名')this.value='';" onblur="if(this.value=='')this.value='输入用户名';"></asp:TextBox>
                </td>
                <td style="padding-left: 8px;" width="60px">
                    <ShoveWebUI:ShoveConfirmButton ID="btnSearch" runat="server" BackgroupImage="images/button_sousuo.jpg"
                        Style="cursor: hand;" BorderStyle="None" Height="23px" OnClick="btnSearch_Click"
                        Width="72px" />
                </td>
                <td>
                    <div class="home_dl" style="display:none">
                        <div class="hemai">
                            <div class="M_box">
                                <div class="M_title">
                                    <h2>
                                        置顶规则</h2>
                                    <div class="toolTips">
                                        <a title="说明" href="javascript:void(0)" id="indexhmsm">
                                            <img src="../../Images/why.gif"></a>
                                        <div class="notifyicon" id="indexhmtipsdetail">
                                            <div class="notifyicon_space">
                                            </div>
                                            <div class="notifyicon_arrow">
                                                <s></s><em></em>
                                            </div>
                                            <div class="notifyicon_content">
                                                <p>
                                                    <asp:Label ID="lbContent" runat="server"></asp:Label>
                                                 </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
                <td style="padding-left: 8px;" id="Personages" runat="server">
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="divData" visible="false">
        <asp:DataGrid ID="g" runat="server" Width="100%" AllowSorting="True" bordercolorlight="#BCD2E9"
            bgcolor="#E9F1F8" AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
            CellPadding="0" BackColor="#E9F1F8" OnItemDataBound="g_ItemDataBound" CellSpacing="0"
            GridLines="None" BorderStyle="None">
            <HeaderStyle HorizontalAlign="Center" CssClass="blue12_line" Height="30px" ForeColor="#0066BA">
            </HeaderStyle>
            <ItemStyle Height="30px" HorizontalAlign="Center" BackColor="#FFFFFF" CssClass="blue12">
            </ItemStyle>
            <Columns>
                <asp:BoundColumn DataField="SchemeNumber" HeaderText="方案号" Visible="False">
                    <HeaderStyle HorizontalAlign="Center" Width="16%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="InitiateName" HeaderText="发起">
                    <HeaderStyle HorizontalAlign="Center" Width="13%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="Black"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Level" HeaderText="战绩" SortExpression="Level">
                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Money" HeaderText="总金额" SortExpression="Money">
                    <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="PlayTypeName" HeaderText="玩法" SortExpression="Money">
                    <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="LotteryNumber" HeaderText="投注内容">
                    <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Share" HeaderText="份数" SortExpression="Share">
                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="每份">
                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Assure" HeaderText="进度">
                    <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="right"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="状态">
                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="入伙">
                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="Red"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="讨论" Visible="false">
                    <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="Red"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="AssureMoney"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="InitiateUserID"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="QuashStatus"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="PlayTypeID"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="Buyed"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="SecrecyLevel"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="EndTime"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="IsOpened"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="LotteryID"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="IsTop"></asp:BoundColumn>
            </Columns>
            <PagerStyle Visible="False"></PagerStyle>
        </asp:DataGrid>
        <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="10"
            ShowSelectColumn="False" DataGrid="g" AlternatingRowColor="#F8F8F8" GridColor="#FFFFFF"
            HotColor="MistyRose" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore"
            RowCursorStyle="" />
    </div>
    <div id="divLoding" runat="server" style="line-height: 35px; height: 100%; overflow: hidden;"
        visible="true">
        <img src='images/londing.gif' style="position: relative; top: 10%; left: 40%;" alt="" />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>

<script type="text/javascript" language="javascript">
    $(function(d, t) {
        t = $('#indexhmtipsdetail');
        function a() {
            t.css('left', '-9999px');
        }
        function b() {
            d = setTimeout(a, 200);
        }
        $('#indexhmsm').mouseover(function() {
            clearTimeout(d);
            t.css('left', 0);
        }).mouseout(b);
        t.mouseover(function() {
            clearTimeout(d);
        }).mouseout(b);
    });
    
    function newCoBuy() {
       parent.document.getElementById("TabMenu").childNodes(1).click();
    }

    function personages(name) {
        document.getElementById("TxtName").value = name;

    }

    function postback() {
        __doPostBack('btnSearch', '');
    }
    
    function document.onreadystatechange()
        {
            if (document.readyState=="complete") {
            <%= script.ToString() %>
            }
    }

</script>