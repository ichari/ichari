using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Xml;

public partial class Lottery_Buy_JQC : RoomPageBase
{
    public int LotteryID;
    public string LotteryName;

    public int Number = 3;
    public string script = "";
   
    public string DZ = "";
  
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Lottery_Buy_JQC), this.Page);
        LotteryID = 2;

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

            tbSFC.InnerHtml = GetWinListHM(74, 0);
            tbRJC.InnerHtml = GetWinListHM(75, 1);
            tbJQC.InnerHtml = GetWinListHM(2, 0);
            tbLCJQC.InnerHtml = GetWinListHM(15, 0);

            tbWin1.InnerHtml = BindWinList(DataCache.GetWinInfo(LotteryID));
            
            DZ = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), Shove._Web.Utility.GetRequest("DZ"));

            BindIsuses();
        }
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

    private void BindIsuses()
    {
        DataTable dt = new DAL.Tables.T_Isuses().Open("Name, EndTime, StartTime", "LotteryID=" + LotteryID.ToString() + " and isopened=0 and getdate() < EndTime", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        dt.Columns.Add("value", typeof(System.String));

        foreach (DataRow dr in dt.Rows)
        {
            if (DateTime.Now.CompareTo(Shove._Convert.StrToDateTime(dr["StartTime"].ToString(), DateTime.Now.ToString())) < 0)
            {
                dr["value"] = dr["Name"];
            }
            else
            {
                dr["value"] = dr["Name"];
            }
        }

        Shove.ControlExt.FillDropDownList(ddlIsuses, dt, "value", "Name");
    }

    /// <summary>
    /// 格式化开奖号码
    /// </summary>
    /// <param name="LotteryID"></param>
    /// <param name="winNumber"></param>
    /// <returns></returns>
    private string FormatWinNumber(string winNumber)
    {
        StringBuilder sb = new StringBuilder();

        if (winNumber.Length > 0)
        {
            sb.Append("</td><td align='left' class='red14'>").Append(winNumber);
        }

        return sb.ToString();
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
    public string GetIsuseInfo(int LotteryID, string Name)
    {
        try
        {
            DataTable dt = DataCache.GetIsusesInfo(LotteryID);
            string strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //本期期信息
            DataRow[] drCurrIsuse = dt.Select("name='" + Shove._Web.Utility.FilteSqlInfusion(Name) + "'", "EndTime desc");
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

            StringBuilder sb = new StringBuilder();

            sb.Append(IsuseID.ToString())
                .Append(",")
                .Append(IsuseName)
                .Append(",")
                .Append(IsuseEndTime)
                .Append("|<table cellspacing='5' cellpadding='0' style='text-align: center; font-weight: bold;'><tr><td align='left'  height='25' class='hui12'>")
                .Append(LastIsuseName)
                .Append("&nbsp;期开奖:&nbsp;&nbsp;&nbsp;&nbsp;")
                .Append(LastWinNumber)
                .Append("</td></tr>");

            sb.Append("</table>");

            return sb.ToString();
        }
        catch (Exception e)
        {
            new Log("TWZT").Write(this.GetType() + e.Message);

            return e.Message;
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
    /// 获取胜负彩奖池信息
    /// </summary>
    /// <param name="IsuseID"></param>
    /// <returns></returns>
    /// 
    private string GetTotalMoneySFC(string IsuseID)
    {
        string TotalMoneySFC = "";

        string key = "Home_Room_Buy_GetTotalMoneySFC_" + IsuseID;

        DataTable dtTotalMoneySFC = Shove._Web.Cache.GetCacheAsDataTable(key);

        if (dtTotalMoneySFC == null)
        {
            dtTotalMoneySFC = new DAL.Tables.T_TotalMoney().Open("", "IsuseID=" + IsuseID, "");

            if (dtTotalMoneySFC == null)
            {
                new Log("System").Write(this.GetType().FullName + "数据库繁忙，请重试(GetTotalMoneySFC)");
                return "";
            }

            Shove._Web.Cache.SetCache(key, dtTotalMoneySFC, 120);
        }

        if (dtTotalMoneySFC.Rows.Count > 0)
        {
            TotalMoneySFC = dtTotalMoneySFC.Rows[0]["TotalMoney"].ToString();
        }

        return TotalMoneySFC;
    }

    /// <summary>
    /// 购买彩票
    /// </summary>
    /// <param name="_User"></param>
    private void Buy(Users _User)
    {
        string HidIsuseID = Shove._Web.Utility.GetRequest("HidIsuseID");
        string HidIsuseEndTime = Shove._Web.Utility.GetRequest("HidIsuseEndTime");
        string playType = Shove._Web.Utility.GetRequest("playType");
        string CoBuy = Shove._Web.Utility.GetRequest("CoBuy");
        string tb_Share = Shove._Web.Utility.GetRequest("tb_Share");
        string tb_BuyShare = Shove._Web.Utility.GetRequest("tb_BuyShare");
        string tb_AssureShare = Shove._Web.Utility.GetRequest("tb_AssureShare");
        string tb_OpenUserList ="";
        string tb_Title = Shove._Web.Utility.GetRequest("tb_Title");
        string tb_Description = Shove._Web.Utility.GetRequest("tb_Description");
        string tbSecrecyLevel = Shove._Web.Utility.GetRequest("SecrecyLevel");
        string tb_LotteryNumber = Shove._Web.Utility.FilteSqlInfusion(Request["tb_LotteryNumber"]);
        string tb_hide_SumMoney = Shove._Web.Utility.GetRequest("tb_hide_SumMoney");
        string tb_hide_AssureMoney = Shove._Web.Utility.GetRequest("tb_hide_AssureMoney");
        string tb_hide_SumNum = Shove._Web.Utility.GetRequest("tb_hide_SumNum");
        string HidLotteryID = Shove._Web.Utility.GetRequest("HidLotteryID");
        string tb_Multiple = Shove._Web.Utility.GetRequest("tb_Multiple");
        string tb_SchemeBonusScale = Shove._Web.Utility.GetRequest("tb_SchemeBonusScale");
        string playTypeID = Shove._Web.Utility.GetRequest("tbPlayTypeID");
        string tb_SumMoney = Shove._Web.Utility.GetRequest("tb_SchemeMoney");
        string bet = Shove._Web.Utility.GetRequest("bet");

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
        double SchemeBonusScale = 0;

        if (string.IsNullOrEmpty(tb_Share))
        {
            tb_Share = Shove._Web.Utility.GetRequest("tb_MinSchemeMoney");
        }

        try
        {
            SumMoney = double.Parse(tb_hide_SumMoney);
            Share = int.Parse(tb_Share);
            BuyShare = int.Parse(tb_BuyShare);
            AssureMoney = double.Parse(tb_hide_AssureMoney);
            Multiple = int.Parse(tb_Multiple);
            SumNum = int.Parse(tb_hide_SumNum);
            SecrecyLevel = short.Parse(tbSecrecyLevel);
            PlayTypeID = int.Parse(playTypeID);
            LotteryID = int.Parse(HidLotteryID);
            IsuseID = long.Parse(HidIsuseID);
            SchemeBonusScale = double.Parse(tb_SchemeBonusScale);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if (playTypeID != playType)
        {
            if (bet != "Bet01")
            {
                SumMoney = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest("tb_MinSchemeMoney"), 0);
            }
            else
            {
                SumMoney = double.Parse(tb_SumMoney);
            }
        }

        if ((SumMoney <= 0) || (SumNum < 1 && playTypeID == playType))
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

        SchemeBonusScale = SchemeBonusScale / 100;

        string LotteryNumber = tb_LotteryNumber;

        if (playTypeID == playType)
        {
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
        }
                else
        {
            LotteryNumber = "";
        }

        string ReturnDescription = "";

        if (DateTime.Now >= Shove._Convert.StrToDateTime(HidIsuseEndTime.Replace("/", "-"), DateTime.Now.AddDays(-1).ToString()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "本期投注已截止，谢谢。");

            return;
        }

        if (playType == playTypeID && Price * SumNum * Multiple != SumMoney)
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

        if (playType != playTypeID)
        {
            double MinMoney = 0;
            double MaxMoney = 0;

            if (bet == "Bet01")
            {
                MinMoney = SumMoney;
                MaxMoney = SumMoney;
            }
            else
            {
                MinMoney = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest("tb_MinSchemeMoney"), 0);
                MaxMoney = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest("tb_MaxSchemeMoney"), 0);
            }

            if (MaxMoney < MinMoney)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您输入的最大方案金额不能小于最小方案金额！");

                return;
            }

            if (MaxMoney > MinMoney * 1.4)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您输入的最大方案金额大于最小方案金额的 1.4 倍！");

                return;
            }

            DAL.Tables.T_PrepareBet t_PrepareBet = new DAL.Tables.T_PrepareBet();

            t_PrepareBet.SchemeID.Value = SchemeID;
            t_PrepareBet.MinMoney.Value = MinMoney;
            t_PrepareBet.MaxMoney.Value = MaxMoney;
            t_PrepareBet.Insert();
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

    /// <summary>
    ///合买热单推荐
    /// </summary>
    /// <param name="_User"></param>
    /// 
    private string GetWinListHM(int LotteryID, int Number)
    {
        string Key = "Home_Room_Buy_ZC_GetWinListHM_" + LotteryID.ToString() + Number.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            string sql = "";

            sql = "select top 8 b.ID,IsuseID,LotteryID ,InitiateUserID ,InitiateName, Money ,Schedule from T_SchemeSupperCobuy a inner join V_Schemes b on a.SchemeID = b.ID and TypeState = 1 and IsuseID=(select top 1 ID from T_Isuses where LotteryID =" + LotteryID.ToString() + " and GETDATE() between StartTime and EndTime order by ID desc) and QuashStatus = 0 order by AtTopStatus desc,Money desc";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                new Log("TWZT").Write("GetWinListHM方法出错!");

                return "";
            }

            Shove._Web.Cache.SetCache(Key, dt, 600);
        }

        StringBuilder sb = new StringBuilder();
        string Name;
        foreach (DataRow dr in dt.Rows)
        {
            Name = dr["InitiateName"].ToString();
            Name = Name.Length > 5 ? (Name.Substring(0, 4) + "*") : Name;
            sb.Append("<tr><td class='black12' title='" + dr["InitiateName"].ToString() + "'>")
                .Append("<a href='../Home/Web/Score.aspx?id=" + dr["InitiateUserID"].ToString() + "&LotteryID=" + LotteryID + "'target='_blank'>")
                .Append(Name)
                .Append("</a>")
                .Append("</td><td class='black12'>")
                .Append(Shove._Convert.StrToDouble(dr["Money"].ToString(), 0).ToString("N0"))
                .Append("</td><td class='black12'>")
                .Append(Shove._Convert.StrToDouble(dr["Schedule"].ToString(), 0))
                .Append("%")
        .Append("</td><td class='red12_2'><a target='_blank' href='")
                .Append("../Home/Room/Scheme.aspx?LotteryID=")
                .Append(LotteryID.ToString()).Append("&ID=")
                .Append(dr["ID"].ToString())
               .Append("'}\">入伙</a></td></tr>");
        }

        return sb.ToString();
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
        string playType = Shove._Web.Utility.GetRequest("playType");
        string Cobuy = Shove._Web.Utility.GetRequest("Cobuy");
        string tb_Share = Shove._Web.Utility.GetRequest("tb_Share");
        string tb_BuyShare = Shove._Web.Utility.GetRequest("tb_BuyShare");
        string tb_AssureShare = Shove._Web.Utility.GetRequest("tb_AssureShare");
        string tb_OpenUserList = Shove._Web.Utility.GetRequest("tb_OpenUserList");
        string tb_Title = Shove._Web.Utility.GetRequest("tb_Title");
        string tb_Description = Shove._Web.Utility.GetRequest("tb_Description");
        string tbSecrecyLevel = Shove._Web.Utility.GetRequest("SecrecyLevel");
        string tb_LotteryNumber = Shove._Web.Utility.FilteSqlInfusion(Request["tb_LotteryNumber"]);
        string tb_hide_SumMoney = Shove._Web.Utility.GetRequest("tb_hide_SumMoney");
        string tb_hide_AssureMoney = Shove._Web.Utility.GetRequest("tb_hide_AssureMoney");
        string tb_hide_SumNum = Shove._Web.Utility.GetRequest("tb_hide_SumNum");
        string HidLotteryID = Shove._Web.Utility.GetRequest("HidLotteryID");
        string tb_Multiple = Shove._Web.Utility.GetRequest("tb_Multiple");
        string playTypeID = Shove._Web.Utility.GetRequest("tbPlayTypeID");
        string tb_SumMoney = Shove._Web.Utility.GetRequest("tb_SchemeMoney");

        if (tb_Multiple == "")
        {
            tb_Multiple = "1";
        }

        DAL.Tables.T_AlipayBuyTemp tbp = new DAL.Tables.T_AlipayBuyTemp();

        tbp.SiteID.Value = 1;
        tbp.UserID.Value = -1;
        if (playTypeID == playType)
        {
            tbp.Money.Value = tb_hide_SumMoney;
        }
        else
        {
            tbp.Money.Value = tb_SumMoney;
        }

        tbp.HandleResult.Value = 0;
        tbp.IsCoBuy.Value = Cobuy == "2";
        tbp.LotteryID.Value = HidLotteryID;
        tbp.IsuseID.Value = HidIsuseID;
        tbp.PlayTypeID.Value = playType;
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
        tbp.Number.Value = Number;

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
        bool IsCoBuy = Shove._Convert.StrToBool(dr["IsCoBuy"].ToString(), false);
        string tb_Share = dr["Share"].ToString();
        string tb_BuyShare = dr["BuyShare"].ToString();
        string tb_AssureShare = dr["AssureShare"].ToString();
        string tb_OpenUserList = dr["OpenUsers"].ToString();
        string tb_Title = dr["Title"].ToString();
        string tb_Description = dr["Description"].ToString();
        string tbSecrecyLevel = dr["SecrecyLevel"].ToString();
        string tb_LotteryNumber = dr["LotteryNumber"].ToString();
        string tb_hide_SumMoney = dr["SumMoney"].ToString();
        string tb_hide_AssureMoney = dr["AssureMoney"].ToString();
        string HidLotteryID = dr["LotteryID"].ToString();
        string tb_Multiple = dr["Multiple"].ToString();

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
        }
        catch { }

        if ((BuyShare == Share) && (AssureMoney == 0))
        {
            Share = 1;
            BuyShare = 1;
        }

        double BuyMoney = BuyShare * (SumMoney / Share) + AssureMoney;

        string LotteryNumber = tb_LotteryNumber;

        if (!string.IsNullOrEmpty(LotteryNumber) && LotteryNumber[LotteryNumber.Length - 1] == '\n')
        {
            LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
        }

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<script type='text/javascript' defer='defer'>");

        sb.Append("$Id('playType").Append(PlayTypeID.ToString()).AppendLine("').checked = true;");
        sb.AppendLine("clickPlayType('" + PlayTypeID.ToString() + "');");
        sb.AppendLine("function BindDataForFromAli(){");

        LotteryNumber = LotteryNumber.Replace("\r", "");
        int LotteryNum = 0;
        string Number = "";
        foreach (string lotteryNumber in LotteryNumber.Split('\n'))
        {
            if (!string.IsNullOrEmpty(lotteryNumber))
            {
                LotteryNum += new SLS.Lottery()[LotteryID].ToSingle(lotteryNumber, ref Number, PlayTypeID).Length;
                sb.AppendLine("var option = document.createElement('option');");
                sb.AppendLine("$Id('list_LotteryNumber').options.add(option);");
                sb.Append("option.innerText = '").Append(lotteryNumber).AppendLine("';");
                sb.Append("option.value = '").Append(lotteryNumber).AppendLine("';");
            }
        }

        if (IsCoBuy)
        {
            sb.AppendLine("$Id('CoBuy').checked = true;");
            sb.AppendLine("oncbInitiateTypeClick();");
        }

        sb.Append("$Id('tb_LotteryNumber').value = '").Append(tb_LotteryNumber.Replace("\r", "\\r").Replace("\n", "\\n")).AppendLine("';");
        sb.Append("$Id('tb_Share').value = '").Append(tb_Share).AppendLine("';");
        sb.Append("$Id('tb_BuyShare').value = '").Append(tb_BuyShare).AppendLine("';");
        sb.Append("$Id('tb_AssureShare').value = '").Append(tb_AssureShare).AppendLine("';");
        sb.Append("$Id('tb_OpenUserList').value = '").Append(tb_OpenUserList).AppendLine("';");
        sb.Append("$Id('tb_Title').value = '").Append(tb_Title).AppendLine("';");
        sb.Append("$Id('tb_Description').value = '").Append(tb_Description.Replace("\r", "\\r").Replace("\n", "\\n")).AppendLine("';");
        sb.Append("$Id('SecrecyLevel").Append(SecrecyLevel.ToString()).AppendLine("').checked = true;");
        sb.Append("$Id('tb_hide_SumMoney').value = '").Append(tb_hide_SumMoney).AppendLine("';");
        sb.Append("$Id('tb_hide_AssureMoney').value = '").Append(AssureMoney.ToString("N0")).AppendLine("';");
        sb.Append("$Id('tb_Multiple').value = '").Append(Multiple.ToString()).AppendLine("';");
        sb.Append("$Id('lab_AssureMoney').innerText = '").Append(AssureMoney.ToString("N0")).AppendLine("';");
        sb.Append("$Id('lab_SumMoney').innerText = '").Append(SumMoney.ToString("N0")).AppendLine("';");
        sb.Append("$Id('lab_Num').innerText = '").Append(LotteryNum.ToString()).AppendLine("';");
        sb.Append("$Id('tb_SchemeMoney').value = '").Append(SumMoney.ToString()).AppendLine("';");
        sb.AppendLine("CalcResult();");
        sb.AppendLine("}");
        if (playType.Length != LotteryID.ToString().Length + 2)
        {
            sb.AppendLine("BindDataForFromAli()");
        }

        sb.AppendLine("</script>");

        script = sb.ToString();
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string GetAddone(int LotteryID, string IsuseID)
    {
        string t_str = "";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_JQC" + IsuseID);

        if (dt == null || dt.Rows.Count == 0)
        {
            dt = new DAL.Tables.T_IsuseForJQC().Open(" top 8 * ", "IsuseID = " + IsuseID, "[No]");

            if ((dt == null) || (dt.Rows.Count != 8))
            {
                return "";
            }

            Shove._Web.Cache.SetCache("DataCache_JQC" + IsuseID, dt, 300);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            t_str += dt.Rows[i]["Team"].ToString().Trim() + "," + dt.Rows[i]["DateTime"].ToString();
            if (i < dt.Rows.Count - 1)
                t_str += ";";
        }
        
        return t_str;
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
        int number = 0;

        return bScalec + "|" + Opt_InitiateSchemeLimitLowerScaleMoney + "|" + Opt_InitiateSchemeLimitLowerScale + "|" + Opt_InitiateSchemeLimitUpperScaleMoney + "|" + Opt_InitiateSchemeLimitUpperScale + "|" + lotteryId.ToString() + "|" + lotteryName + "|" + number.ToString();


    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string GetZCExpertList(int lotteryID)
    {
        DataTable dtExpert = DataCache.GetZCExpertList(lotteryID);

        StringBuilder sb = new StringBuilder();

        if (dtExpert != null && dtExpert.Rows.Count > 0)
        {
            int i = 1;
            foreach (DataRow dr in dtExpert.Rows)
            {
                string Name = dr["UserName"].ToString();
                Name = Name.Length > 6 ? (Name.Substring(0, 5) + "*") : Name;
                sb.AppendLine("<tr align=\"center\">")
                .AppendLine("<td  height=\"22\"  bgcolor=\"#FFFFFF\" class=\"black12\"  title='" + dr["UserName"].ToString() + "'>")
                .Append("<a href='../Home/Web/Score.aspx?id=")
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
                .Append(dr["UserName"].ToString()).Append("'}\">定制</a></td></tr>");

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

    /// <summary>
    /// 获取资讯信息
    /// </summary>
    /// <param name="LotteryID"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string GetNewsInfo(int LotteryID)
    {
        return DataCache.GetLotteryNews(LotteryID);
    }
}

