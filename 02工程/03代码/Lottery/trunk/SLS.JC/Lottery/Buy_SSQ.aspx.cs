using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

public partial class Lottery_Buy_SSQ: RoomPageBase
{
    public int LotteryID;
    public string LotteryName;

    public string script = "";
    public string DZ = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Lottery_Buy_SSQ), this.Page);
        LotteryID = 5;

        bool result = false;
        string UseLotteryList = Shove._Web.Cache.GetCacheAsString("Site_UseLotteryList" + _Site.ID, "");
        string[] Lottery = null;

        if (UseLotteryList == "")
        {
            UseLotteryList = DAL.Functions.F_GetUsedLotteryList(_Site.ID);

            if (UseLotteryList != "")
            {
                Shove._Web.Cache.SetCache("Site_UseLotteryList" + _Site.ID, UseLotteryList);
            }
        }

        Lottery = UseLotteryList.Split(',');

        for (int i = 0; i < Lottery.Length; i++)
        {
            if (LotteryID.ToString().Equals(Lottery[i]))
            {
                result = true;
                break;
            }
        }

        if (result == false)
        {
            Response.Redirect("../Default.aspx");
        }

        hlAgreement.NavigateUrl = "../Home/Room/BuyProtocol.aspx?LotteryID=" + LotteryID;

        LotteryName = DataCache.Lotteries[LotteryID];


        if (!IsPostBack)
        {
            long BuyID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("BuyID"), -1);
            if (BuyID > 0)
            {
                BindDataForAliBuy(BuyID);
            }

            tbWin1.InnerHtml = BindWinList(DataCache.GetWinInfo(LotteryID));
           
            DZ = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), Shove._Web.Utility.GetRequest("DZ"));

            tdLuckNumber.InnerHtml = InitLuckLotteryNumber();

            BindWinNumber(LotteryID);
        }
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string GetWinNumber(int lotteryID)
    {
        DataTable dtIsuses = DataCache.GetWinNumber(lotteryID);

        StringBuilder sb = new StringBuilder();

        if (dtIsuses != null && dtIsuses.Rows.Count > 0)
        {
            int i = 1;
            foreach (DataRow dr in dtIsuses.Rows)
            {
                sb.AppendLine("<tr>")
               .AppendLine("<td  height=\"22\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"blue12\">")
               .AppendLine(dr["Name"].ToString())
               .AppendLine("</td>");

                sb.AppendLine("<td  align=\"center\" bgcolor=\"#FFFFFF\" class=\"red2\">")
                .AppendLine(dr["WinLotteryNumber"].ToString())
                .AppendLine("</td>")
                .AppendLine("</tr>");

                if (i % 10 == 0 && i != dtIsuses.Rows.Count)
                {
                    sb.Append("|");
                }

                i++;

            }

        }

        return sb.ToString();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    /// <summary>
    /// 格式化开奖号码
    /// </summary>
    /// <param name="LotteryID"></param>
    /// <param name="winNumber"></param>
    /// <returns></returns>
    private string FormatWinNumber(string winNumber)
    {
        StringBuilder sb = new StringBuilder();

        if (winNumber.IndexOf(" + ") > 0)
        {
            winNumber = winNumber.Replace(" + ", " ");

            string[] number = winNumber.Split(' ');

            for (int i = 0; i < number.Length; i++)
            {
                if (i < number.Length - 1)
                {
                    sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(../Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                }
                else
                {
                    sb.Append("</td><td align='center' class='white14' style='width: 25px;background-image: url(../Home/Room/Images/zfb_blueball.gif)'>").Append(number[i]);
                }
            }
        }

        return sb.ToString();
    }

    private int GetWinNumberCellNumber(int LotteryID, string winNumber)
    {
        int cellNumber = 0;
        if (winNumber.IndexOf(" + ") > 0)
        {
            winNumber = winNumber.Replace(" + ", " ");

            string[] number = winNumber.Split(' ');

            cellNumber = number.Length;
        }

        return cellNumber + 1;
    }

    /// <summary>
    /// 获取服务器时间
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string GetSysTime()
    {
        DateTime now = Shove._Convert.StrToDateTime(new DAL.Views.V_GetDate().Open("", "", "").Rows[0]["Now"].ToString(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

        return now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    /// <summary>
    /// 获取期信息
    /// </summary>
    /// <param name="LotteryID"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string GetIsuseInfo(int LotteryID)
    {
        try
        {
            DataTable dt = DataCache.GetIsusesInfo(LotteryID);
            string strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //本期期信息
            DataRow[] drCurrIsuse = dt.Select("'" + strNow + "' > StartTime and '" + strNow + "' < EndTime", "EndTime desc");
            //上期期信息（已开奖）
            DataRow[] drPreIsuse = dt.Select("EndTime < '" + strNow + "' and WinLotteryNumber<>''", "EndTime desc");

            if (drCurrIsuse.Length == 0 && drPreIsuse.Length == 0)
            {
                return "";
            }

            if (drCurrIsuse.Length == 0)
            {
                drCurrIsuse = dt.Select("EndTime < '" + strNow + "'", "EndTime desc");
            }

            if (drPreIsuse.Length == 0)
            {
                drPreIsuse = drCurrIsuse;
            }

            //本期
            int IsuseID = Shove._Convert.StrToInt(drCurrIsuse[0]["ID"].ToString(), -1);
            string IsuseName = drCurrIsuse[0]["Name"].ToString();

            int SystemEndAheadMinute = DataCache.LotteryEndAheadMinute[LotteryID];
            DateTime EndTime = Convert.ToDateTime(drCurrIsuse[0]["EndTime"]);
            DateTime SystemEndTime = EndTime.AddMinutes(SystemEndAheadMinute * -1);

            string IsuseEndTime = SystemEndTime.ToString("yyyy/MM/dd HH:mm:ss");
            //END

            //上期
            string LastIsuseName = drPreIsuse[0]["Name"].ToString();
            string LastWinNumber = drPreIsuse[0]["WinLotteryNumber"].ToString().Trim();

            LastWinNumber = FormatWinNumber(LastWinNumber);

            //END

            StringBuilder sb = new StringBuilder();

            sb.Append(IsuseID.ToString())
                .Append(",")
                .Append(IsuseName)
                .Append(",")
                .Append(IsuseEndTime)
                .Append("|<table  cellspacing='5' cellpadding='0' style='text-align: center; font-weight: bold;'><tr><td align='left'  height='25' class='hui12'>")
                .Append(LastIsuseName)
                .Append("&nbsp;期开奖:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
                .Append(LastWinNumber)
                .Append("</td>");

            string Total = GetTotalMoney(IsuseID.ToString());

            if (Total != "")
            {
                double winRate = (Shove._Convert.StrToDouble(Total, 0) / 5000000);
                double rate = Math.Floor(winRate);
                sb.Append("<tr style='font-weight:normal;'><td colspan='" + GetWinNumberCellNumber(LotteryID, drPreIsuse[0]["WinLotteryNumber"].ToString().Trim()) + "' align='left'><span class=\"hui12\">奖池累积奖金已达</span><span class='red12' style='font-weight: bold;'>" + Total + "</span><span class=\"hui12\">元</span>");
                if (rate > 0)
                {
                    sb.Append("<span class=\"hui12\">，可开出</span><span class='red12' style='font-weight: bold;'>" + Math.Floor(winRate) + "</span><span class=\"hui12\">个足额500万</span>");
                }
                sb.Append("</td></tr>");
            }

            sb.Append("</table>")
              .Append("|").Append(GetIsuseChase(LotteryID));

            return sb.ToString();
        }
        catch (Exception e)
        {
            new Log("TWZT").Write(this.GetType() + e.Message);
            return "";
        }
    }

    private string BindWinList(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<tr>")
            .Append("<td width='8%' height='25' bgcolor='#E8E8E8'>")
            .Append("</td>")
            .Append("<td width='31%' bgcolor='#E8E8E8'>")
            .Append("用户名")
            .Append("</td>")
            .Append("<td width='30%' bgcolor='#E8E8E8'>")
            .Append("奖金")
            .Append("</td>")
            .Append("<td width='13%' bgcolor='#E8E8E8'>")
            .Append("跟单")
            .Append("</td>")
            .Append("</tr>");

        int i = 0;

        foreach (DataRow dr in dt.Rows)
        {
            i++;
            string Name = dr["InitiateName"].ToString();
            Name = Name.Length > 5 ? (Name.Substring(0, 4) + "*") : Name;
            sb.Append("<tr><td height='25'><img src='../Home/Room/Images/num_").Append(i.ToString()).Append(".gif'/></td><td class='black12' title='" + dr["InitiateName"].ToString() + "'>")
                .Append("<a href='../Home/Web/Score.aspx?id=" + dr["InitiateUserID"].ToString() + "&LotteryID=" + LotteryID + "'target='_blank'>")
                .Append(Name)
                .Append("</a>")
                .Append("</td><td class='black12'>")
                .Append(Shove._Convert.StrToDouble(dr["Win"].ToString(), 0).ToString("N0"))
        .Append("</td><td class='red12_2'><a href='javascript:;' onclick=\"if(CreateLogin()){followScheme(" + LotteryID + ");$Id('iframeFollowScheme').src='")
                .Append("../Home/Room/FollowFriendSchemeAdd.aspx?LotteryID=")
                .Append(LotteryID.ToString()).Append("&FollowUserID=")
                .Append(dr["InitiateUserID"].ToString()).Append("&FollowUserName=")
               .Append(HttpUtility.UrlEncode(dr["InitiateName"].ToString())).Append("'}\">定制</a></td></tr>");
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取双色球奖池信息
    /// </summary>
    /// <param name="IsuseID"></param>
    /// <returns></returns>
    /// 
    private string GetTotalMoney(string IsuseID)
    {
        string TotalMoneySSQ = "";

        string key = "Home_Room_Buy_GetTotalMoneySSQ_" + IsuseID;

        DataTable dtTotalMoneySSQ = Shove._Web.Cache.GetCacheAsDataTable(key);
        if (dtTotalMoneySSQ == null)
        {
            dtTotalMoneySSQ = new DAL.Tables.T_TotalMoney().Open("", "IsuseID=" + IsuseID, "");

            if (dtTotalMoneySSQ == null)
            {
                new Log("System").Write(this.GetType().FullName + "数据库繁忙，请重试(GetTotalMoneySSQ)");
                return "";
            }
            Shove._Web.Cache.SetCache(key, dtTotalMoneySSQ, 120);
        }
        if (dtTotalMoneySSQ.Rows.Count > 0)
        {
            TotalMoneySSQ = dtTotalMoneySSQ.Rows[0]["TotalMoney"].ToString();
        }

        return TotalMoneySSQ;
    }

    /// <summary>
    /// 追号信息
    /// </summary>
    /// <param name="LotteryID"></param>
    /// <returns></returns>
    private string GetIsuseChase(int LotteryID)
    {
        try
        {
            DataTable dt = DataCache.GetIsusesInfo(LotteryID);

            int SystemEndAheadMinute = DataCache.LotteryEndAheadMinute[LotteryID];

            DataRow[] drCanChase = dt.Select("('" + DateTime.Now.ToString() + "' < StartTime or ('" + DateTime.Now.ToString() + "'>StartTime and '" + DateTime.Now.AddMinutes(SystemEndAheadMinute).ToString() + "'<EndTime))", "EndTime");

            if (drCanChase.Length == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            int row = 0;
            sb.Append("<table cellpadding='0' cellspacing='1' style='width: 100%; text-align: center; background-color: #E2EAED;'><tbody style='background-color: White;'>");
            foreach (DataRow dr in drCanChase)
            {
                row++;
                sb.Append("<tr>")
                .Append("<td style='width: 10%;'>")
                .Append("<input id='check").Append(dr["ID"].ToString()).Append("' type='checkbox' name='check").Append(dr["ID"].ToString()).Append("' onclick='check(this);' value='").Append(dr["ID"].ToString()).Append("'/>")
                .Append("</td>")
                .Append("<td style='width: 40%;'>")
                .Append("<span>").Append(dr["Name"].ToString()).Append("</span>")
                .Append("<span>").Append(row == 1 ? "<font color='red'>[本期]</font>" : row == 2 ? "<font color='red'>[下期]</font>" : "").Append("</span>")
                .Append("</td>")
                .Append("<td style='width: 20%;'>")
                .Append("<input name='times").Append(dr["ID"].ToString()).Append("' type='text' value='1' id='times").Append(dr["ID"].ToString()).Append("' class='TextBox' onchange='onTextChange(this);' onkeypress='return InputMask_Number();' disabled='disabled' onblur='CheckMultiple2(this);' style='width: 45px;' />倍")
                .Append("</td>")
                .Append("<td style='width: 30%;'>")
                .Append("<input name='money").Append(dr["ID"].ToString()).Append("' type='text' value='0' id='money").Append(dr["ID"].ToString()).Append("' class='TextBox' disabled='disabled' style='width: 45px;' />元")
                .Append("</td>")
                .Append("</tr>");
            }
            sb.Append("</tbody></table>");

            return sb.ToString();
        }
        catch (Exception e)
        {
            new Log("TWZT").Write("AlipayTask Running Error: " + e.Message);
            return "";
        }
    }

    /// <summary>
    /// 购买彩票
    /// </summary>
    /// <param name="_User"></param>
    private void Buy(Users _User)
    {
        string HidIsuseID = Shove._Web.Utility.GetRequest("HidIsuseID");
        string HidIsuseEndTime = Shove._Web.Utility.GetRequest("HidIsuseEndTime");
        string playType = Shove._Web.Utility.GetRequest("tbPlayTypeID");
        string Chase = Shove._Web.Utility.GetRequest("Chase");
        string CoBuy = Shove._Web.Utility.GetRequest("CoBuy");
        string tb_Share = Shove._Web.Utility.GetRequest("tb_Share");
        string tb_BuyShare = Shove._Web.Utility.GetRequest("tb_BuyShare");
        string tb_AssureShare = Shove._Web.Utility.GetRequest("tb_AssureShare");
        string tb_OpenUserList = "";
        string tb_Title = Shove._Web.Utility.GetRequest("tb_Title");
        string tb_Description = Shove._Web.Utility.GetRequest("tb_Description");
        string tbAutoStopAtWinMoney = Shove._Web.Utility.GetRequest("tbAutoStopAtWinMoney");
        string tbSecrecyLevel = Shove._Web.Utility.GetRequest("SecrecyLevel");
        string tb_LotteryNumber = Shove._Web.Utility.FilteSqlInfusion(Request["tb_LotteryNumber"]);
        string tb_hide_SumMoney = Shove._Web.Utility.GetRequest("tb_hide_SumMoney");
        string tb_hide_AssureMoney = Shove._Web.Utility.GetRequest("tb_hide_AssureMoney");
        string tb_hide_SumNum = Shove._Web.Utility.GetRequest("tb_hide_SumNum");
        string HidIsuseCount = Shove._Web.Utility.GetRequest("HidIsuseCount");
        string HidLotteryID = Shove._Web.Utility.GetRequest("HidLotteryID");
        string HidIsAlipay = Shove._Web.Utility.GetRequest("HidIsAlipay");
        string tb_Multiple = Shove._Web.Utility.GetRequest("tb_Multiple");
        string HidIsuseName = Shove._Web.Utility.GetRequest("HidIsuseName");
        string tbPlayTypeName = Shove._Web.Utility.GetRequest("tbPlayTypeName");
        string ChaseBuyedMoney = Shove._Web.Utility.GetRequest("tb_hide_ChaseBuyedMoney");
        string tb_SchemeBonusScale = Shove._Web.Utility.GetRequest("tb_SchemeBonusScale");
        string tb_SchemeBonusScalec = Shove._Web.Utility.GetRequest("tb_SchemeBonusScalec");

        int Price = 2;

        if (tb_Multiple == "")
        {
            tb_Multiple = "1";
        }

        double SumMoney = 0;
        int Share = 0;
        int BuyShare = 0;
        double AssureMoney = 0;
        int Multiple = 0;
        int SumNum = 0;
        short SecrecyLevel = 0;
        int PlayTypeID = 0;
        int LotteryID = 0;
        long IsuseID = 0;
        double AutoStopAtWinMoney = 0;
        double SchemeBonusScale = 0;
        double SchemeBonusScalec = 0;

        try
        {
            SumMoney = double.Parse(tb_hide_SumMoney);
            Share = int.Parse(tb_Share);
            BuyShare = int.Parse(tb_BuyShare);
            AssureMoney = double.Parse(tb_hide_AssureMoney);
            Multiple = int.Parse(tb_Multiple);
            SumNum = int.Parse(tb_hide_SumNum);
            SecrecyLevel = short.Parse(tbSecrecyLevel);
            PlayTypeID = int.Parse(playType);
            LotteryID = int.Parse(HidLotteryID);
            IsuseID = long.Parse(HidIsuseID);
            AutoStopAtWinMoney = double.Parse(tbAutoStopAtWinMoney);
            SchemeBonusScale = double.Parse(tb_SchemeBonusScale);
            SchemeBonusScalec = double.Parse(tb_SchemeBonusScalec);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if ((SumMoney <= 0) || (SumNum < 1))
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if (AssureMoney < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if (Share < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if ((BuyShare == Share) && (AssureMoney == 0))
        {
            Share = 1;
            BuyShare = 1;
        }

        if ((SumMoney / Share) < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "每份金额最低不能少于 1 元。");

            return;
        }

        double BuyMoney = BuyShare * (SumMoney / Share) + AssureMoney;

        if (Chase != "")
        {
            BuyMoney = double.Parse(ChaseBuyedMoney);
        }

        if (BuyMoney > _User.Balance)
        {
            SaveDataForAliBuy();

            return;
        }

        if (BuyMoney > PF.SchemeMaxBettingMoney)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注金额不能大于" + PF.SchemeMaxBettingMoney.ToString() + "，谢谢。");

            return;
        }

        if (Multiple > 999)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注倍数不能大于 999 倍，谢谢。");

            return;
        }
        //佣金比例的计算

        if (!(SchemeBonusScale >= 0 || SchemeBonusScale <= 10))
        {
            Shove._Web.JavaScript.Alert(this.Page, "佣金比例只能在0~10之间");

            return;
        }

        if (SchemeBonusScale.ToString().IndexOf("-") > -1 || SchemeBonusScale.ToString().IndexOf(".") > -1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "佣金比例输入有误");

            return;
        }
        if (!(SchemeBonusScalec >= 0 || SchemeBonusScalec <= 10))
        {
            Shove._Web.JavaScript.Alert(this.Page, "佣金比例只能在0~10之间");

            return;
        }

        if (SchemeBonusScalec.ToString().IndexOf("-") > -1 || SchemeBonusScalec.ToString().IndexOf(".") > -1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "佣金比例输入有误");

            return;
        }

        SchemeBonusScale = SchemeBonusScale / 100;
        SchemeBonusScalec = SchemeBonusScalec / 100;

        string LotteryNumber = tb_LotteryNumber;

        if (LotteryNumber[LotteryNumber.Length - 1] == '\n')
        {
            LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
        }

        #region 对彩票号码进行分析，判断注数

        SLS.Lottery slsLottery = new SLS.Lottery();
        string[] t_lotterys = SplitLotteryNumber(LotteryNumber);

        if ((t_lotterys == null) || (t_lotterys.Length < 1))
        {
            Shove._Web.JavaScript.Alert(this.Page, "选号发生异常，请重新选择号码投注，谢谢。(-694)");

            return;
        }

        int ValidNum = 0;

        foreach (string str in t_lotterys)
        {
            string Number = slsLottery[LotteryID].AnalyseScheme(str, PlayTypeID);

            if (string.IsNullOrEmpty(Number))
            {
                continue;
            }

            string[] str_s = Number.Split('|');

            if (str_s == null || str_s.Length < 1)
            {
                continue;
            }

            ValidNum += Shove._Convert.StrToInt(str_s[str_s.Length - 1], 0);
        }

        if (ValidNum != SumNum)
        {
            Shove._Web.JavaScript.Alert(this.Page, "选号发生异常，请重新选择号码投注，谢谢。");

            return;
        }

        #endregion

        StringBuilder ChaseXML = new StringBuilder();
        int RpTodayDataCount = 0;
        string AdditionasXml = "";
        string ReturnDescription = "";

        //追号
        if (Chase == "1")
        {
            foreach (string key in Request.Form.AllKeys)
            {
                if (key.IndexOf("check") > -1)
                {
                    int row = Shove._Convert.StrToInt(key.Replace("check", ""), -1);
                    if (row > 0)
                    {
                        RpTodayDataCount++;
                        int money = Shove._Convert.StrToInt(Request.Form["tb_hide_SumNum"], -1) * Price * Shove._Convert.StrToInt(Request.Form["times" + row.ToString()], -1);
                        ChaseXML.Append(Request.Form[key]).Append(",")
                            .Append(Request.Form["times" + row.ToString()]).Append(",")
                            .Append(money.ToString()).Append(";");
                    }
                }
            }

            if (ChaseXML.Length > 0)
            {
                ChaseXML.Remove(ChaseXML.Length - 1, 1);
            }

            if (LotteryNumber[LotteryNumber.Length - 1] == '\n')
            {
                LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
            }

            try
            {
                SumMoney = double.Parse(tb_hide_SumMoney);
            }
            catch
            {
                Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。(-1325)");

                return;
            }

            if (SumMoney < 2)
            {
                Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。(-1332)");

                return;
            }

            string[] XML = ChaseXML.ToString().Split(';');
            int CompetitionCount = XML.Length;

            string[] Xmlparams = new string[CompetitionCount * 6];

            string str_EndTime = DAL.Functions.F_GetIsuseSystemEndTime(long.Parse(XML[0].Split(',')[0]), PlayTypeID).ToString();
            DateTime EndTime = DateTime.Parse(str_EndTime);

            if (DateTime.Now >= EndTime)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您选择的追号期号中包含已截止的期，请重新选择。");

                return;
            }

            //构建格式：期号,玩法类别,方案,倍数,金额,方案保密级别
            for (int i = 0; i < CompetitionCount; i++)
            {
                Xmlparams[i * 6] = XML[i].Split(',')[0];        //期号
                Xmlparams[i * 6 + 1] = PlayTypeID.ToString();   //玩法类别
                Xmlparams[i * 6 + 2] = LotteryNumber;           //方案
                Xmlparams[i * 6 + 3] = XML[i].Split(',')[1];    //倍数
                Xmlparams[i * 6 + 4] = XML[i].Split(',')[2];    //金额
                Xmlparams[i * 6 + 5] = SecrecyLevel.ToString();

                if (Shove._Convert.StrToDouble(Xmlparams[i * 6 + 3], 0) * SumMoney != Shove._Convert.StrToDouble(Xmlparams[i * 6 + 4], 1))
                {
                    Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

                    return;
                }

                if (Shove._Convert.StrToDouble(Xmlparams[i * 6 + 3], 0) < Multiple)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "追号倍数有错误，请仔细检查！");

                    return;
                }

                if (double.Parse(Xmlparams[i * 6 + 3]) * SumNum * Price != double.Parse(Xmlparams[i * 6 + 4]))
                {
                    Shove._Web.JavaScript.Alert(this.Page, "追号金额有错误，请仔细检查！可能原因：浏览器不兼容，建议使用IE 7.0");

                    return;
                }
            }

            AdditionasXml = PF.BuildIsuseAdditionasXmlForChase(Xmlparams);

            if (AdditionasXml == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "追号发生错误。");

                return;
            }

            if (_User.InitiateChaseTask(tb_Title.Trim(), tb_Description.Trim(), LotteryID, AutoStopAtWinMoney, AdditionasXml, LotteryNumber, SchemeBonusScalec, ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName + "(-754)");

                return;
            }

            Shove._Web.Cache.ClearCache("Home_Room_CoBuy_BindDataForType" + IsuseID.ToString());
            Shove._Web.Cache.ClearCache("Home_Room_SchemeAll_BindData" + IsuseID.ToString());
            Shove._Web.Cache.ClearCache(_Site.ID.ToString() + "AccountFreezeDetail_" + _User.ID.ToString());

            Response.Redirect("../Home/Room/UserBuySuccess.aspx?LotteryID=" + LotteryID.ToString() + "&Type=2&Money=" + BuyMoney.ToString() + "");

            return;
        }
        else
        {
            if (DateTime.Now >= Shove._Convert.StrToDateTime(HidIsuseEndTime.Replace("/", "-"), DateTime.Now.AddDays(-1).ToString()))
            {
                Shove._Web.JavaScript.Alert(this.Page, "本期投注已截止，谢谢。");

                return;
            }

            if (Price * SumNum * Multiple != SumMoney)
            {
                Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

                return;
            }

            long SchemeID = _User.InitiateScheme(IsuseID, PlayTypeID, tb_Title.Trim() == "" ? "(无标题)" : tb_Title.Trim(), tb_Description.Trim(), LotteryNumber, "", Multiple, SumMoney, AssureMoney, Share, BuyShare, tb_OpenUserList.Trim(), short.Parse(SecrecyLevel.ToString()), SchemeBonusScale, ref ReturnDescription);
            if (SchemeID < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName + "(-755)");

                return;
            }

            Shove._Web.Cache.ClearCache("Home_Room_CoBuy_BindDataForType" + IsuseID.ToString());
            Shove._Web.Cache.ClearCache("Home_Room_SchemeAll_BindData" + IsuseID.ToString());

            if (SumMoney > 50 && Share > 1)
            {
                Shove._Web.Cache.ClearCache("Home_Room_JoinAllBuy_BindData");
            }

            Response.Redirect("../Home/Room/UserBuySuccess.aspx?LotteryID=" + LotteryID.ToString() + "&&Money=" + BuyMoney.ToString() + "&SchemeID=" + SchemeID.ToString() + "");

            return;
        }
    }

    /// <summary>
    /// 分析方案
    /// </summary>
    /// <param name="Content"></param>
    /// <param name="LotteryID"></param>
    /// <param name="PlayTypeID"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string AnalyseScheme(string Content, string LotteryID, int PlayTypeID)
    {
        string Result = new SLS.Lottery()[Shove._Convert.StrToInt(LotteryID, 0)].AnalyseScheme(Content, PlayTypeID);

        return Result.Trim();
    }

    /// <summary>
    /// 余额不足时，保存数据
    /// </summary>
    private void SaveDataForAliBuy()
    {
        string HidIsuseID = Shove._Web.Utility.GetRequest("HidIsuseID");
        string HidIsuseEndTime = Shove._Web.Utility.GetRequest("HidIsuseEndTime");
        string playType = Shove._Web.Utility.GetRequest("tbPlayTypeID");
        string Chase = Shove._Web.Utility.GetRequest("Chase");
        string Cobuy = Shove._Web.Utility.GetRequest("Cobuy");
        string tb_Share = Shove._Web.Utility.GetRequest("tb_Share");
        string tb_BuyShare = Shove._Web.Utility.GetRequest("tb_BuyShare");
        string tb_AssureShare = Shove._Web.Utility.GetRequest("tb_AssureShare");
        string tb_OpenUserList = Shove._Web.Utility.GetRequest("tb_OpenUserList");
        string tb_Title = Shove._Web.Utility.GetRequest("tb_Title");
        string tb_Description = Shove._Web.Utility.GetRequest("tb_Description");
        string tbAutoStopAtWinMoney = Shove._Web.Utility.GetRequest("tbAutoStopAtWinMoney");
        string tbSecrecyLevel = Shove._Web.Utility.GetRequest("SecrecyLevel");
        string tbPlayTypeName = Shove._Web.Utility.GetRequest("tbPlayTypeName");
        string tb_LotteryNumber = Shove._Web.Utility.FilteSqlInfusion(Request["tb_LotteryNumber"]);
        string tb_hide_SumMoney = Shove._Web.Utility.GetRequest("tb_hide_SumMoney");
        string tb_hide_AssureMoney = Shove._Web.Utility.GetRequest("tb_hide_AssureMoney");
        string tb_hide_SumNum = Shove._Web.Utility.GetRequest("tb_hide_SumNum");
        string HidIsuseCount = Shove._Web.Utility.GetRequest("HidIsuseCount");
        string HidLotteryID = Shove._Web.Utility.GetRequest("HidLotteryID");
        string HidIsAlipay = Shove._Web.Utility.GetRequest("HidIsAlipay");
        string tb_Multiple = Shove._Web.Utility.GetRequest("tb_Multiple");
        string AdditionasXml = "";
        StringBuilder ChaseXML = new StringBuilder();
        int RpTodayDataCount = 0;

        int Price = 2;

        if (tb_Multiple == "")
        {
            tb_Multiple = "1";
        }

        if (Chase == "1")
        {
            foreach (string key in Request.Form.AllKeys)
            {
                if (key.IndexOf("check") > -1)
                {
                    int row = Shove._Convert.StrToInt(key.Replace("check", ""), -1);
                    if (row > 0)
                    {
                        RpTodayDataCount++;
                        int money = Shove._Convert.StrToInt(tb_hide_SumNum, -1) * Price * Shove._Convert.StrToInt(Request.Form["times" + row.ToString()], -1);
                        ChaseXML.Append(Request.Form[key]).Append(",")
                            .Append(Request.Form["times" + row.ToString()]).Append(",")
                            .Append(money.ToString()).Append(";");
                    }
                }
            }

            string LotteryNumber = tb_LotteryNumber;

            if (ChaseXML.Length > 0)
            {
                ChaseXML.Remove(ChaseXML.Length - 1, 1);
            }

            if (LotteryNumber[LotteryNumber.Length - 1] == '\n')
            {
                LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
            }

            string[] XML = ChaseXML.ToString().Split(';');
            int CompetitionCount = XML.Length;

            string[] Xmlparams = new string[CompetitionCount * 6];

            string str_EndTime = DAL.Functions.F_GetIsuseSystemEndTime(long.Parse(XML[0].Split(',')[0]), int.Parse(playType)).ToString();
            DateTime EndTime = DateTime.Parse(str_EndTime);

            if (DateTime.Now >= EndTime)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您选择的追号期号中包含已截止的期，请重新选择。");

                return;
            }

            //构建格式：期号,玩法类别,方案,倍数,金额,方案保密级别
            for (int i = 0; i < CompetitionCount; i++)
            {
                Xmlparams[i * 6] = XML[i].Split(',')[0];        //期号
                Xmlparams[i * 6 + 1] = playType;   //玩法类别
                Xmlparams[i * 6 + 2] = LotteryNumber;           //方案
                Xmlparams[i * 6 + 3] = XML[i].Split(',')[1];    //倍数
                Xmlparams[i * 6 + 4] = XML[i].Split(',')[2];    //金额
                Xmlparams[i * 6 + 5] = tbSecrecyLevel;
            }

            AdditionasXml = PF.BuildIsuseAdditionasXmlForChase(Xmlparams);
        }

        DAL.Tables.T_AlipayBuyTemp tbp = new DAL.Tables.T_AlipayBuyTemp();

        tbp.SiteID.Value = 1;
        tbp.UserID.Value = -1;
        tbp.Money.Value = tb_hide_SumMoney;
        tbp.HandleResult.Value = 0;
        tbp.IsChase.Value = Chase == "1";
        tbp.IsCoBuy.Value = Cobuy == "2";
        tbp.LotteryID.Value = HidLotteryID;
        tbp.IsuseID.Value = HidIsuseID;
        tbp.PlayTypeID.Value = playType;
        tbp.StopwhenwinMoney.Value = tbAutoStopAtWinMoney;
        tbp.AdditionasXml.Value = AdditionasXml;
        tbp.Title.Value = tb_Title;
        tbp.Description.Value = tb_Description;
        tbp.LotteryNumber.Value = tb_LotteryNumber;
        tbp.UpdateloadFileContent.Value = "";
        tbp.Multiple.Value = tb_Multiple;
        tbp.BuyMoney.Value = tb_BuyShare;
        tbp.SumMoney.Value = tb_hide_SumMoney;
        tbp.AssureMoney.Value = tb_hide_AssureMoney;
        tbp.Share.Value = tb_Share;
        tbp.BuyShare.Value = tb_BuyShare;
        tbp.AssureShare.Value = tb_AssureShare;
        tbp.OpenUsers.Value = tb_OpenUserList;
        tbp.SecrecyLevel.Value = tbSecrecyLevel;

        long Result = tbp.Insert();

        if (Result < 0)
        {
            new Log("System").Write("T_AlipayBuyTemp 数据库读写错误。");
        }

        Shove._Web.JavaScript.Alert(this.Page, "您的账户余额不足，请先充值，谢谢。", "../Home/Room/OnlinePay/Default.aspx?BuyID=" + Result.ToString());
    }

    /// <summary>
    /// 充值成功后，绑定数据
    /// </summary>
    /// <param name="BuyID"></param>
    private void BindDataForAliBuy(long BuyID)
    {
        DataTable dt = new DAL.Tables.T_AlipayBuyTemp().Open("", "ID=" + BuyID.ToString(), "");

        if (dt == null || dt.Rows.Count == 0)
        {
            return;
        }

        DataRow dr = dt.Rows[0];

        string HidIsuseID = dr["IsuseID"].ToString();
        string playType = dr["PlayTypeID"].ToString();
        bool IsChase = Shove._Convert.StrToBool(dr["IsChase"].ToString(), false);
        bool IsCoBuy = Shove._Convert.StrToBool(dr["IsCoBuy"].ToString(), false);
        string tb_Share = dr["Share"].ToString();
        string tb_BuyShare = dr["BuyShare"].ToString();
        string tb_AssureShare = dr["AssureShare"].ToString();
        string tb_OpenUserList = dr["OpenUsers"].ToString();
        string tb_Title = dr["Title"].ToString();
        string tb_Description = dr["Description"].ToString();
        string tbAutoStopAtWinMoney = dr["StopwhenwinMoney"].ToString();
        string tbSecrecyLevel = dr["SecrecyLevel"].ToString();
        string tb_LotteryNumber = dr["LotteryNumber"].ToString();
        string tb_hide_SumMoney = dr["SumMoney"].ToString();
        string tb_hide_AssureMoney = dr["AssureMoney"].ToString();
        string HidLotteryID = dr["LotteryID"].ToString();
        string tb_Multiple = dr["Multiple"].ToString();
        string AdditionasXml = dr["AdditionasXml"].ToString();

        if (tb_Multiple == "")
        {
            tb_Multiple = "1";
        }

        double SumMoney = 0;
        int Share = 0;
        int BuyShare = 0;
        double AssureMoney = 0;
        int Multiple = 0;
        short SecrecyLevel = 0;
        int PlayTypeID = 0;
        int LotteryID = 0;
        long IsuseID = 0;
        double AutoStopAtWinMoney = 0;

        try
        {
            SumMoney = double.Parse(tb_hide_SumMoney);
            Share = int.Parse(tb_Share);
            BuyShare = int.Parse(tb_BuyShare);
            AssureMoney = double.Parse(tb_hide_AssureMoney);
            Multiple = int.Parse(tb_Multiple);
            SecrecyLevel = short.Parse(tbSecrecyLevel);
            PlayTypeID = int.Parse(playType);
            LotteryID = int.Parse(HidLotteryID);
            IsuseID = long.Parse(HidIsuseID);
            AutoStopAtWinMoney = double.Parse(tbAutoStopAtWinMoney);
        }
        catch { }

        if ((BuyShare == Share) && (AssureMoney == 0))
        {
            Share = 1;
            BuyShare = 1;
        }

        double BuyMoney = BuyShare * (SumMoney / Share) + AssureMoney;

        if (IsChase)
        {
            BuyMoney = double.Parse(tb_hide_SumMoney);
        }

        string LotteryNumber = tb_LotteryNumber;

        if (LotteryNumber[LotteryNumber.Length - 1] == '\n')
        {
            LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
        }

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<script type='text/javascript' defer='defer'>");

        sb.Append("$Id('playType").Append(PlayTypeID.ToString()).AppendLine("').checked = true;");
        sb.AppendLine("clickPlayType('" + PlayTypeID.ToString() + "');");
        sb.AppendLine("function BindDataForFromAli(){");

        //追号
        if (IsChase)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(AdditionasXml);
            XmlNodeList xnl = xml.ChildNodes[0].ChildNodes;

            foreach (XmlNode xn in xnl)
            {
                sb.Append("$Id('times").Append(xn.Attributes["IsuseID"].Value).Append("').value = '").Append(xn.Attributes["Multiple"].Value).AppendLine("';");
                sb.Append("$Id('money").Append(xn.Attributes["IsuseID"].Value).Append("').value = '").Append(xn.Attributes["Money"].Value).AppendLine("';");
                sb.Append("$Id('check").Append(xn.Attributes["IsuseID"].Value).AppendLine("').checked = true;");
                sb.Append("$Id('times").Append(xn.Attributes["IsuseID"].Value).AppendLine("').disabled = '';");
            }
        }

        LotteryNumber = LotteryNumber.Replace("\r", "");
        int LotteryNum = 0;
        string Number = "";
        foreach (string lotteryNumber in LotteryNumber.Split('\n'))
        {
            LotteryNum += new SLS.Lottery()[LotteryID].ToSingle(lotteryNumber, ref Number, PlayTypeID).Length;
            sb.AppendLine("var option = document.createElement('option');");
            sb.AppendLine("$Id('list_LotteryNumber').options.add(option);");
            sb.Append("option.innerText = '").Append(lotteryNumber).AppendLine("';");
            sb.Append("option.value = '").Append(lotteryNumber).AppendLine("';");
        }

        if (IsChase)
        {
            sb.AppendLine("$Id('Chase').checked = true;");
            sb.AppendLine("oncbInitiateTypeClick($Id('Chase'));");
        }

        if (IsCoBuy)
        {
            sb.AppendLine("$Id('CoBuy').checked = true;");
            sb.AppendLine("oncbInitiateTypeClick($Id('CoBuy'));");
        }

        sb.Append("$Id('tb_LotteryNumber').value = '").Append(tb_LotteryNumber.Replace("\r", "\\r").Replace("\n", "\\n")).AppendLine("';");
        sb.Append("$Id('tb_Share').value = '").Append(tb_Share).AppendLine("';");
        sb.Append("$Id('tb_BuyShare').value = '").Append(tb_BuyShare).AppendLine("';");
        sb.Append("$Id('tb_AssureShare').value = '").Append(tb_AssureShare).AppendLine("';");
        sb.Append("$Id('tb_OpenUserList').value = '").Append(tb_OpenUserList).AppendLine("';");
        sb.Append("$Id('tb_Title').value = '").Append(tb_Title).AppendLine("';");
        sb.Append("$Id('tb_Description').value = '").Append(tb_Description.Replace("\r", "\\r").Replace("\n", "\\n")).AppendLine("';");
        sb.Append("$Id('tbAutoStopAtWinMoney').value = '").Append(tbAutoStopAtWinMoney).AppendLine("';");
        sb.Append("$Id('SecrecyLevel").Append(SecrecyLevel.ToString()).AppendLine("').checked = true;");
        sb.Append("$Id('tb_hide_SumMoney').value = '").Append(tb_hide_SumMoney).AppendLine("';");
        sb.Append("$Id('tb_hide_AssureMoney').value = '").Append(AssureMoney.ToString("N0")).AppendLine("';");
        sb.Append("$Id('tb_Multiple').value = '").Append(Multiple.ToString()).AppendLine("';");
        sb.Append("$Id('lab_AssureMoney').innerText = '").Append(AssureMoney.ToString("N0")).AppendLine("';");
        sb.Append("$Id('lab_SumMoney').innerText = '").Append(SumMoney.ToString("N0")).AppendLine("';");
        sb.Append("$Id('LbSumMoney').innerText = '").Append(SumMoney.ToString("N0")).AppendLine("';");
        sb.Append("$Id('lab_Num').innerText = '").Append(LotteryNum.ToString()).AppendLine("';");
        sb.AppendLine("CalcResult();");
        sb.AppendLine("}");
        sb.AppendLine("</script>");

        script = sb.ToString();
    }

    private string[] SplitLotteryNumber(string Number)
    {
        string[] s = Number.Split('\n');
        if (s.Length == 0)
            return null;
        for (int i = 0; i < s.Length; i++)
            s[i] = s[i].Trim();
        return s;
    }

    protected void btn_OK_Click(object sender, EventArgs e)
    {
        Buy(_User);
    }

    /// <summary> 中奖注数播放
    /// 
    /// </summary>
    /// <param name="LotteryID">彩种编号</param>
    /// 

    private void BindWinNumber(int LotteryID)
    {
        string Key = "Home_Room_Buy_BindWinNumber" + LotteryID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select  a.WinDescription ,b.Name from T_Schemes as a INNER JOIN T_Isuses as b on a.IsuseID =b.ID where IsuseID = (SELECT TOP 1 ID FROM dbo.T_Isuses WHERE LotteryID=" + LotteryID.ToString() + " AND GETDATE()>EndTime and IsOpened = 1 AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC) and ISNULL(WinDescription,'')<>'' and WinMoney > 0");
           
            if (dt == null)
            {
                new Log("BindWinNumber方法出错");

                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache(Key,dt, 600);
            }
        }

        StringBuilder sb = new StringBuilder();

        if (dt.Rows.Count == 0)
        {
            sb.Append("<tr>")
                .Append("<td align=\"center\" class=\"blue12\" width=\"218\">")
                .Append("暂无数据")
                .Append("</td>")
                .Append("</tr>");
        }

        else
        {
            sb.AppendLine("<div id='divwen' style='overflow: hidden;height:22px;'>");
            foreach (DataRow dr in GetWinNumberLotteryID(LotteryID).Rows)
            {
                int count = 0;

                DataRow[] drs = dt.Select("WinDescription like '%" + dr["Name"].ToString() + "%'");
                string[] winDescription = null;
                foreach (DataRow r in drs)
                {
                    winDescription = r["WinDescription"].ToString().Trim().Replace("，", ",").Split(',');

                    foreach (string s in winDescription)
                    {
                        if (s.Contains(dr["Name"].ToString()))
                        {
                            count += Shove._Convert.StrToInt(Regex.Replace(s.Trim(), @"[^\d.\d]", ""), 0);
                        }
                    }
                }

                if (count > 0)
                {
                    sb.AppendLine("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"  style='padding-left:5px;margin-top:1px;'>")
                       .Append("<tr>")
                       .AppendLine("<td width=\"1%\" height=\"22\" class=\"hui14\">&#9642;</td>")
                     .Append("<td align=\"center\" class=\"hui12\" height=\"22\" width=\"58%\">")
                     .Append(dt.Rows[0]["Name"].ToString())
                     .Append("&nbsp期中出")
                     .Append("</td>")
                     .Append("<td align=\"center\" class=\"blue12\" width=\"28%\">")
                     .Append(dr["Name"].ToString())
                     .Append("</td>")
                     .Append("<td width=\"10%\">")
                     .Append("<font color=\"red\">")
                     .Append(count.ToString())
                     .Append("</font>")
                     .Append("</td>")
                     .Append("<td width=\"5%\" class=\"hui12\" >")
                     .Append("注&nbsp")
                     .Append("</td>")
                     .Append("</tr>")
                     .Append("</table>");

                }

            }
        }
        sb.Append("</div>");
        divNumber.InnerHtml = sb.ToString();
    }

    private DataTable GetWinNumberLotteryID(int LotteryID)
    {
        string Key = "Home_Room_Buy_GetWinNumberLotteryID" + LotteryID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            dt = new DAL.Tables.T_WinTypes().Open("Name", "LotteryID=" + LotteryID.ToString(), "ID desc");

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
        }

        Shove._Web.Cache.SetCache(Key,dt, 360000);

        return dt;
    }


    /// <summary>
    /// 获得佣金比例
    /// </summary>
    /// <returns>返回佣金比例</returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetSchemeBonusScalec(int lotteryId)
    {
        string bScalec;
        //获得站点选项中的佣金比率
        DataTable dt = new DAL.Tables.T_Sites().Open("Opt_InitiateSchemeBonusScale,Opt_InitiateSchemeLimitLowerScaleMoney,Opt_InitiateSchemeLimitLowerScale,Opt_InitiateSchemeLimitUpperScaleMoney,Opt_InitiateSchemeLimitUpperScale", "", "");
        //把佣金比率换成整数
        bScalec = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeBonusScale"].ToString(), 0) * 100).ToString();

        //发起方案条件
        string Opt_InitiateSchemeLimitLowerScaleMoney = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitLowerScaleMoney"].ToString(), 100)).ToString();
        string Opt_InitiateSchemeLimitLowerScale = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitLowerScale"].ToString(), 0.2)).ToString();
        string Opt_InitiateSchemeLimitUpperScaleMoney = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitUpperScaleMoney"].ToString(), 10000)).ToString();
        string Opt_InitiateSchemeLimitUpperScale = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitUpperScale"].ToString(), 0.05)).ToString();

        string lotteryName = DataCache.Lotteries[lotteryId];

        return bScalec + "|" + Opt_InitiateSchemeLimitLowerScaleMoney + "|" + Opt_InitiateSchemeLimitLowerScale + "|" + Opt_InitiateSchemeLimitUpperScaleMoney + "|" + Opt_InitiateSchemeLimitUpperScale + "|" + lotteryId.ToString() + "|" + lotteryName;
    }

    private string InitLuckLotteryNumber()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">")
            .AppendLine("<tr>")
            .AppendLine("<td height=\"22\" align=\"center\">")
            .AppendLine("&nbsp;</td>");

        for (int i = 0; i < 6; i++)
        {
            sb.AppendLine("<td align=\"center\">")
                .AppendLine("<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">")
                .AppendLine("<tr>")
                .AppendLine("<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.ToString() + "'>")
                .AppendLine("-")
                .AppendLine("</td></tr></table></td>");
        }
        for (int i = 0; i < 1; i++)
        {
            sb.AppendLine("<td align=\"center\">")
                .AppendLine("<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">")
                .AppendLine("<tr>")
                .AppendLine("<td height=\"22\" align=\"center\" class=\"blue12\" id='tdLuckNumber" + (6 + i).ToString() + "'>")
                .AppendLine("-")
                .AppendLine("</td></tr></table></td>");
        }

        sb.AppendLine("<td>&nbsp;</td>")
            .AppendLine("</tr>")
            .AppendLine("</table>");

        return sb.ToString();
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string GenerateLuckLotteryNumber(int LotteryID, string Type, string Name)
    {
        string Key = "Home_Room_Buy_GenerateLuckLotteryNumber" + LotteryID.ToString();

        Type = Shove._Web.Utility.FilteSqlInfusion(Type);
        Name = Shove._Web.Utility.FilteSqlInfusion(Name);

        if (Type == "3")
        {
            try
            {
                DateTime time = Convert.ToDateTime(Name);
                Name = time.ToString("yyyy-MM-dd");

                if (time > DateTime.Now)
                {
                    return "出生日期不能超过当前日期！";
                }
            }
            catch
            {
                return "日期格式不正确！";
            }
        }

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null || dt.Rows.Count == 0)
        {
            dt = new DAL.Tables.T_LuckNumber().Open("", "datediff(d,getdate(),DateTime)=0 and LotteryID=" + LotteryID.ToString() + "", "");

            Shove._Web.Cache.SetCache(Key, dt, 3600);
        }

        string LotteryNumber = "";

        DataRow[] dr = dt.Select("Type=" + Type + " and Name='" + Name + "'");

        if (dr != null && dr.Length > 0)
        {
            LotteryNumber = dr[0]["LotteryNumber"].ToString();
        }
        else
        {
            LotteryNumber = new SLS.Lottery()[LotteryID].BuildNumber(6, 1, 1);

            DAL.Tables.T_LuckNumber ln = new DAL.Tables.T_LuckNumber();

            ln.LotteryID.Value = LotteryID;
            ln.LotteryNumber.Value = LotteryNumber;
            ln.Name.Value = Name;
            ln.Type.Value = Type;

            ln.Insert();
            ln.Delete("datediff(d,DateTime,getdate())>0");

            Shove._Web.Cache.ClearCache(Key);
        }

        return LotteryNumber + "|" + FormatLuckLotteryNumber(LotteryID, LotteryNumber);
    }

    private string FormatLuckLotteryNumber(int LotteryID, string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        LotteryNumber = LotteryNumber.Replace(" + ", " ");

        return LotteryNumber;
    }
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string GetFCExpertList(int lotteryID)
    {
        DataTable dtExpert = GetFCExpertListCache(lotteryID);

        StringBuilder sb = new StringBuilder();

        if (dtExpert != null && dtExpert.Rows.Count > 0)
        {
            int i = 1;
            foreach (DataRow dr in dtExpert.Rows)
            {
                string Name = dr["UserName"].ToString();
                Name = Name.Length > 6 ? (Name.Substring(0, 5) + "*") : Name;

                sb.AppendLine("<tr align=\"center\">")
                .AppendLine("<td  height=\"22\"  bgcolor=\"#FFFFFF\" class=\"black12\" title='" + dr["UserName"].ToString() + "'>")
                .Append("<a href='..\\Home\\Web\\Score.aspx?id=")
                .Append(dr["UserID"].ToString())
                .Append("&LotteryID=")
                .Append(lotteryID + "' target='_blank'>")
                .AppendLine(Name)
                .Append("</a>")
                .AppendLine("</td>")
                .AppendLine("<td  align=\"center\" bgcolor=\"#FFFFFF\" class=\"black12\">")
                .AppendLine(dr["LotteryName"].ToString())
                .Append("</td>")
                .Append("<td bgcolor=\"#FFFFFF\" class='red12_2'><a href='javascript:;' onclick=\"if(CreateLogin()){followScheme(" + LotteryID + ");$Id('iframeFollowScheme').src='")
                .Append("../Home/Room/FollowFriendSchemeAdd.aspx?LotteryID=")
                .Append(lotteryID).Append("&FollowUserID=")
                .Append(dr["UserID"]).Append("&FollowUserName=")
                .Append(HttpUtility.UrlEncode(dr["UserName"].ToString())).Append("'}\">定制</a></td></tr>");

                if (i % 10 == 0 && i != dtExpert.Rows.Count)
                {
                    sb.Append("|");
                }

                i++;

            }

        }
        else
        {
            sb.AppendLine("<tr>")
             .AppendLine("<td colspan=\"3\" height=\"22\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"blue12\">")
             .AppendLine("<span style=\"color:red;\">暂无数据</span>")
             .AppendLine("</td>");
        }

        return sb.ToString();
    }

    public DataTable GetFCExpertListCache(int lID)
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("FCExpert" + lID);

        if (dt == null || dt.Rows.Count == 0)
        {
            dt = new DAL.Views.V_Experts().Open("UserName,UserID,LotteryName,LotteryID", "[ON]=1 and [isCommend]=1 and LotteryID =" + lID, "");

            if (dt != null)
            {
                dt.Columns.Add("ID", typeof(int));

                int i = 1;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["ID"] = i;

                    i++;
                }

                Shove._Web.Cache.SetCache("FCExpert" + lID, dt, 60);
            }
        }

        return dt;
    }
}
