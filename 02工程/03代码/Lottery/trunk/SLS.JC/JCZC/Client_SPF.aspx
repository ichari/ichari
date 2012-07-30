<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Client_SPF.aspx.cs" Inherits="JCZC_Client_SPF" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>让球胜平负-竞彩足球</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/tips_globle.css" rel="stylesheet" type="text/css" />
    <link href="Style/public.css" rel="stylesheet" type="text/css" />
    <link href="Style/trade.css" rel="stylesheet" type="text/css" />
    <link href="/Style/pub.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="buy_form" runat="server" ajax="Buy_Handler.ashx">
    <div style="width:814px;">
        <div style="width:814px;">
            <!-- 开始选择 !-->
            <div style="border-top-color: #FFF;">
                <div style="display:none">
                    <p class="time_scroll">
                        <span class="time_scroll2" id="TimeDisplay"></span>
                    </p>
                    <img src="Images/btn_sai.gif" style="cursor: pointer;"
                        onclick="document.getElementById('leagueBox').style.display='block'" align="absmiddle"
                        alt="">
                    <span class="red" id="hideCount">0</span> <a href="javascript:void 0" id="showAllTeam">
                        <font color="#ff0000"></font></a> 
                      <input name="rq" checked="checked" id="showRq" type="checkbox" />
                      <asp:Label ID="lbShowRq" runat="server"></asp:Label>
                      <input name="norq" checked="checked" id="showNoRq" type="checkbox" />
                      <asp:Label ID="lbShowRoRaq" runat="server"></asp:Label>
                </div>
                <div class="searchbg" style="display:none">
                </div>
                <div class="ss_slect" style="display:none">
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
                                </td>
                            </tr>
                            <tr>
                                <td align="center" bgcolor="#f1f8fe" height="25">
                                    <img src="Images/select_all.gif" alt="全选" style="cursor: pointer;"
                                        id="selectAllBtn" /><img src="Images/select_alt.gif"
                                            alt="反选" style="cursor: pointer;" id="selectOppBtn" /><img src="Images/close2.gif"
                                                alt="关闭" style="cursor: pointer;" onclick="document.getElementById('leagueBox').style.display='none'" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!-- 日期选择开始 -->
                <div style="position: absolute; left: 530px; top: 230px; display:none;">
                </div>
                  <!-- 开始对阵列表 !-->
                  <div id="scroll" style="display: none; background-color:White;">
                  <table class="ttd showodds" width="100%" border="0" cellpadding="0" cellspacing="1">
                      <tbody>
                          <tr class="gg_form_tr_01">
                              <td rowspan="2" style="width:100px;">赛事编号</td>
                              <td rowspan="2" width="62">赛事<br />类型 </td>
                              <td rowspan="2" style="width:104px;">截止<br />时间</td>
                              <td rowspan="2">主队</td>
                              <td rowspan="2" style="width:40px;">让球</td>
                              <td rowspan="2" style="width:77px;">客队</td>
                               <td colspan="3">
                                <select id="Select1">
                                  <option value="99家平均">99家平均欧指</option>
                                  <option value="威廉希尔">威廉希尔</option>
                                  <option value="立博">立博</option>
                                  <option value="Bet365">Bet365</option>
                                  <option value="澳门彩票">澳门彩票</option>
                                </select></td>
                              <td rowspan="2" style="width:80px;">数据</td>
                              <td colspan="4">请选择投注</td>
                          </tr>
                          <tr class="gg_form_tr_02">
                              <td height="18" style="width:56px;">主胜</td>
                              <td style="width:56px;">平局</td>
                              <td style="width:56px;">主负</td>
                              <td>胜(3)</td>
                              <td>平(1)</td>
                              <td>负(0)</td>
                              <td>全包</td>
                          </tr>
                      </tbody>
                  </table>
                  </div>
                  <div id="tz">
                   <table class="ttd showodds" id="table_vs" width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tbody><tr class="gg_form_tr_01">
                  <td rowspan="2">赛事编号</td>
                  <td rowspan="2" width="62">赛事<br />类型 </td>
                  <td rowspan="2">截止<br />时间</td>
                  <td rowspan="2">主队</td>
                  <td rowspan="2">让球</td>
                  <td rowspan="2">客队</td>
                   <td colspan="3">
                    <select id="op_col">
                      <option value="99家平均">99家平均欧指</option>
                      <option value="威廉希尔">威廉希尔</option>
                      <option value="立博">立博</option>
                      <option value="Bet365">Bet365</option>
                      <option value="澳门彩票">澳门彩票</option>
                    </select></td>
                  <td rowspan="2">数据</td>
                  <td colspan="4">请选择投注</td>
                  </tr>
                  <tr class="gg_form_tr_02">
                  <td height="18">主胜</td>
                  <td>平局</td>
                  <td>主负</td>
                  <td>胜(3)</td>
                  <td>平(1)</td>
                  <td>负(0)</td>
                  <td>全包</td>
                  </tr>
                 <%= MatchList %>
                 </tbody>
            </table>
            </div>
            <!-- start 合买区  -->
            <table class="bd_touzhu blank" width="99%" border="0" cellpadding="0" cellspacing="0">
  <tbody><tr>
  <td class="l" valign="top" width="58%">
  <table class="table_jczqtz_gg" width="98%" border="0" cellpadding="4" cellspacing="0">
  <tbody><tr>
  <td class="jczqtouzu_title_gg"><span class="bold">您选择的场次及投注选项</span></td>
  </tr>
  <tr>
  <td><table class="table_bdtz_gg" width="100%" border="0" cellpadding="4" cellspacing="0">
    <tbody><tr class="t02">
      <td width="100" align="center">赛事编号</td>
      <td align="center">比赛对阵</td>
      <td align="center" class="bnone">您的投注选项</td>
      <td align="center" class="bnone" style="display:none">设胆</td>
    </tr>
      </tbody>
      <tbody id="row_tpl" style="display:none;">
      <tr class="t03"><td><input checked="checked" value="2929" name="m2929" type="checkbox"/><span>周四006</span></td><td align="center">巴兰基利亚青年 VS 蒙特维迪奥竞技</td><td align="center" class="bnone"><label style="display: none;"><input checked="checked" value="3" type="checkbox"/>胜</label><label style="display: none;"><input checked="checked" value="1" type="checkbox"/>平</label><label style="display: none;"><input checked="checked" value="0" type="checkbox"/>负</label>
       </td>
       <td  style="display:none"><input type="checkbox" name="danma2929" value="2929" onclick="return damaclick(this,2929);" /></td>
       </tr>
      </tbody>
      <tbody id="selectTeams"></tbody>
       <tbody><tr>
        <td colspan="4" style="text-align:right; padding-right:10px;">
            <span id="spnUnsureDan" style="display:none;">
		        <b>模糊设胆：</b><select name="dpUnsureDan" id="dpUnsureDan" ></select>
		        <a href="/beijingdanchang/info_t2i8704.html" class="spnLnk" target="_blank">使用说明</a>
		    </span>
		</td>
      </tr></tbody>
  </table></td>
  </tr>
  <tr>
  <td align="center" height="50">
      <span style="line-height: 22px; cursor: pointer;" id="clearAllSelect"><img src="Images/b_img_014.gif" title="清空投注"></span></td>
  </tr>
  </tbody></table>
  </td>
  <td class="r" valign="top"><table class="table_jczqtz_gg" style="" width="100%" border="0" cellpadding="4" cellspacing="0">
     <tbody><tr>
  <td colspan="5" class="jczqtouzu_title_gg">请选择过关方式</td>
  </tr>
      <tr>
        <td><table class="table_bdtz_gg" width="100%" border="0" cellpadding="4" cellspacing="0" style="margin:0;">
          <tbody>
          <tr class="t04" style="display:none">
            <td>1、选择过关类型</td>
            </tr>
          <tr class="t05" style="display:none">
            <td><input name="ssq_dg" checked="checked" type="radio">普通过关<div style="display: none;"> <input name="ssq_dg" disabled="disabled" type="radio">组合过关<input name="ssq_dg" disabled="disabled" type="radio">自由过关 </div></td>
            </tr>
           <tr class="t04" style="display:none">
            <td>2、选择过关方式</td>
            </tr>
            <tr class="t05">
            <td id="ggList" align="left" bgcolor="#ffffff">
            <div class="betr">
                    <label style="display:none;" for="r2c1"><input name="ggtype_radio" id="r2c1" value="2串1" type="checkbox" />2串1</label>
                    <label style="display:none;" for="r3c1"><input name="ggtype_radio" id="r3c1" value="3串1" type="checkbox" />3串1</label>
                    <label style="display:none;" for="r4c1"><input name="ggtype_radio" id="r4c1" value="4串1" type="checkbox" />4串1</label>
                    <label style="display:none;" for="r5c1"><input name="ggtype_radio" id="r5c1" value="5串1" type="checkbox" />5串1</label>
                    <label style="display:none;" for="r6c1"><input name="ggtype_radio" id="r6c1" value="6串1" type="checkbox" />6串1</label>
                    <label style="display:none;" for="r7c1"><input name="ggtype_radio" id="r7c1" value="7串1" type="checkbox" />7串1</label>
                    <label style="display:none;" for="r8c1"><input name="ggtype_radio" id="r8c1" value="8串1" type="checkbox" />8串1</label>
                    <div></div>
                    <label style="display:none;" for="r3c3"><input name="ggtype_radio" id="r3c3" value="3串3" type="checkbox" />3串3</label>
			        <label style="display:none;" for="r3c4"><input name="ggtype_radio" id="r3c4" value="3串4" type="checkbox" />3串4</label>
                    <label style="display:none;" for="r4c4"><input name="ggtype_radio" id="r4c4" value="4串4" type="checkbox" />4串4</label>
			        <label style="display:none;" for="r4c5"><input name="ggtype_radio" id="r4c5" value="4串5" type="checkbox" />4串5</label>
			        <label style="display:none;" for="r4c6"><input name="ggtype_radio" id="r4c6" value="4串6" type="checkbox" />4串6</label>
			        <label style="display:none;" for="r4c11"><input name="ggtype_radio" id="r4c11" value="4串11" type="checkbox" />4串11</label>
			        <div></div> 
			        <label style="display:none;" for="r5c5"><input name="ggtype_radio" id="r5c5" value="5串5" type="checkbox" />5串5</label>
			        <label style="display:none;" for="r5c6"><input name="ggtype_radio" id="r5c6" value="5串6" type="checkbox" />5串6</label>
			        <label style="display:none;" for="r5c10"><input name="ggtype_radio" id="r5c10" value="5串10" type="checkbox" />5串10</label>
			        <label style="display:none;" for="r5c16"><input name="ggtype_radio" id="r5c16" value="5串16" type="checkbox" />5串16</label>
			        <label style="display:none;" for="r5c20"><input name="ggtype_radio" id="r5c20" value="5串20" type="checkbox" />5串20</label>
			        <label style="display:none;" for="r5c26"><input name="ggtype_radio" id="r5c26" value="5串26" type="checkbox" />5串26</label>
			        <div></div>
			        <label style="display:none;" for="r6c6"><input name="ggtype_radio" id="r6c6" value="6串6" type="checkbox" />6串6</label>
			        <label style="display:none;" for="r6c7"><input name="ggtype_radio" id="r6c7" value="6串7" type="checkbox" />6串7</label>
			        <label style="display:none;" for="r6c15"><input name="ggtype_radio" id="r6c15" value="6串15" type="checkbox" />6串15</label>
			        <label style="display:none;" for="r6c20"><input name="ggtype_radio" id="r6c20" value="6串20" type="checkbox" />6串20</label>
			        <label style="display:none;" for="r6c22"><input name="ggtype_radio" id="r6c22" value="6串22" type="checkbox" />6串22</label>
			        <label style="display:none;" for="r6c35"><input name="ggtype_radio" id="r6c35" value="6串35" type="checkbox" />6串35</label>
			        <div></div>
			        <label style="display:none;" for="r6c42"><input name="ggtype_radio" id="r6c42" value="6串42" type="checkbox" />6串42</label>
			        <label style="display:none;" for="r6c50"><input name="ggtype_radio" id="r6c50" value="6串50" type="checkbox" />6串50</label>
			        <label style="display:none;" for="r6c57"><input name="ggtype_radio" id="r6c57" value="6串57" type="checkbox" />6串57</label>
			        <div></div>
			        <label style="display:none;" for="r7c7"><input name="ggtype_radio" id="r7c7" value="7串7" type="checkbox" />7串7</label>
			        <label style="display:none;" for="r7c8"><input name="ggtype_radio" id="r7c8" value="7串8" type="checkbox" />7串8</label>
			        <label style="display:none;" for="r7c21"><input name="ggtype_radio" id="r7c21" value="7串21" type="checkbox" />7串21</label>
			        <label style="display:none;" for="r7c35"><input name="ggtype_radio" id="r7c35" value="7串35" type="checkbox" />7串35</label>
			        <label style="display:none;" for="r7c120"><input name="ggtype_radio" id="r7c120" value="7串120" type="checkbox" />7串120</label>
			        <div></div>
			        <label style="display:none;" for="r8c8"><input name="ggtype_radio" id="r8c8" value="8串8" type="checkbox" />8串8</label>
			        <label style="display:none;" for="r8c9"><input name="ggtype_radio" id="r8c9" value="8串9" type="checkbox" />8串9</label>
			        <label style="display:none;" for="r8c28"><input name="ggtype_radio" id="r8c28" value="8串28" type="checkbox" />8串28</label>
			        <label style="display:none;" for="r8c56"><input name="ggtype_radio" id="r8c56" value="8串56" type="checkbox" />8串56</label>
			        <label style="display:none;" for="r8c70"><input name="ggtype_radio" id="r8c70" value="8串70" type="checkbox" />8串70</label>
			        <label style="display:none;" for="r8c247"><input name="ggtype_radio" id="r8c247" value="8串247" type="checkbox" />8串247</label>
            <span class="clear"></span></div>
              </td>
            </tr>
            <tr class="t04" style="display:none">
                <td>
                    3、确认投注信息
                </td>
            </tr>
            <tr style="display:none">
                <td>
                    <table class="table_BetInfo" width="100%" border="0" cellpadding="2" cellspacing="0">
                        <tr>
                            <td width="80" align="right">
                                方案金额：
                            </td>
                            <td>
                                方案注数<span class="r14px" id="showCount">0</span>注，<span
                                    class="r14px" id="showMoney">￥0.00</span>元
                            </td>
                        </tr>
                        <tr>
                            <td width="80" align="right">
                                投注倍数：
                            </td>
                            <td align="left">
                                <input class="jczqtz_input" id="buybs" value="1" size="7" maxlength="5" />
                                (最高99999倍)
                            </td>
                        </tr>
                        <tr>
                            <td width="80" align="right">
                                购彩类型：
                            </td>
                            <td align="left">
                                <input name="Scheme_Type" checked="checked" type="radio" id="Scheme_Buy" />代购<input
                                    name="Scheme_Type" type="radio" id="Scheme_join" />合买
                            </td>
                        </tr>
                        <tbody id="trShowJion" style="height: 36px; text-align: center; padding-left: 10px; padding-right: 10px; display:none;">
                            <tr>
                                <td>
                                    佣金比率
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_SchemeBonusScale" name="tb_SchemeBonusScale" style="width: 56px;" value="4"
                                        maxlength="10" />% &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;佣金比例只能为0~10。
                                </td>
                            </tr>
                            <tr id="trShare">
                                <td>
                                    我要分成
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_Share" name="tb_Share" style="width: 56px;" maxlength="10" value="1" />份，每份
                                    <span id="lab_ShareMoney" style="color: Red">0.00</span>&nbsp;元。&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    我要认购
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_BuyShare" name="tb_BuyShare" style="width: 56px;" value="1" />份，金额 <span id="lab_BuyMoney"
                                            style="color: Red">0.00</span>&nbsp;元。
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    我要保底
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_AssureShare" name="tb_AssureShare" style="width: 56px;" value="0" />份，保底 <span id="lab_AssureMoney"
                                            style="color: Red">0.00</span>&nbsp;元。&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    方案标题
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_Title" name="tb_Title" style="width: 99%;" maxlength="50" /><font
                                        color="#ff0000">【注】</font>长度不能超过 <font color="#ff0000">50</font> 个字符。
                                </td>
                            </tr>
                        </tbody>
                        <tr>
                            <td width="80" align="right">
                                保密设置：
                            </td>
                            <td align="left" id="ggSecrecyLevel">
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel0" value="0" checked="checked" /><span>不保密</span>
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel1" value="1" /><span>保密到截止</span>
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel2" value="2" /><span>保密到开奖</span>
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel3" value="3" /><span>永久保密</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="touz_btn"><img src="Images/btn_dg.gif" id="dgBtn" border="0" style="cursor: pointer;" alt="确认代购">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src="Images/FilterShrink.gif" id="FilterBtn" border="0" style="cursor: pointer;" alt="高级过滤" /></div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
  </td>
      </tr>
    </tbody></table></td>
  </tr>
  </tbody>
  </table>
            <!--投注结束 -->
        </div>
        
        </div>
        
  <input type="hidden" id="jsonggtype" value="{'单关':'A0','2串1':'AA','3串1':'AB','3串3':'AC','3串4':'AD','4串1':'AE','4串4':'AF','4串5':'AG','4串6':'AH','4串11':'AI','5串1':'AJ','5串5':'AK','5串6':'AL','5串10':'AM','5串16':'AN','5串20':'AO','5串26':'AP','6串1':'AQ','6串6':'AR','6串7':'AS','6串15':'AT','6串20':'AU','6串22':'AV','6串35':'AW','6串42':'AX','6串50':'AY','6串57':'AZ','7串1':'BA','7串7':'BB','7串8':'BC','7串21':'BD','7串35':'BE','7串120':'BF','8串1':'BG','8串8':'BH','8串9':'BI','8串28':'BJ','8串56':'BK','8串70':'BL','8串247':'BM'}" />
<input type="hidden" id="ggtypename" name="sgtypename" value="普通过关" />
<input type="hidden" id="ggtypeid" name="ggtypeid" value="AA" />
<input type="hidden" id="codes" name="codes" />
<input type="hidden" id="totalmoney" name="totalmoney" />
<input type="hidden" id="zhushu" name="zhushu" />
<input type="hidden" id="beishu" name="beishu" />
<input type="hidden" id="playid" name="playid" value="7201" />
<input type="hidden" id="playname" name="playname" value="竞彩胜平负" />
<input type="hidden" id="lotid" name="lotid" value="72" />
<input type="hidden" id="SecrecyL" name="SecrecyL" value="0" />
<input type="hidden" id="lotType" name="lotType" value="spf" />
<div class="layer" id="popWindow" style="position:absolute; z-index:9999; display:none; top:400px; left:25%;">
  <div class="layer2">
    <div class="lay_box4">
      <div class="title_t title_t4 clear"><h4><span id="popWindowTitle">竞彩足球奖金明细</span></h4><a href="javascript:void(0)" class="del" title="关闭" onclick="document.getElementById('popWindow').style.display='none'" alt="关闭">关闭</a></div>
      <div class="lay_content2 scroll" id="popContent">
      <table border="0" cellspacing="0" cellpadding="0" class="yuce_tablebox blank">
         <tr class="trt">
            <th>赛事编号</th>
            <th class="tb_line">比赛</th>
            <th class="tb_line">您的选择(奖金)</th>
            <th class="tb_line">最小奖金</th>
            <th class="tb_line">最大奖金</th>
          </tr>
          <tr>
            <td>周四026</td>
            <td>德甲</td>
            <td>胜(1.76) 平(3.07) 负(9.28)</td>
            <td class="vcenter">14.21</td>
            <td class="td_line vcenter">112.00</td>
          </tr>
          <tr>
            <td>周四026</td>
            <td>德甲</td>
            <td>胜(1.76) 平(3.07) 负(9.28)</td>
            <td class="vcenter">4.21</td>
            <td class="td_line vcenter">12.00</td>
          </tr>
          <tr>
            <td>周四026</td>
            <td>德甲</td>
            <td>胜(1.76) 平(3.07) 负(9.28)</td>
            <td class="vcenter">4.21</td>
            <td class="td_line vcenter">12.00</td>
          </tr>
          <tr>
            <td>周四026</td>
            <td>德甲</td>
            <td>胜(1.76) 平(3.07) 负(9.28)</td>
            <td class="vcenter">4.21</td>
            <td class="td_line vcenter">12.00</td>
          </tr>
          <tr>
            <td>周四026</td>
            <td>德甲</td>
            <td>胜(1.76) 平(3.07) 负(9.28)</td>
            <td class="vcenter">4.21</td>
            <td class="td_line vcenter">12.00</td>
          </tr>
          <tr>
            <td colspan="5" class="td_line">过关方式：<span class="red">9串1</span> 倍数：<span class="red">1</span>倍 方案总金额：<span
class="red">￥2.00</span></td>
          </tr>
        </table>
        <table border="0" cellspacing="0" cellpadding="0" class="yuce_tablebox">
          <tr class="trt">
            <th rowspan="2">命中场数</th>
            <th class="tb_line tb_line2">中奖注数</th>
            <th rowspan="2" class="tb_line">倍数</th>
            <th colspan="2" class="tb_line tb_line2">奖金范围</th>
          </tr>
          <tr class="trt">
            <th class="tb_line">5串1</th>
            <th class="tb_line">最小奖金</th>
            <th class="tb_line">最大奖金</th>
          </tr>
          <tr>
            <td>周四026</td>
            <td>1注</td>
            <td>1234</td>
            <td>4.21元[明细]</td>
            <td class="td_line vcenter">4.21元[明细]</td>
          </tr>
          <tr>
            <td colspan="5">注：奖金预测sp值为投注时即时sp值，最终奖金以开奖sp值为准<span class="red"></span></td>
          </tr>
        </table>
<table border="0" cellspacing="0" cellpadding="0" class="yuce_tablebox">
          <tr class="trt">
            <th>过关方式</th>
            <th class="tb_line">中奖注数</th>
            <th class="tb_line">中12场 最大奖金 中奖明细</th>
            <th class="tb_line">奖金</th>
          </tr>
          <tr>
            <td>9串1</td>
            <td>9565注</td>
            <td><p class="td_width">[50]9.38×[46]4.27×[53]4.18×[51]3.29
×[43]3.16×[54]3.09×1倍×2元×65%
= 6991.85元</p></td>
            <td class="td_line vcenter">13243.11元</td>
          </tr>
          <tr>
            <td>合计</td>
            <td>23注</td>
            <td>&nbsp;</td>
            <td class="td_line vcenter"><span class="bold">131.00</span>元</td>
          </tr>
        </table>
      </div>
    </div>
  </div>
</div>
</div>
<asp:HiddenField runat="server" ID="HidNowTime"></asp:HiddenField>
</form>
</body>

<script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<script type="text/javascript" src="JScript/tool.js"></script>
<script type="text/javascript" src="JScript/com.js"></script>
<script type="text/javascript" src="JScriptClient/gg.js"></script>
<script id="config" type="text/javascript" src="JScript/form.js?1"></script>

</html>
