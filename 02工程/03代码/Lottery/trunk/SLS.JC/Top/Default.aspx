<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Top_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=_Site.Name %>-跟单管家</title>
    <link href="../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../style/paihang.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
    <link rel="shortcut icon" href="../favicon.ico" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#WebHead1_currentMenu").val("mMngm");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
    <div style="padding-top:10px"></div>
    <div class="wrap">
        <div class="paihang_left">
            <div>
                <div class="left_title01">
                    排 行 榜</div>
                 <div class="box01">
                    <div class="t_01">
                        <img src="Images/row02.gif" align="absmiddle" />
                        竞彩彩票</div>
                    <div>
                        <ul class="ul_01">
                            <li class="bg_01">·<a href="Default.aspx?lot=jczq">竞彩足球排行榜</a></li>
                            <li class="bg_02">·<a href="Default.aspx?lot=jclq">竞彩篮球排行榜</a></li>
                        </ul>
                    </div>
                </div>
                <div class="box01">
                    <div class="t_01">
                        <img src="Images/row01.gif" align="absmiddle" />
                        足球彩票</div>
                    <div>
                        <ul class="ul_01">
                            <li class="bg_01">·<a href="Default.aspx?lot=sfc">足彩胜负排行榜</a></li>
                            <li class="bg_02">·<a href="Default.aspx?lot=rj">任选9场排行榜</a></li>
                            <li class="bg_01">·<a href="Default.aspx?lot=6cbq">6场半全排行榜</a></li>
                            <li class="bg_02">·<a href="Default.aspx?lot=4cjq">4场进球排行榜</a></li>
                        </ul>
                    </div>
                </div>
                <div class="box01">
                    <div class="t_01">
                        <img src="Images/row03.gif" align="absmiddle" />
                        体育彩票</div>
                    <div>
                        <ul class="ul_01">
                            <li class="bg_01">·<a href="Default.aspx?lot=dlt"><font color="red">超级大乐透排行榜</font></a>
                                <a href="Default.aspx?lot=dlt">
                                    <img src="Images/new.gif" /></a></li>
                            <li class="bg_02">·<a href="Default.aspx?lot=pls">排列3排行榜</a></li>
                            <li class="bg_01">·<a href="Default.aspx?lot=qxc">七星彩排行榜</a></li>
                            <li class="bg_02">·<a href="Default.aspx?lot=22x5">22选5排行榜</a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="kong10">
            </div>
        </div>
        <div class="paihang_right">
            <div class="box01">
                <div class="t_01">
                    <span class="p_left">
                        <img src="<%=ImageUrl %>" /></span>
                    <span class="font001">排行榜</span><span class="p_right"></span>
                </div>
            </div>
            <div>
                <table class="bq_01" id="menutab1" width="50%" border="0" cellspacing="5" cellpadding="0">
                    <!--菜单-->
                    <tr>
                        <td width="20%" class="td_02">
                            <a href="javascript:void(0)" onclick="onclickmenu(1, this, <%=LotteryID %>)">发起盈利排行榜</a>
                        </td>
                        <td width="20%" class="td_01">
                            <a href="javascript:void(0)" onclick="onclickmenu(2, this, <%=LotteryID %>)">合买人气排行榜</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="kong10">
            </div>
            <div class="bq_02">
                <ul id="menutab2">
                    <li class="li_01"><a href="#" id="sfc">周 榜</a></li>
                    <li class="li_02"><a href="#" id="sfc">月 榜</a></li>
                    <li class="li_01"><a href="#" id="sfc">赛季榜</a></li>
                    <li class="li_right" id="query">
                        <div style="display: none;">
                            过往回查:<input type="text" name="week" id="week" value="<%=DTime %>" readonly="true"
                                onblur="if(this.value=='') this.value=document.getElementById('week').value"
                                                        name="week" onFocus="WdatePicker({el:'week',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" size="10" />
                            <img height="21" style="cursor: pointer" src="Images/calendar.gif"
                                width="25" align="absMiddle">
                            <input type="button" id="submit1" name="submit" value="查询" />
                        </div>
                        <div style="display: block;">
                            过往回查:<select name="year" id="year">
                                <option value="2011">2011</option>
                                <option value="2010">2010</option>
                                <option value="2009">2009</option>
                                <option value="2008">2008</option>
                                <option value="2007">2007</option>
                                <option value="2006">2006</option>
                                <option value="2005">2005</option>
                                <option value="2004">2004</option>
                            </select>
                            <select name="month" id="month">
                                <option value="12">12</option>
                                <option value="11">11</option>
                                <option value="10">10</option>
                                <option value="9">9</option>
                                <option value="8">8</option>
                                <option value="7">7</option>
                                <option value="6">6</option>
                                <option value="5">5</option>
                                <option value="4">4</option>
                                <option value="3">3</option>
                                <option value="2">2</option>
                                <option value="1">1</option>
                            </select>
                            <input type="button" id="submit2" name="submit" value="查询" />
                        </div>
                        <div style="display: none;">
                            过往回查:<select name="season" id="season">
                                <option value="1383">10-11赛季</option>
                                <option value="1343">09-10赛季</option>
                                <option value="1243">08-09赛季</option>
                                <option value="30">07-08赛季</option>
                                <option value="3">06-07赛季</option>
                                <option value="2">05-06赛季</option>
                                <option value="1">04-05赛季</option>
                            </select>
                            <input type="button" id="submit3" name="submit" value="查询" />
                        </div>
                    </li>
                </ul>
            </div>
            <iframe id="fra" name="fra" src="list.aspx?lot=<%=lot%>" frameborder="0" allowtransparency="true"
                scrolling="no" width="100%"></iframe>
        </div>
    </div>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
        <script language="javascript" type="text/javascript" src="../JScript/boot.js"></script>
    <script language="javascript" type="text/javascript" src="../JScript/top.js"></script>
    <script language="javascript" type="text/javascript" src="../Components/My97DatePicker/WdatePicker.js"></script>
    </form>
    <!--#includefile="../Html/TrafficStatistics/1.htm"-->
</body>
</html>
