<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Open_index" EnableViewState="false"%>

<%@ Register Src="../Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="../Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>竞彩赛果开奖-竞彩开奖结果-第一竞彩网</title>
    <meta name="DESCRIPTION" content="第一竞彩网竞彩赛果开奖栏目为大家提供最新的竞彩篮球开奖结果、竞彩足球开奖结果、竞彩足球胜平负开奖等" />
    <meta name="keywords" content="竞彩开奖,竞彩篮球开奖,竞彩足球开奖" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/public.css" rel="stylesheet" type="text/css" />
    <link href="Style/kaijiang.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../Components/My97DatePicker/WdatePicker.js"></script>
<script language="javascript" type="text/javascript">
    function SerachClick() {
        if ($("#tbEndTime").val() < $("#tbBeginTime").val()) {
            alert('截止时间不能小于开奖时间');
            return false;
        }

        location.href = '?startdate=' + $("#tbBeginTime").val() + '&enddate=' + $("#tbEndTime").val() + '&league=' + $("#ddlleague").val();
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:WebHead ID="WebHead1" runat="server" />
        <div id="position">
            <span class="fa">当前位置：<a href="/"><%=_Site.Name %></a> > <a href="/WinQuery/OpenInfoList.aspx">赛果开奖</a> > </span>
            <span class="sub_t">足球开奖</span></div>
        <div id="wrap" class="kj_box">
            <div class="c_box clear">
                <div class="tab_box fl">
                    <a class="kj_btn1 fl" href="index.aspx">足球开奖</a><a class="kj_btn2 fl" href="kj_lanq.aspx">篮球开奖</a></div>
            </div>
            <div class="sg_search clear">
                <div class="fl">
                    赛果查询：<asp:TextBox runat="server" ID="tbBeginTime" Width="140px" onblur="if(this.value=='') this.value=document.getElementById('tbBeginTime').value"
                                                        onFocus="WdatePicker({el:'tbBeginTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" Height="15px" />
                    至
                    <asp:TextBox runat="server" ID="tbEndTime" Width="140px" onblur="if(this.value=='') this.value=document.getElementById('tbEndTime').value"
                    onFocus="WdatePicker({el:'tbEndTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})"
                    Height="15px" />
                    <asp:DropDownList id="ddlleague"  runat="server" CssClass="vcenter"></asp:DropDownList>
                    <input name="searchbutton" type="button" value="查 询" class="sou_btn vcenter" id="searchbutton" onclick="SerachClick();"/>
                </div>
                <p class="sou_info fr red">
                    注：表内奖金，表示单关浮动奖金（单位：2元/注）</p>
            </div>
        </div>
        <div class="data_box">
            <p class="s_rezult">
                查询结果：有 <asp:Label ID="lbNum" runat="server" ForeColor="Red"></asp:Label> 场赛事符合要求</p>
                <asp:Label ID="JczqMatch" runat="server"></asp:Label>
            <div class="page npage">
                <ul>
                    <asp:Label ID="lbPage" runat="server"></asp:Label>
                </ul>
            </div>
        </div>
        <uc2:WebFoot ID="WebFoot1" runat="server" />
    </div>
    </form>
</body>
</html>