﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Buy_ZJQS.aspx.cs" Inherits="JCZC_Buy_ZJQS" EnableViewState="false"%>
<%@ Register src="../Home/Room/UserControls/WebHead.ascx" tagname="WebHead" tagprefix="uc1" %>
<%@ Register src="../Home/Room/UserControls/WebFoot.ascx" tagname="WebFoot" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>总进球数-竞彩足球</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/tips_globle.css" rel="stylesheet" type="text/css" />
    <link href="Style/public.css" rel="stylesheet" type="text/css" />
    <link href="Style/trade.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="buy_form" runat="server" ajax="Buy_Handler.ashx">
    <uc1:WebHead ID="WebHead1" runat="server" />
    <div id="gc_bg">
        <div id="position">
            <span class="fa">当前位置：<a href="/" target="_parent">首页</a> > <a href="/jczc/buy_spf.aspx" target="_parent">足球投注</a> > </span>
            <span class="sub_t">总进球数(最多选择6场赛事)</span></div>
        <div id="wrap">
            <div class="nav_button">
                <ul>
                    <li class="butt2"><a href="Buy_SPF.aspx">
                        让球胜平负</a></li>
                    <li class="butt1"><a href="Buy_ZJQS.aspx">
                        总进球数</a></li>
                    <li class="butt2"><a href="Buy_ZQBF.aspx">
                        比分</a></li>
                    <li class="butt2"><a href="Buy_BQC.aspx">
                        半全场</a></li>
                    <li style="float:right; width:280px; text-align:right;">
                        <a href="/ZX.aspx?NewsName=jczq" target="_blank">竞彩资讯</a>&nbsp;&nbsp;|
                        <a href="/Join/Project_List.aspx?id=72">参与合买</a>&nbsp;&nbsp;|
                        <a href="/Open/index.aspx"  target="_blank">开奖公告</a>&nbsp;&nbsp;|
                        <a href="/Help/play_7203.aspx" target="_blank">玩法介绍</a>                        
                    </li>
               </ul>
            </div>
             <div class="choice">
    <div class="fsds">
      <div class="l">
      <input type="radio"  onclick="window.location.href='Buy_ZJQS_DG.aspx'">
      <label for="dgjc">单关投注</label>
      <input type="radio" checked="checked">
      <label for="ggjc" >过关投注</label>
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
                    <img src="Images/btn_sai.gif" style="cursor: pointer;"
                        onclick="document.getElementById('leagueBox').style.display='block'" align="absmiddle"
                        alt="赛事选择">
                    已隐藏<span class="red" id="hideCount">0</span>场比赛 <a href="javascript:void 0" id="showAllTeam">
                        <font color="#ff0000">恢复</font></a> <span style="display: none"><a href="javascript:void(0)"
                            id="lookHistory" class="alink">查看历史赛果</a></span>
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
                                    <img src="Images/select_all.gif" alt="全选" style="cursor: pointer;"
                                        id="selectAllBtn" /><img src="Images/select_alt.gif"
                                            alt="反选" style="cursor: pointer;" id="selectOppBtn" /><img src="Images/close2.gif"
                                                alt="关闭" style="cursor: pointer;" onclick="document.getElementById('leagueBox').style.display='none'" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                </div>
                <!-- 日期选择开始 -->
                <!-- Left -->
<div class="touzhu" style="width:750px; float:left;border-top-color: #FFF; margin-left:0px;">
                <div style="position: absolute; left: 530px; top: 230px;">
                </div>
                  <!-- 开始对阵列表 !-->
                   <table class="ttd showodds" id="table_vs" width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tbody><tr class="gg_form_tr_01">
                  <td rowspan="2">赛事编号</td>
                  <td rowspan="2" width="55">赛事<br />类型 </td>
                  <td rowspan="2">截止<br />时间</td>
                  <td rowspan="2">主队</td>  
                  <td rowspan="2">客队</td>
                  <td rowspan="2">数据</td>
                  <td height="20" colspan="10">请选择投注</td>
                  </tr>
                  <tr class="gg_form_tr_02">
                  <td width="52" height="20"><strong>0</strong></td>
                  <td width="52">1</td>
                  <td width="52">2</td>    
                  <td width="52">3</td>
                  <td width="52">4</td>
                  <td width="52">5</td>
                  <td width="52">6</td>
                  <td width="52">7+</td>
                  <td>全包</td>
                  </tr>
                   <%= MatchList %>
                </tbody>
            </table>
            
            <div class="sg_text01"><strong>投注提示</strong>：<br />
              <span  class="red">1、实际奖金指数以最终出票时为准。</span><br />
              <span>2、单个方案最多只能选择6场赛事进行投注。</span><br />
              <span>3、表中所有固定奖金额仅供参考，2或3场过关投注，单注最高奖金限额20万元，4或5场过关投注，单注最高奖金限额50万元，6场过关投注，单注最高奖金限额100万元。</span><br />
              <span class="red">4、特别提示：表中显示中奖金额=每1元对应中奖奖金。</span><br />
          </div>
</div>
            <!-- start 合买区  !-->

<!-- Right ! -->
<div style="width:240px; float:right;">
            <table class="bd_touzhu blank" width="240px" border="0" cellpadding="0" cellspacing="0">
  <tbody><tr>
  <td valign="top">
  <table class="table_jczqtz_gg" width="240px" border="0" cellpadding="4" cellspacing="0">
  <tbody><tr>
  <td class="jczqtouzu_title_gg" width="200px"><span class="bold">确认投注信息</span></td>
  <td style="background-color:rgb(121, 178, 241); width:40px; text-align:right;"><span style="cursor: pointer; background-color:rgb(121, 178, 241); font-size:16px; font-weight:bold; color:Red;" id="clearAllSelect">×&nbsp;</span></td>
  </tr>
  <tr>
  <td colspan="2"><table class="table_bdtz_gg" width="100%" border="0" cellpadding="4" cellspacing="0">
    <tbody><tr class="t02">
      <td align="center">编号</td>
      <td align="center">主队</td>
      <td align="center" class="bnone">选项</td>
      <td align="center" class="bnone">胆</td>
    </tr>
      </tbody>
      <tbody id="row_tpl" style="display:none">
      <tr class="t03"><td style="width:75px;" align="center"><input checked="checked" value="2929" name="m2929" type="checkbox" /><span>周四006</span></td><td align="center" style="width:50px;">巴兰基利亚青年 VS 蒙特维迪奥竞技</td>
        <td align="left" class="bnone" valign="middle" style="width:120px">
            <div style="padding-left:5px;">
              <label style="display:none;float:left; margin-left:4px;"><input type="checkbox"checked="checked"value="0" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'"><div style="width:20px; text-align:center; margin-top:3px; margin-bottom:2px;">0</div></em></label>
              <label style="display:none;float:left; margin-left:4px;"><input type="checkbox"checked="checked"value="1" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'"><div style="width:20px; text-align:center; margin-top:3px; margin-bottom:2px;">1</div></em></label>
              <label style="display:none;float:left; margin-left:4px;"><input type="checkbox"checked="checked"value="2" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'"><div style="width:20px; text-align:center; margin-top:3px; margin-bottom:2px;">2</div></em></label>
              <label style="display:none;float:left; margin-left:4px;"><input type="checkbox"checked="checked"value="3" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'"><div style="width:20px; text-align:center; margin-top:3px; margin-bottom:2px;">3</div></em></label>
              <label style="display:none;float:left; margin-left:4px;"><input type="checkbox"checked="checked"value="4" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'"><div style="width:20px; text-align:center; margin-top:3px; margin-bottom:2px;">4</div></em></label>
              <label style="display:none;float:left; margin-left:4px;"><input type="checkbox"checked="checked"value="5" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'"><div style="width:20px; text-align:center; margin-top:3px; margin-bottom:2px;">5</div></em></label>
              <label style="display:none;float:left; margin-left:4px;"><input type="checkbox"checked="checked"value="6" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'"><div style="width:20px; text-align:center; margin-top:3px; margin-bottom:2px;">6</div></em></label>
              <label style="display:none;float:left; margin-left:4px;"><input type="checkbox"checked="checked"value="7" visible="false" style="display: none;"/><em class="x_s" onmouseover="javascript:this.className='x_s nx_s'" onmouseout="javscript:this.className='x_s'"><div style="width:20px; text-align:center; margin-top:3px; margin-bottom:2px;">7+</div></em></label>
            </div>
        </td>
      <td align="right" style="line-height:20px; padding-right:5px;"><input type="checkbox" name="danma2929" value="2929" onclick="return damaclick(this,2929);" /></td>
      </tr>
      </tbody>   
      <tbody id="selectTeams"></tbody>
      <tbody><tr>
        <td colspan="4" style="text-align:right; padding-right:10px;">
            <span id="spnUnsureDan" style="display:none;">
		        <b>模糊设胆：</b><select name="dpUnsureDan" id="dpUnsureDan" ></select>
		        <a href="dt.aspx" class="spnLnk" target="_blank">使用说明</a>
		    </span>
		</td>
      </tr></tbody>
  </table></td>
  </tr>
  <tr>
  <td align="center" colspan="2"></td>
  </tr>
  </tbody></table>    
  </td>
  </tr>
  <tr>
  <td valign="top" width="240px">
    <table class="table_jczqtz_gg" style="" width="240px" border="0" cellpadding="4" cellspacing="0">    
     <tbody>        
            <tr><div style="height:6px;line-height:1px;"></div></tr>
     <tr>
  <td class="jczqtouzu_title_gg">确认投注信息及购买</td>
  </tr>
      <tr>
        <td>
        <table class="table_bdtz_gg" width="100%" border="0" cellpadding="4" cellspacing="0" style="margin:0;">
          <tbody>
           <tr class="t04">
            <td>1、选择过关方式</td>
            </tr>
           <tr>
            <td>
                <div class="gtitle">
			        <ul id="gTab">
				        <li class="active" onclick="nTabs(this,0);" style="border-right:1px solid #b4d8fc;">自由过关</li>
				        <li class="normal" onclick="nTabs(this,1);">多串过关</li>
			        </ul>
		        </div>
            </td>
           </tr>
            <tr class="t05">
            <td id="ggList" align="left" bgcolor="#ffffff">
            <div class="betr">
                    <div id="gTab_Content0">
                        <div style="height:6px; line-height:1px;"></div>
                        <label style="display:none;" for="r2c1"><input name="ggtype_radio" id="r2c1" value="2串1" type="checkbox" />2串1&nbsp;</label>
                        <label style="display:none;" for="r3c1"><input name="ggtype_radio" id="r3c1" value="3串1" type="checkbox" />3串1&nbsp;</label>
                        <label style="display:none;" for="r4c1"><input name="ggtype_radio" id="r4c1" value="4串1" type="checkbox" />4串1&nbsp;</label>
                        <div></div>
                        <label style="display:none;" for="r5c1"><input name="ggtype_radio" id="r5c1" value="5串1" type="checkbox" />5串1&nbsp;</label>
                        <label style="display:none;" for="r6c1"><input name="ggtype_radio" id="r6c1" value="6串1" type="checkbox" />6串1&nbsp;</label>
                        <label style="display:none;" for="r7c1"><input name="ggtype_radio" id="r7c1" value="7串1" type="checkbox" />7串1&nbsp;</label>
                        <label style="display:none;" for="r8c1"><input name="ggtype_radio" id="r8c1" value="8串1" type="checkbox" />8串1&nbsp;</label>
                        <div style="height:6px; line-height:1px;"></div>
                    </div>
                    <div id="gTab_Content1" class="none">
                        <div style="height:6px; line-height:1px;"></div>
                        <label style="display:none;" for="r3c3"><input name="ggtype_radio" id="r3c3" value="3串3" type="checkbox" />3串3&nbsp;</label>
			            <label style="display:none;" for="r3c4"><input name="ggtype_radio" id="r3c4" value="3串4" type="checkbox" />3串4&nbsp;</label>
                        <label style="display:none;" for="r4c4"><input name="ggtype_radio" id="r4c4" value="4串4" type="checkbox" />4串4&nbsp;</label>
			            <label style="display:none;" for="r4c5"><input name="ggtype_radio" id="r4c5" value="4串5" type="checkbox" />4串5</label>			        
			            <div></div> 
			            <label style="display:none;" for="r4c6"><input name="ggtype_radio" id="r4c6" value="4串6" type="checkbox" />4串6&nbsp;</label>
			            <label style="display:none;" for="r4c11"><input name="ggtype_radio" id="r4c11" value="4串11" type="checkbox" />4串11&nbsp;</label>
			            <div></div> 
			            <label style="display:none;" for="r5c5"><input name="ggtype_radio" id="r5c5" value="5串5" type="checkbox" />5串5&nbsp;</label>
			            <label style="display:none;" for="r5c6"><input name="ggtype_radio" id="r5c6" value="5串6" type="checkbox" />5串6&nbsp;</label>
			            <label style="display:none;" for="r5c10"><input name="ggtype_radio" id="r5c10" value="5串10" type="checkbox" />5串10</label>
			            <div></div>
			            <label style="display:none;" for="r5c16"><input name="ggtype_radio" id="r5c16" value="5串16" type="checkbox" />5串16</label>
			            <label style="display:none;" for="r5c20"><input name="ggtype_radio" id="r5c20" value="5串20" type="checkbox" />5串20</label>
			            <label style="display:none;" for="r5c26"><input name="ggtype_radio" id="r5c26" value="5串26" type="checkbox" />5串26</label>
			            <div></div>
			            <label style="display:none;" for="r6c6"><input name="ggtype_radio" id="r6c6" value="6串6" type="checkbox" />6串6&nbsp;</label>
			            <label style="display:none;" for="r6c7"><input name="ggtype_radio" id="r6c7" value="6串7" type="checkbox" />6串7&nbsp;</label>
			            <label style="display:none;" for="r6c15"><input name="ggtype_radio" id="r6c15" value="6串15" type="checkbox" />6串15</label>
			            <div></div>
			            <label style="display:none;" for="r6c20"><input name="ggtype_radio" id="r6c20" value="6串20" type="checkbox" />6串20</label>
			            <label style="display:none;" for="r6c22"><input name="ggtype_radio" id="r6c22" value="6串22" type="checkbox" />6串22</label>
			            <label style="display:none;" for="r6c35"><input name="ggtype_radio" id="r6c35" value="6串35" type="checkbox" />6串35</label>
			            <div></div>
			            <label style="display:none;" for="r6c42"><input name="ggtype_radio" id="r6c42" value="6串42" type="checkbox" />6串42</label>
			            <label style="display:none;" for="r6c50"><input name="ggtype_radio" id="r6c50" value="6串50" type="checkbox" />6串50</label>
			            
			            <label style="display:none;" for="r6c57"><input name="ggtype_radio" id="r6c57" value="6串57" type="checkbox" />6串57</label>
			            <div></div>
			            <label style="display:none;" for="r7c7"><input name="ggtype_radio" id="r7c7" value="7串7" type="checkbox" />7串7&nbsp;</label>
			            <label style="display:none;" for="r7c8"><input name="ggtype_radio" id="r7c8" value="7串8" type="checkbox" />7串8&nbsp;</label>
			            <label style="display:none;" for="r7c21"><input name="ggtype_radio" id="r7c21" value="7串21" type="checkbox" />7串21</label>
			            <div></div>
			            <label style="display:none;" for="r7c35"><input name="ggtype_radio" id="r7c35" value="7串35" type="checkbox" />7串35</label>
			            <label style="display:none;" for="r7c120"><input name="ggtype_radio" id="r7c120" value="7串120" type="checkbox" />7串120&nbsp;</label>
			            <div></div>
			            <label style="display:none;" for="r8c8"><input name="ggtype_radio" id="r8c8" value="8串8" type="checkbox" />8串8&nbsp;</label>
			            <label style="display:none;" for="r8c9"><input name="ggtype_radio" id="r8c9" value="8串9" type="checkbox" />8串9&nbsp;</label>
			            <label style="display:none;" for="r8c28"><input name="ggtype_radio" id="r8c28" value="8串28" type="checkbox" />8串28</label>
			            <div></div>
			            <label style="display:none;" for="r8c56"><input name="ggtype_radio" id="r8c56" value="8串56" type="checkbox" />8串56</label>
			            <label style="display:none;" for="r8c70"><input name="ggtype_radio" id="r8c70" value="8串70" type="checkbox" />8串70</label>
			            <label style="display:none;" for="r8c247"><input name="ggtype_radio" id="r8c247" value="8串247" type="checkbox" />8串247</label>
			            <div style="height:6px; line-height:1px;"></div>
			        </div>
            </div>
              </td>
            </tr>
            <tr class="t04">
                <td>
                    2、确认投注信息
                </td>
            </tr>
            <tr>
                <td>
                    <table class="table_BetInfo" width="100%" border="0" cellpadding="2" cellspacing="0">
                        <tr>
                            <td align="right">
                                金额：</td>
                                <td align="left">
                                注数<span style="font-size:12px;color:Red;" id="showCount">0</span>注，<span class="r14px" id="showMoney">￥0.00</span>元<br />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                倍数：</td>
                            <td align="left">
                                <input class="jczqtz_input" id="buybs" value="1" size="7" maxlength="5" style="width:50px"/>
                                (最高99999倍)
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="78">                            
                               <span>理论最高奖金</span>
                            </td>
                            <td align="left"> 
                            <span id="maxmoney" class="r14px">￥0.00</span>
                            <br /><a href="javascript:void(0);" id="lookDetails">查看奖金明细</a>
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
                                    <input type="text" id="tb_Share" name="tb_Share" maxlength="10" value="1"  style="width:50px"/>份
                                    <div></div>每份<span id="lab_ShareMoney" style="color: Red">0.00</span>&nbsp;元。
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    我要认购：</td>
                                <td align="left">
                                    <input type="text" id="tb_BuyShare" name="tb_BuyShare" value="1" style="width:50px"/>份
                                        <span style="color:Gray">&nbsp;所占比例:</span><span style="color:Gray" id="lab_BuyScale">0%</span>
                                    <div></div>金额 <span id="lab_BuyMoney" style="color: Red">0.00</span>&nbsp;元。
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    我要保底：</td>
                                <td align="left">
                                    <input type="text" id="tb_AssureShare" name="tb_AssureShare" value="0" style="width:50px">份
                                        <span style="color:Gray">&nbsp;所占比例:</span><span style="color:Gray" id="lab_AssureScale">0%</span>
                                    <div></div>保底 <span id="lab_AssureMoney" style="color: Red">0.00</span>&nbsp;元。
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
                        <tr height="60px">
                            <td colspan="2">
                                <div class="tz_r_btn">			
                    		        <span><a href=javascript:void(0) class="btn_tzbuy" id="dgBtn">确认购买</a></span>
                                    <span><a href=javascript:void(0) class="btn_tzbuy" id="FilterBtn">高级过滤</a></span>
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
    </tbody></table>
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
<input type="hidden" id="jsonggtype" value="{'单关':'A0','2串1':'AA','3串1':'AB','3串3':'AC','3串4':'AD','4串1':'AE','4串4':'AF','4串5':'AG','4串6':'AH','4串11':'AI','5串1':'AJ','5串5':'AK','5串6':'AL','5串10':'AM','5串16':'AN','5串20':'AO','5串26':'AP','6串1':'AQ','6串6':'AR','6串7':'AS','6串15':'AT','6串20':'AU','6串22':'AV','6串35':'AW','6串42':'AX','6串50':'AY','6串57':'AZ','7串1':'BA','7串7':'BB','7串8':'BC','7串21':'BD','7串35':'BE','7串120':'BF','8串1':'BG','8串8':'BH','8串9':'BI','8串28':'BJ','8串56':'BK','8串70':'BL','8串247':'BM'}" />
<input type="hidden" id="ggtypename" name="sgtypename" value="普通过关" />
<input type="hidden" id="ggtypeid" name="ggtypeid" value="AA" />
<input type="hidden" id="codes" name="codes" />
<input type="hidden" id="totalmoney" name="totalmoney" />
<input type="hidden" id="zhushu" name="zhushu" />
<input type="hidden" id="beishu" name="beishu" />
<input type="hidden" id="playid" name="playid" value="7203" />
<input type="hidden" id="playname" name="playname" value="竞彩总进球数" />
<input type="hidden" id="lotid" name="lotid" value="72" />
<input type="hidden" id="SecrecyL" name="SecrecyL" value="0" />
<input type="hidden" id="AssureMoney" name="AssureMoney" value="0" />
<input type="hidden" id="SchemeSchemeBonusScalec" name="SchemeSchemeBonusScalec" runat="server"/>
<div class="layer" id="popWindow" style="position:absolute; z-index:9999; display:none; top:400px; left:25%;">
  <div class="layer2">
    <div class="lay_box4">
      <div class="title_t title_t4 clear"><h4><span id="popWindowTitle">竞彩足球奖金明细</span></h4><a href="javascript:void(0)" class="del" title="关闭" onclick="document.getElementById('popWindow').style.display='none'" alt="关闭">关闭</a></div>
      <div class="lay_content2 scroll" id="popContent">
            <table class="table1" cellSpacing="1"  cellPadding="2" width="500" border="0" style="margin:10px auto">
				<tr>
					<th>场次</th>
					<th>比赛</th>
					<th>您的选择(SP)</th>
					<th>最小SP值</th>
					<th>最大SP值</th>
				</tr>
				<tr>
					<td>周四002</td>
					<td>英格兰超级联赛</td>
					<td>平(6.72) 负(1.94)</td>
					<td>6.27</td>
					<td>1.94</td>
					<td>x</td>
				</tr>
				<tr>
					<td>周四003</td>
					<td>意大利</td>
					<td>平(6.72) 负(1.94)</td>
					<td>6.27</td>
					<td>1.94</td>
				</tr>
				<tr>
					<td colspan=6>过关方式：4串11 倍数：1倍 方案总金额：￥92.00元</td>
				</tr>
			</table>
			<table cellspacing="0" cellpadding="0">
				<tr class="trt">
					<th rowspan="2">命中场数</th>
					<th colspan=3>中奖注数</th>
					<th>倍数</th>
					<th colspan=2>奖金范围</th>
				</tr>
				<tr>
					<th>6串1</th>
					<th>5串1</th>
					<th>6.27</th>
					<th>1.94</th>
					<th>最大奖金</th>
					<th>最小奖金</th>
				</tr>
				<tr>
					<td>1</td>
					<td>1注</td>
					<td>5注</td>
					<td>15注</td>
					<td>1.94</td>
					<td>3689.20元</td>
					<td>3689.20元</td>
				</tr>
				<tr>
					<td colspan=7>注：奖金预测sp值为投注时即时sp值，最终奖金以开奖sp值为准</td>
				</tr>
			</table>
			<table cellspacing="0" cellpadding="0">
				<tr>
					<th>过关方式</th>
					<th>中奖注数</th>
					<th>中6场 最小奖金 中奖明细</th>
					<th>奖金</th>
				</tr>
				<tr>
					<td>6串1</td>
					<td>1注</td>
					<td>[50]1.59×[51]1.68×[48]1.72×[47]1.83×[46]1.87×[49]1.94×1倍×2元×65% = 39.65元</td>
					<td>396.5元</td>
				</tr>
				<tr>
					<td>合计</td>
					<td>63注</td>
					<td>&nbsp;</td>
					<td>9999.23元</td>
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
<script type="text/javascript" src="JScript/tool.js"></script>
<script type="text/javascript" src="JScript/com.js"></script>

<script type="text/javascript" src="JScript/gg_jqs.js"></script>

<script id="config" type="text/javascript" src="JScript/form.js?2"></script>
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