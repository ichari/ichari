<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Buy_SF_DG.aspx.cs" Inherits="JCZC_Buy_SF_DG" EnableViewState="false"%>
<%@ Register src="../Home/Room/UserControls/WebHead.ascx" tagname="WebHead" tagprefix="uc1" %>
<%@ Register src="../Home/Room/UserControls/WebFoot.ascx" tagname="WebFoot" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>胜负-竞彩篮球</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/tips_globle.css" rel="stylesheet" type="text/css" />
    <link href="Style/public.css" rel="stylesheet" type="text/css" />
    <link href="Style/trade.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table.showodds .odds, table.showrate .rate{display: table-cell;_display: block;}
        table.showodds .rate, table.showrate .odds{display: none;}
    </style>
</head>
<body>
    <form id="buy_form" runat="server" ajax="Buy_Dg_Handler.ashx">
    <uc1:WebHead ID="WebHead1" runat="server" />
    <div id="gc_bg">
        <div id="position">
            <span class="fa">当前位置：<a href="/" target="_parent">首页</a> > <a href="/jclc/buy_sf.aspx" target="_parent">
                篮球投注</a></span></div>
        <div id="wrap">
            <div class="nav_button">
                <ul>
                    <li class="butt1"><a href="Buy_SF.aspx">胜负</a></li>
                    <li class="butt2"><a href="Buy_RFSF.aspx">让分胜负</a></li>
                    <li class="butt2"><a href="Buy_SFC.aspx">胜分差</a></li>
                    <li class="butt2"><a href="Buy_DX.aspx">大小分</a></li>
                    <li style="float:right; width:280px; text-align:right;">
                        <a href="/ZX.aspx?NewsName=jclq" target="_blank">竞彩资讯</a>&nbsp;&nbsp;|
                        <a href="/Join/Project_List.aspx?id=73">参与合买</a>&nbsp;&nbsp;|
                        <a href="/Open/kj_lanq.aspx"  target="_blank">开奖公告</a>&nbsp;&nbsp;|
                        <a href="/Help/play_7301.aspx" target="_blank">玩法介绍</a>                        
                    </li>
                 </ul>
            </div>
            <div class="choice">
                <div class="fsds">
                    <div class="l">
                        <input type="radio" checked="checked">
                        <label for="dgjc">
                            单关投注<span class="gray2">(浮动奖金)</span></label>
                        <input type="radio" onclick="window.location.href='Buy_SF.aspx'">
                        <label for="ggjc">
                            过关投注<span class="gray2">(固定奖金)</span></label>
                    </div>
                </div>
            </div>
            <!--投注开始 -->
            <!-- 开始选择 !-->
            <div class="touzhu" style="border-top-color: #FFF;">
                <div class="reply_type_01">
                    <p class="time_scroll">
                        <span class="time_scroll2" id="TimeDisplay" style="position:relative; right:12px; top:3px; float:right; width:112px; padding:0 5px; display:block; text-align:center; height:18px; line-height:18px; background:#000; color:#0f0;"></span>
                    </p>
                    <img src="Images/btn_sai.gif" style="cursor: pointer;" onclick="document.getElementById('leagueBox').style.display='block'"
                        align="absmiddle" alt="赛事选择">
                    已隐藏<span class="red" id="hideCount">0</span>场比赛 <a href="javascript:void 0" id="showAllTeam">
                        <font color="#ff0000">恢复</font></a>
                </div>
                <div>
                <div class="ss_slect">
                    <table style="border: 1px solid #1989D7; display: none;" id="leagueBox" width="115"
                        bgcolor="#ffffff" border="0" cellpadding="3" cellspacing="1">
                        <tbody>
                            <tr>
                                <td bgcolor="#D1E7FC" height="22">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    &nbsp;赛事选择
                                                </td>
                                                <td width="1" align="right">
                                                    <img src="Images/btnClose.gif" alt="关闭" style="cursor: pointer; padding-right: 3px;"
                                                        onclick="document.getElementById('leagueBox').style.display='none'" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= lgList%>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" bgcolor="#f1f8fe" height="25">
                                    <img src="Images/select_all.gif" alt="全选" style="cursor: pointer;" id="selectAllBtn" /><img
                                        src="Images/select_alt.gif" alt="反选" style="cursor: pointer;" id="selectOppBtn" /><img
                                            src="Images/close2.gif" alt="关闭" style="cursor: pointer;" onclick="document.getElementById('leagueBox').style.display='none'" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                </div>
                <!-- 日期选择开始 -->
                <div style="position: absolute; left: 530px; top: 230px;">
                </div>
                <!-- 开始对阵列表 !-->
<!-- Left -->
<div class="touzhu" style="width:750px; float:left;border-top-color: #FFF;">
                <table class="ttd showodds" id="table_vs" width="100%" border="0" cellpadding="0"
                    cellspacing="1">
                    <tbody>
                        <tr class="gg_form_tr_01_lq">
                            <td width="100" rowspan="2">
                                赛事编号
                            </td>
                            <td width="160" rowspan="2">
                                赛事类型
                            </td>
                            <td width="100" rowspan="2">
                                截止时间
                            </td>
                            <td width="130" rowspan="2">
                                客队
                            </td>
                            <td width="130" rowspan="2">
                                主队
                            </td>
                            <td colspan="3">
                                请选择投注
                            </td>
                        </tr>
                        <tr class="gg_form_tr_02_lq">
                            <td width="80" class="s">
                                主负
                            </td>
                            <td width="80" class="s">
                                主胜
                            </td>
                            <td width="50">
                                全包
                            </td>
                        </tr>
                        <%= MatchList %>
                    </tbody>
                </table>
                <div class="sg_text01">
                    <strong>投注提示：</strong><br />
                    1.竞猜全场比赛包含加时赛。<br />
                    2.让分只适合&ldquo;让分胜负&rdquo;玩法,&ldquo;+&rdquo;为客让主，&ldquo;-&rdquo;为主让客。<br />
                    3.单关与过关让分胜负、预设总分不同，单关让分胜负、预设总分在接受投注后不再变化。过关投注（固定奖金），预设让分数和预设总分数可能会根据销售过程中投注
                    额等相关因素有所变动。之前的投注不受之后预设让分数 和预设总分数变动的影响。<br />
                    4.过关投注固定奖金仅供参考，实际奖金以出票时刻奖金为准。2或3场过关投注，单注最高奖金限额20万元，4或5场过关投注，单注最高奖金限额50万元，6场过关投注，
                    单注最高奖金限额100万元。<br />
                    5.单注彩票保底奖金：如中奖者的单注奖金不足2元，将补足至2元。<br />
                    <span class="red">6.特别提示：表中显示中奖金额=每1元对应中奖奖金。 </span>
                    <br />
                </div>
</div>
                <!-- start 合买区  !-->

<!-- Right ! -->
<div style="width:240px; float:right;">
                <table class="bd_touzhu blank" width="240px" border="0" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td valign="top">
                                <table class="table_jczqtz_gg_lq" width="240px" border="0" cellpadding="4" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="jczqtouzu_title_gg_lq" width="200px">
                                                <span class="bold">确认投注信息</span>
                                            </td>
                                            <td style="background-color:rgb(248, 217, 182); width:40px; text-align:right;"><span style="cursor: pointer; background-color:rgb(248, 217, 182); font-size:16px; font-weight:bold; color:Red;" id="clearAllSelect">×&nbsp;</span></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table class="table_bdtz_gg_lq" width="100%" border="0" cellpadding="4" cellspacing="0">
                                                    <tbody>
                                                        <tr class="t02">
                                                            <td align="center">
                                                                编号
                                                            </td>
                                                            <td align="center">
                                                                主队
                                                            </td>
                                                            <td align="center" class="bnone">
                                                                选项
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                    <tbody id="row_tpl" style="display:none;"><tr class="t03"><td width="75px" align="center"><input checked="checked" value="2929" name="m2929" type="checkbox" /><span>周四006</span></td><td align="center">巴兰基利亚青年 VS 蒙特维迪奥竞技</td><td align="left" class="bnone" width="71px">&nbsp;<label style="display: none;"><input checked="checked" value="1" type="checkbox" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'">主负</em></label><label style="display: none;"><input checked="checked" value="2" type="checkbox" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'">主胜</em></label></td></tr></tbody>
                                                    <tbody id="selectTeams">
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            </tr>
                            <tr>
                            <td valign="top">
							<table class="table_jczqtz_gg_lq" style="" width="100%" border="0" cellpadding="4" cellspacing="0">
								<tbody>
									<tr>
										<td class="jczqtouzu_title_gg_lq">确认投注信息及购买</td>
									</tr>
									<tr>
										<td>
											<table class="table_bdtz_gg_lq" width="100%" border="0" cellpadding="4" cellspacing="0" style="margin:0">
												<tbody>													
													<tr class="t04">
														<td>1、确认投注结果</td>
													</tr>
													<tr>
														<td>
															<table class="table_BetInfo" width="100%" border="0" cellpadding="2" cellspacing="0">
			                                                            <tr>
				                                                            <td width="66px;" align="right">
					                                                            金额：</td>
				                                                            <td>
					                                                            注数<span style="font-size:12px;color:Red;" id="showCount">0</span>注，<span class="r14px" id="showMoney">￥0.00</span>元</td>
			                                                            </tr>
			                                                            <tr>
				                                                            <td align="right">
					                                                            倍数：<span id="selectMatchNum" style="color:#F00; display:none;">0</span></td>
				                                                            <td align="left">
					                                                            <input class="jczqtz_input" id="buybs" value="1" size="7" maxlength="5" style="width:50px"/>
					                                                            (最高99999倍)
				                                                            </td>
			                                                            </tr>
			                                                            <tr>
				                                                            <td align="right">
					                                                            类型：</td>
				                                                            <td align="left">
					                                                            <input name="Scheme_Type" checked="checked" type="radio" id="Scheme_Buy" />代购<input
						                                                            name="Scheme_Type" type="radio" id="Scheme_join" />合买
				                                                            </td>
			                                                            </tr>
			                                                            <tbody id="trShowJion" style="height: 36px; text-align: center; padding-left: 10px; padding-right: 10px; display:none;">
				                                                            <tr>
					                                                            <td align="right">
						                                                            佣金比率：</td>
					                                                            <td align="left">
						                                                            <input type="text" id="tb_SchemeBonusScale" name="tb_SchemeBonusScale" value="4"
							                                                            maxlength="10" style="width:50px"/>% 只能为0~10。
					                                                            </td>
				                                                            </tr>
				                                                            <tr id="trShare">
					                                                            <td align="right">
						                                                            我要分成：</td>
					                                                            <td align="left">
						                                                            <input type="text" id="tb_Share" name="tb_Share" maxlength="10" value="1"  style="width:50px"/>份<div></div>每份
						                                                            <span id="lab_ShareMoney" style="color: Red">0.00</span>&nbsp;元。
					                                                            </td>
				                                                            </tr>
				                                                            <tr>
					                                                            <td align="right">
						                                                            我要认购：</td>
					                                                            <td align="left">
						                                                            <input type="text" id="tb_BuyShare" name="tb_BuyShare" value="1" style="width:50px"/>份<div></div>金额 <span id="lab_BuyMoney"
								                                                            style="color: Red">0.00</span>&nbsp;元。
					                                                            </td>
				                                                            </tr>
				                                                            <tr>
					                                                            <td align="right">
						                                                            我要保底：</td>
					                                                            <td align="left">
						                                                            <input type="text" id="tb_AssureShare" name="tb_AssureShare" value="0" style="width:50px"/>份<div></div>保底 <span id="lab_AssureMoney"
								                                                            style="color: Red">0.00</span>&nbsp;元。
					                                                            </td>
				                                                            </tr>
				                                                            <tr>
					                                                            <td align="right">
						                                                            方案标题：</td>
					                                                            <td align="left">
						                                                            <input type="text" id="tb_Title" name="tb_Title" maxlength="50" style="width:120px"/><div></div>
						                                                            <font color="#ff0000">注：</font>长度不能超过<font color="#ff0000">50</font>个字符。
					                                                            </td>
				                                                            </tr>
			                                                            </tbody>
			                                                            <tr>
				                                                            <td align="right">
					                                                            保密：</td>
				                                                            <td align="left" id="ggSecrecyLevel">
					                                                            <input type="radio" name="SecrecyLevel" id="SecrecyLevel0" value="0" checked="checked" /><span>不保密</span>
					                                                            <input type="radio" name="SecrecyLevel" id="SecrecyLevel1" value="1" /><span>保密到截止</span>
					                                                            <div></div>
					                                                            <input type="radio" name="SecrecyLevel" id="SecrecyLevel2" value="2" /><span>保密到开奖</span>
					                                                            <input type="radio" name="SecrecyLevel" id="SecrecyLevel3" value="3" /><span>永久保密</span>
				                                                            </td>
			                                                            </tr>
			                                                            <tr>
				                                                            <td class="td_pad" colspan="2">
					                                                            <div style="display:none">方案注数<span class="r14px" id="showCount">0</span>注，倍数<span class="r14px" id="showBS">1</span>倍
					                                                            总金额<span class="r14px" id="showMoney">￥0.00</span>元<br /></div>
					                                                            				                                                            
					                                                            <div class="tz_r_btn">
						                                                            <span><a href="#" class="btn_tzbuy" id="dgBtn" alt="确认代购">确认代购</a></span>
					                                                            </div>
				                                                            </td>
			                                                            </tr>
		                                                            </table>
                                                        </td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
				</tbody>
			</table>
			<!-- end合买区  !-->
</div>
			<!--投注结束 -->
            </div>
    <uc2:WebFoot ID="WebFoot1" runat="server" />
        </div>
        <input type="hidden" id="jsonggtype" value="{'单关':'A0','2串1':'AA','3串1':'AB','3串3':'AC','3串4':'AD','4串1':'AE','4串4':'AF','4串5':'AG','4串6':'AH','4串11':'AI','5串1':'AJ','5串5':'AK','5串6':'AL','5串10':'AM','5串16':'AN','5串20':'AO','5串26':'AP','6串1':'AQ','6串6':'AR','6串7':'AS','6串15':'AT','6串20':'AU','6串22':'AV','6串35':'AW','6串42':'AX','6串50':'AY','6串57':'AZ', '7串1':'BA', '7串7':'BB', '7串8':'BC', '7串21':'BD', '7串35':'BE', '7串120':'BF', '8串1':'BG', '8串8':'BH', '8串9':'BI', '8串28':'BJ', '8串56':'BK', '8串70':'BL', '8串247':'BM', '8串255':'BN'}" />
        <input type="hidden" id="ggtypename" name="sgtypename" value="普通过关" />
        <input type="hidden" id="ggtypeid" name="ggtypeid" value="A0" />
        <input type="hidden" id="codes" name="codes" />
        <input type="hidden" id="totalmoney" name="totalmoney" />
        <input type="hidden" id="zhushu" name="zhushu" />
        <input type="hidden" id="beishu" name="beishu" />
        <input type="hidden" id="playid" name="playid" value="7301" />
        <input type="hidden" id="playname" name="playname" value="胜负" />
        <input type="hidden" id="lotid" name="lotid" value="73" />
        <input type="hidden" id="SecrecyL" name="SecrecyL" value="0" />
        <input type="hidden" id="AssureMoney" name="AssureMoney" value="0" />
        <input type="hidden" id="SchemeSchemeBonusScalec" name="SchemeSchemeBonusScalec" runat="server"/>
        <asp:HiddenField runat="server" ID="HidNowTime"></asp:HiddenField>
    </div>
    </form>
</body>

<script type="text/javascript" src="JScript/tool.js"></script>
<script type="text/javascript" src="JScript/com.js"></script>
<script type="text/javascript" src="JScript/dg.js"></script>
<script id="config" type="text/javascript" src="JScript/form.js?1"></script>
<script type="text/javascript">
function stimeshow() {
    var nowTime = new Date(Date(document.getElementById("<%=HidNowTime.ClientID %>").value));

    var y = nowTime.getFullYear();
    var M = nowTime.getMonth() + 1;
    var d = nowTime.getDate();
    var h = nowTime.getHours();
    var m = nowTime.getMinutes();
    var s = nowTime.getSeconds();

    document.getElementById("TimeDisplay").innerHTML = M + "月" + d + "日 " + (h > 9 ? h : "0" + String(h)) + ":" + (m > 9 ? m : "0" + String(m)) + ":" + (s > 9 ? s : "0" + String(s));

    //时间差
    var TimePoor = nowTime.getTime() - new Date().getTime();
    document.getElementById("<%=HidNowTime.ClientID %>").value = new Date(new Date().getTime() + TimePoor + 1000);
}
setInterval(stimeshow,1000);
</script>

</html>
