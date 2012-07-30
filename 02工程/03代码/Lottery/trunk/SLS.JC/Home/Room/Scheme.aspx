<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Scheme.aspx.cs" Inherits="Home_Room_Scheme" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>彩票购买方案-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！</title>
    <meta name="description" content="彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。" />
    <meta name="keywords" content="竞彩开奖，彩票走势图，超级大乐透，排列3/5" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="shortcut icon" href="../../favicon.ico" />
    <script language="javascript" type="text/javascript">
    var o_labBalance;
    var o_labShare;
    var o_labShareMoney;
    var o_tbShare;
    var o_labSumMoney;
    var o_btnOK;
    var o_labWinNumber;
    var o_labReward;

    function Init() {
        o_labBalance = document.getElementById("<%=labBalance.ClientID%>");
        o_labShare = document.getElementById("<%=labBuyedShare.ClientID%>");
        o_labShareMoney = document.getElementById("<%=labShareMoney.ClientID%>");
        o_tbShare = document.getElementById("<%=tbShare.ClientID%>");
        o_labSumMoney = document.getElementById("<%=labSumMoney.ClientID%>");
        o_btnOK = document.getElementById("<%=btnOK.ClientID%>");
        o_labWinNumber = document.getElementById("<%=lbWinNumber.ClientID%>");
        o_labReward = document.getElementById("<%=lbReward.ClientID%>");
        var o_trWinNumber = document.getElementById("trWinNumber");

        if (o_labWinNumber.innerHTML != "") {
            trWinNumber.style.display = "";
        }
        else {
            trWinNumber.style.display = "none";
        }

        var o_trReward = document.getElementById("trReward");
    
        if(o_trReward)
        {
            if (o_labReward.innerHTML != "") {
                trReward.style.display = "";
            }
            else {
                trReward.style.display = "none";
            }
        }
    }

    function SetbtnOKFocus() {
        o_btnOK.focus();
        return true;
    }

    function onUserListClick() {
        var obj1 = document.getElementById("trUserListDetail");

        if (obj1.style.display == "none") {
            obj1.style.display = "";
        }
        else {
            obj1.style.display = "none";
        }
    }

    function InputMask_Number() {
        if (window.event.keyCode < 48 || window.event.keyCode > 57)
            return false;
        return true;
    }

    function CheckShare(sender) {
        var BuyShare = StrToInt(sender.value);
        var SpareShare = StrToInt(o_labShare.innerText);

        if ((BuyShare < 1) || (BuyShare > SpareShare)) {
            if (confirm("份数不正确，按“确定”重新输入，按“取消”自动更正为 " + SpareShare + " 份，请选择。")) {
                sender.focus();
                return;
            }
            else {
                BuyShare = SpareShare;
                sender.value = SpareShare;
            }
        }
        o_labSumMoney.innerText = Round(BuyShare * StrToFloat(o_labShareMoney.innerText), 2);
        SetbtnOKFocus();
    }

    function btnOKClick() {
        var BuyShare = StrToInt(o_tbShare.value);
        var SpareShare = StrToInt(o_labShare.innerText);
        var SumMoney = StrToFloat(o_labSumMoney.innerText);

        if ((BuyShare < 1) || (BuyShare > SpareShare)) {
            alert("请输入正确的认购份数。");
            o_tbShare.focus();
            return false;
        }
        if (SumMoney < 0) {
            alert("输入有错误。");
            return false;
        }
        o_labBalance.innerText = Home_Room_Scheme.GetUserBalance().value;
        var Balance = StrToFloat(o_labBalance.innerText);
        if (Balance < SumMoney) {
            if (confirm("您的账户余额不足，请先充值，谢谢。您要立即在线购买吗？"))
                window.document.location.href = "MyIcaile.aspx?SubPage=OnlinePay/Default.aspx";
            return false;
        }

        var TipStr = "您要入伙此合买方案，详细内容：\n\n";
        TipStr += "　　份　数：　" + BuyShare + " 份\n";
        TipStr += "　　每　份：　" + o_labShareMoney.innerText + " 元\n";
        TipStr += "　　总金额：　" + SumMoney + " 元\n\n";

        if (!confirm(TipStr + "按“确定”即表示您已阅读《用户电话短信投注协议》并立即参与合买方案，确定要入伙吗？"))
            return false;

        __doPostBack("btnOK", "");
    }

    function CreateUplaodDialog() {
        var msgw, msgh, bordercolor;
        msgw = 580; //提示窗口的宽度 
        msgh = 450; //提示窗口的高度 
        //titleheight=25 //提示窗口标题高度 
        //bordercolor="#336699";//提示窗口的边框颜色
        //titlecolor="#99CCFF";//提示窗口的标题颜色
        var sWidth, sHeight;
        sWidth = document.body.offsetWidth;
        sHeight = document.body.offsetHeight;
        var bgObj = document.createElement("div");
        bgObj.setAttribute('id', 'bgDiv2');
        bgObj.style.position = "absolute";
        bgObj.style.top = "0";
        bgObj.style.background = "#777";
        bgObj.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75";
        bgObj.style.opacity = "0.6";
        bgObj.style.left = "0";
        bgObj.style.width = sWidth + "px";
        bgObj.style.height = sHeight + "px";
        bgObj.style.zIndex = "10000";
        document.body.appendChild(bgObj);

        var msgObj = document.createElement("div")
        msgObj.setAttribute("id", "msgDiv2");
        msgObj.setAttribute("align", "center");
        msgObj.style.backcolor = "white";
        //msgObj.style.border="1px solid " + bordercolor; 
        msgObj.style.position = "absolute";
        msgObj.style.left = "50%";
        msgObj.style.top = "20%";
        msgObj.style.font = "12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif";
        msgObj.style.marginLeft = "-225px";
        msgObj.style.marginTop = document.documentElement.scrollTop + "px";
        msgObj.style.width = msgw + "px";
        msgObj.style.height = msgh + "px";
        msgObj.style.textAlign = "center";
        msgObj.style.lineHeight = "25px";
        msgObj.style.zIndex = "10001";
        document.body.appendChild(msgObj);
        var txt = document.createElement("p");
        txt.style.margin = "1em 0"
        txt.setAttribute("id", "msgTxt2");
        var dialog = '<table><tr><td style="background-color: #AFBCD6; padding: 10px;font-size:12px"><table style="width: 100%;background-color:White;" border="0" cellpadding="0" cellspacing="1"><tr><td height="36" bgcolor="#6D84B4" class="bai14" style="padding: 0px 10px 0px 15px;text-align:left;"><span id="lbLotteryName"></span> 第 <span id="lbIsuse"></span>&nbsp;期 粘贴投注</td></tr><tr><td style="padding: 5px;" align="center"><textarea id="tbLotteryNumbers" style="width:98%; height:160px;"></textarea></td></tr><tr><td><table width="100%" border="0" align="right" cellpadding="0" cellspacing="0"><tr><td style="text-align:left;"><table cellpadding="0" cellspacing="0" style="width:100%;"><tr><td style="text-align:right;">方案上传：</td><td colspan="2"><iframe id="frame_Upload" name="frame_Upload" frameborder="0" src="SchemeUpload.aspx?id=<%=LotteryID %>&PlayType=<%=PlayTypeID %>" width="100%" scrolling="no" height="30"></iframe></td></tr></table></td></tr><tr><td style="text-align:right; padding-right:10px;"><font color="#ff0000">【注】</font>如果选择方案文件<font color="#ff0000">(.txt格式)</font>上传,上面的投注内容将被清除并被替换成方案文件里面的内容。<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 方案文件中请输入规范的投注内容，多注请用回车换行。 <span class="blue12"><a href="SchemeExemple.aspx?id=' + <%=LotteryID %> + '" target="_blank">请参看格式规范</a></span></td></tr><tr><td style="background-color:#f2f2f2; padding:10px;"><table width="280" border="0" align="right" cellpadding="0" cellspacing="0"><tbody style="cursor: pointer; color: White;"><tr><td width="19%" align="right"><table width="88" border="0" cellpadding="0" cellspacing="1" bgcolor="#FF3300"><tr><td height="23" align="center" bgcolor="#FD9A00" onclick=" btn_OK();">确定</td></tr></table></td><td width="32%" align="right"><table width="88" border="0" cellpadding="0" cellspacing="1" bgcolor="#FF3300"><tr><td height="23" align="center" bgcolor="#FD9A00" onclick=" btn_Close();">取消</td></tr></table></td><td width="32%" align="right"><table width="88" border="0" cellpadding="0" cellspacing="1" bgcolor="#FF3300"><tr><td height="23" align="center" bgcolor="#FD9A00" onclick=" btn_Close();">关闭窗口</td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></table></td></tr></table>';
        txt.innerHTML = dialog;
        document.getElementById("msgDiv2").appendChild(txt);
        document.getElementById("tbLotteryNumbers").focus();
        document.getElementById("lbIsuse").innerHTML = document.getElementById('labTitle').innerHTML;
        document.getElementById("lbLotteryName").innerHTML = '<%=LotteryName %>';
        return false;
    }

    function btn_Close() {
        document.body.removeChild(bgDiv2);
        document.body.removeChild(msgDiv2);
    }

    function btn_OK() {
        try {
            var LotteryNumber = Home_Room_Scheme.AnalyseScheme(document.getElementById("tbLotteryNumbers").value, '<%=LotteryID %>', '<%=PlayTypeID %>');
            if (LotteryNumber == null || LotteryNumber.value == null) {
                document.body.removeChild(bgDiv2);
                document.body.removeChild(msgDiv2);
                alert("从方案文件中没有提取到符合书写规则的投注内容。");
                return;
            }

            var r = LotteryNumber.value;

            if (typeof (r) != "string") {
                document.body.removeChild(bgDiv2);
                document.body.removeChild(msgDiv2);
                alert("从方案文件中没有提取到符合书写规则的投注内容。");
                return;
            }
        }
        catch (e) {
            document.body.removeChild(bgDiv2);
            document.body.removeChild(msgDiv2);
            alert("从方案文件中没有提取到符合书写规则的投注内容。");
            return;
        }

        var Lotterys = r.split("\n");
        var Num=0;
        var Content="";
        for (var i = 0; i < Lotterys.length; i++) {
            var str = Lotterys[i].trim();
            if (str == "")
                continue;
            strs = str.split("|");
            if (strs.length != 2) {
                continue;
            }

            str = strs[0].trim();
            if (str == "") {
                continue;
            }
        
            Content += str+"\n";
            Num += StrToInt(strs[1]);
        }
    
        if(Num*2 < StrToInt(labSchemeMoney.innerHTML) || Num * 2 > StrToInt(document.getElementById("hidMaxMoney").value))
        {
            alert("上传的金额与方案金额不相符, 您的方案金额必须在"+labSchemeMoney.innerHTML+"至"+ StrToFloat(document.getElementById("hidMaxMoney").value) +"之间！");
            return;
        }
    
        var result = Home_Room_Scheme.UpdateLotteryNumber(String('<%=SchemeID %>'),Content, Num * 2).value;
        alert(result);
        document.body.removeChild(bgDiv2);
        document.body.removeChild(msgDiv2);
        window.location.href = window.location.href;
    }
    </script>
</head>
<body class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<asp:HiddenField ID="hfID" runat="server" />
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div style="padding-top:10px;">
        <table align="center" cellpadding="0" cellspacing="1" bgcolor="#9FC8EA" class="Schemetable">
            <tr style="display: none;">
                <td height="50" background="images/bg_58.jpg">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <asp:Image ID="ImageLogo" runat="server" />
                            </td>
                            <td  style="padding-right: 50px; padding-left: 20px;">
                                第<span class="red14"><asp:Label ID="labTitle" runat="server" Text=""></asp:Label></span>期
                                <asp:TextBox ID="tbIsuseID" runat="server" Width="30px" Visible="False"></asp:TextBox>
                                <asp:TextBox ID="tbLotteryID" runat="server" Width="30px" Visible="False"></asp:TextBox>
                                <asp:TextBox ID="tbSchemeID" runat="server" Width="30px" Visible="False"></asp:TextBox>
                                <asp:TextBox ID="tbStop" runat="server" Width="30px" Visible="False"></asp:TextBox>
                            </td>
                            <td>
                                认购开始时间：
                                <asp:Label ID="labStartTime" runat="server" ForeColor="Red"></asp:Label>
                                认购截止时间：
                                <asp:Label ID="labEndTime" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td bgcolor="#FFFFFF">
                    <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#E2EAED"
                        id="Table2" style="width: 100%">
                        <tr>
                            <td colspan="2" align="left" bgcolor="#f7f7f7" style="padding-left:50px; font-size:13px; font-family:微软雅黑; height:26px;">
                                <asp:Label ID="Label3" runat="server" class="style1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" colspan="2" bgcolor="#E9F1F8" style="padding-left: 12px;">
                                <strong>方案基本信息</strong>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right" bgcolor="#F6F9FE" style="width: 120px">
                                <font face="Tahoma">方案发起人：</font>
                            </td>
                            <td align="left" valign="top" bgcolor="#FFFFFF" style="padding-left: 12px;">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="labInitiateUser" runat="server"></asp:Label>
                                            &nbsp;&nbsp;
                                            <ShoveWebUI:ShoveConfirmButton ID="btnQuashScheme" BackgroupImage="../images/btnBack02.gif"
                                                Style="font-size: 9pt; cursor: hand; border-top-style: none; font-family: Tahoma;
                                                border-right-style: none; border-left-style: none; border-bottom-style: none"
                                                runat="server" Height="20px" Width="84px" Text="我要撤消方案" Visible="False" CommandName="QuashIsuse"
                                                AlertText="确信要撤消此方案吗？" OnClick="btnQuashScheme_Click" onblur="return SetbtnOKFocus();" />&nbsp;
                                            <asp:CheckBox ID="cbAtTopApplication" runat="server" Text="申请置顶" AutoPostBack="True"
                                                Visible="False" OnCheckedChanged="cbAtTopApplication_CheckedChanged"></asp:CheckBox>
                                            <span>
                                                <asp:Label ID="labAtTop" runat="server" Visible="False">方案已置顶</asp:Label></span>
                                        </td>
                                        <td width="300px;" style="margin-right: 0px;">
                                            <asp:ImageButton ID="btn_Single" ImageUrl="../web/images/btnzzThis.jpg" runat="server"
                                                Width="133px" Height="28px" OnClientClick="return CreateLogin(this);" OnClick="btn_Single_Click" />
                                            <asp:ImageButton ID="btn_All" ImageUrl="../web/images/btnzzAll.jpg" runat="server"
                                                Width="133px" Height="28px" OnClientClick="return CreateLogin(this);" OnClick="btn_All_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right" bgcolor="#F6F9FE" style="width: 120px">
                                <font face="Tahoma">方案信息：</font>
                            </td>
                            <td align="left" bgcolor="#FFFFFF" style="padding-left: 12px;">
                                <div id="fanandiv" class="tdbback">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="tablelay eng">
                                  <tr>
                                    <th width="18%">方案编号</th>
                                    <th width="10%">总金额</th>
                                    <th width="8%">倍数</th>
                                    <th width="8%">份数</th>
                                    <th width="10%">每份</th>
                                    <th width="10%">佣金比例</th>
                                    <th width="12%">保底金额</th>
                                    <th width="10%">购买进度</th>
                                    <th width="12%" class="last_th">状态</th>
                                  </tr>
                                  <tr class="last_tr">
	                                <td><asp:Label ID="labSchemeNumber" runat="server"></asp:Label></td> 
                                    <td><span class="red eng">￥<asp:Label ID="labSchemeMoney" runat="server"></asp:Label></span>元</td>                                                            
                                    <td><asp:Label ID="labMultiple" runat="server"></asp:Label>倍</td>
                                    <td><asp:Label ID="labShare" runat="server"></asp:Label>份</td>
                                    <td nowrap="nowrap">￥<asp:Label ID="labShareMoney" runat="server" ForeColor="Red">0.00</asp:Label>元</td>
                                    <td nowrap="nowrap"><span class="red eng"><asp:Label ID="lbSchemeBonus" runat="server"></asp:Label></span></td>
                                    <td><asp:Label ID="labAssureMoney" runat="server"></asp:Label></td>
                                    <td><span class="red eng"><asp:Label ID="labSchedule" runat="server"></asp:Label>%</span></td>
                                    <td class="last_td"><asp:Label ID="labState" runat="server"></asp:Label></td>
                                  </tr>
                                </table>
                             </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right" bgcolor="#F6F9FE" style="width: 120px">
                                <font face="Tahoma">投注内容：</font>
                            </td>
                            <td align="left" bgcolor="#FFFFFF" style="padding-right: 2px; padding-left: 12px; padding-bottom: 2px; padding-top: 2px">  
                                <asp:HyperLink ID="linkDownloadScheme" runat="server" Visible="False" Target="_blank">下载方案</asp:HyperLink>
                                <asp:LinkButton ID="lbUploadScheme" runat="server" Visible="False" OnClientClick="return CreateUplaodDialog()">上传方案</asp:LinkButton>
				                 <!--点击展开查看方案begin-->
                                <asp:Label ID="labLotteryNumber" runat="server"></asp:Label>
                                <!--点击展开查看方案end-->
                            </td>
                        </tr>
                        <tr id="trWinNumber">
                            <td height="25" align="right" bgcolor="#F6F9FE" id="td1" style="width: 120px; height: 19px">
                                <font face="Tahoma">开奖号码：</font>
                            </td>
                            <td align="left" bgcolor="#FFFFFF" id="td2" style="padding-left: 12px;">
                                <span><asp:Label ID="lbWinNumber" runat="server" Font-Bold="true"></asp:Label></span>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" colspan="2" bgcolor="#E9F1F8" style="padding-left: 12px;">
                                <strong>方案投注信息</strong>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right" bgcolor="#F6F9FE" style="width: 120px">
                                <font face="Tahoma">方案标题：</font>
                            </td>
                            <td align="left" bgcolor="#FFFFFF" style="padding-left: 12px;">
                                <asp:Label ID="labSchemeTitle" runat="server" Style="word-break: break-all; word-wrap: break-word"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right" bgcolor="#F6F9FE" style="width: 120px">
                                <font face="Tahoma">方案描述：</font>
                            </td>
                            <td align="left" valign="top" bgcolor="#FFFFFF" style="padding-left: 12px;">
                                <asp:Label ID="labSchemeDescription" runat="server" Style="word-break: break-all;word-wrap: break-word"></asp:Label>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td height="25" align="right" bgcolor="#F6F9FE" style="width: 120px">
                                <font face="Tahoma">分享给好友：</font>
                            </td>
                            <td align="left" valign="top" bgcolor="#FFFFFF" style="padding-left: 12px;">
                                <asp:Label ID="labSchemeADUrl" runat="server"></asp:Label>
                                <!-- JiaThis Button BEGIN -->
                                <div id="ckepop">
                                    <span class="jiathis_txt">分享到：</span>
                                    <a class="jiathis_button_qzone"></a>
                                    <a class="jiathis_button_tsina"></a>
                                    <a class="jiathis_button_renren"></a>
                                    <a class="jiathis_button_kaixin001"></a>
                                    <a href="http://www.jiathis.com/share/?uid=1526992" class="jiathis jiathis_txt jtico jtico_jiathis" target="_blank"></a>
                                    <a class="jiathis_counter_style"></a>
                                </div>
                                <script type="text/javascript">    var jiathis_config = { data_track_clickback: true };</script>
                                <script type="text/javascript" src="http://v2.jiathis.com/code/jia.js?uid=1526992" charset="utf-8"></script>
                                <!-- JiaThis Button END -->
                            </td>
                        </tr>
                        <tr>
                            <td height="36" align="right" bgcolor="#FFF2F2" style="width: 120px">
                                <font face="Tahoma">我要认购：</font>
                            </td>
                            <td align="left" bgcolor="#FFF2F2" style="padding-left: 12px;">
                                <asp:Label ID="labCannotBuyTip" runat="server" Visible="False"></asp:Label><asp:Panel
                                    ID="pBuy" runat="server" Visible="False" Width="100%">
                                    我的账户余额
                                    <asp:Label ID="labBalance" runat="server" ForeColor="Red">0.00</asp:Label>&nbsp;,此方案还有
                                    <asp:Label ID="labBuyedShare" runat="server" ForeColor="Red">0</asp:Label>&nbsp;份可以认购,我想认购&nbsp;
                                    <asp:TextBox onkeypress="return InputMask_Number();" class="in_text_hui" ID="tbShare"
                                        onblur="return CheckShare(this);" runat="server" Width="64px"></asp:TextBox>&nbsp;份,总金额
                                    <asp:Label ID="labSumMoney" runat="server" ForeColor="Red">0.00</asp:Label>&nbsp;元</asp:Panel>
                                【<a href="OnlinePay/Default.aspx" target="_blank">用户充值</a>】【<a href="AccountDetail.aspx"
                                    target="_blank">账户明细</a>】
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; margin: 20px; background-color: #ffffff; padding-bottom: 5px;
                                padding-top: 5px;" id="divOK" runat="server" colspan="2">
                                <ShoveWebUI:ShoveConfirmButton ID="btnOK" BackgroupImage="images/button_qxgm.jpg"
                                    runat="server" Style="cursor: pointer;" Height="38" Width="127" OnClick="btnOK_Click"
                                    OnClientClick="return CreateLogin('btnOKClick()');" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; background-color: #ffffff;" id="divqueding"
                                runat="server" colspan="2">
                                【注】点击的“确定购买”按钮即表示您已阅读了<a href="/Home/Room/BuyProtocol.aspx?LotteryID=<%=LotteryID %>" target="_blank">《用户电话短信投注协议》</a>并同意其中条款。
                            </td>
                        </tr>
                        <tbody id="tbgcjl" runat="server">
                            <tr>
                                <td height="25" colspan="2" bgcolor="#E9F1F8" style="padding-left: 12px;">
                                    <strong>方案认购信息</strong>
                                </td>
                            </tr>
                            <tr>
                                <td height="25" align="right" bgcolor="#F6F9FE" style="width: 120px">
                                    <font face="Tahoma">参与用户列表：</font>
                                </td>
                                <td align="left" bgcolor="#FFFFFF" style="padding-left: 12px;">
                                    <asp:Label ID="labUserList" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trUserListDetail">
                                <td height="25" bgcolor="#F6F9FE">
                                </td>
                                <td bgcolor="#FFFFFF" align="left" style="padding-left: 8px; padding-bottom: 8px;
                                    padding-right: 8px; padding-top: 8px;">
                                    <ShoveWebUI:ShoveDataList ID="gUserList" runat="server" Width="100%" RepeatColumns="2"
                                        AllowPaging="true" PageSize="60" NextPageText="下一页" PageMode="NextPrev" PrevPageText="上一页"
                                        OnPageIndexChanged="gUserList_PageIndexChanged" PagerPosition="Bottom">
                                        <ItemTemplate>
                                            <table cellspacing="0" cellpadding="0" width="100%">
                                                <tr>
                                                    <td class="s2">
                                                        <%# Eval("Name")%>：
                                                    </td>
                                                    <td>
                                                        <font color="red">
                                                            <%# Eval("Share")%>
                                                        </font>份
                                                    </td>
                                                    <td>
                                                        <font color="red">
                                                            <%# double.Parse(Eval("DetailMoney").ToString()).ToString("N")%>
                                                        </font>元
                                                    </td>
                                                    <td class="s1">
                                                        <%# Eval("DateTime")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </ShoveWebUI:ShoveDataList>
                                </td>
                            </tr>
                            <tr>
                                <td height="25" align="right" bgcolor="#F6F9FE" style="width: 120px">
                                    <font face="Tahoma">我的认购记录：</font>
                                </td>
                                <td align="left" bgcolor="#FFFFFF" style="padding-left: 12px;">
                                    <asp:Label ID="labMyBuy" runat="server"></asp:Label>
                                    <asp:DataGrid ID="g" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None"
                                        ShowHeader="False" OnItemCommand="g_ItemCommand" OnItemDataBound="g_ItemDataBound">
                                        <Columns>
                                            <asp:BoundColumn DataField="Share">
                                                <HeaderStyle Width="5%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="DetailMoney">
                                                <HeaderStyle Width="10%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn>
                                                <HeaderStyle Width="35%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="DateTime">
                                                <HeaderStyle Width="20%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn>
                                                <HeaderStyle Width="10%"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle Width="20%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <ShoveWebUI:ShoveConfirmButton ID="btnQuashBuy" BackgroupImage="images/btnBack02.gif"
                                                        Style="font-size: 9pt; cursor: hand; border-top-style: none; font-family: Tahoma;
                                                        border-right-style: none; border-left-style: none; border-bottom-style: none"
                                                        runat="server" Height="20px" Width="84px" Text="我要撤消" CommandName="QuashBuy"
                                                        AlertText="确信要撤消此认购记录吗？" onblur="return SetbtnOKFocus();" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn Visible="False" DataField="QuashStatus">
                                                <HeaderStyle Width="0px"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="Buyed">
                                                <HeaderStyle Width="0px"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="IsuseID"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="Code"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="BuyedShare"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="Schedule"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="id"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="isWhenInitiate"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="SchemeShare"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr>
                                <td height="25" colspan="2" bgcolor="#E9F1F8" style="padding-left: 12px;">
                                    <strong>方案中奖信息</strong>
                                </td>
                            </tr>
                        </tbody>
                        <tr>
                            <td height="25" align="right" bgcolor="#F6F9FE" style="width: 120px">
                                <font face="Tahoma">中奖情况：</font>
                            </td>
                            <td align="left" valign="top" bgcolor="#FFFFFF" style="padding-left: 12px;">
                                <asp:Label ID="labWin" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trReward" runat="server">
                            <td height="25" align="right" bgcolor="#F6F9FE" id="td5" style="width: 120px">
                                <font face="Tahoma">我的奖金：</font>
                            </td>
                            <td align="left" valign="top" bgcolor="#FFFFFF" id="td6" style="padding-left: 12px;">
                                <asp:Label ID="lbReward" runat="server" ForeColor="red"></asp:Label>
                                元
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="hidMaxMoney" runat="server" />


</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>

    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
<script src="/JScript/Public.js" type="text/javascript" language="javascript"></script>
<script language="javascript" type="text/javascript">
    Init();
</script>