using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;

public partial class Home_Room__BuyConfirm : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindBet();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Request.Url.AbsolutePath;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    private void BindBet()
    {
        long BuyID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), 0);

        string type = Shove._Web.Utility.GetRequest("type").ToString();

        if (BuyID < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "获取数据失败请重新操作");
            return;
        }

        if (_User == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请登陆后执行此操作");
            return;
        }

        DataTable dt = null;

        // 通过ID 获取擂台方案表中的相关数据
        if (type == "1")
        {
            dt = new DAL.Tables.T_ChallengeScheme().Open("ID,[DateTime],InitiateUserId,IsuseID,PlayTypeID,LotteryNumber,IsOpened", " ID = " + BuyID, "");
        }
        else
        {
            dt = Shove.Database.MSSQL.Select("select ID,[DateTime],InitiateUserId,IsuseID,PlayTypeID,LotteryNumber,IsOpened from T_ChallengeSaveScheme where ID = " + BuyID);
        }

        if (dt == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "对不起获取该方案失败，请重新操作");
            return;
        }

        if (dt.Rows.Count != 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "对不起获取该方案失败，请重新操作");
            return;
        }
        // 是否已经开过奖的
        if (dt.Rows[0]["IsOpened"].ToString().Equals("1"))
        {
            Shove._Web.JavaScript.Alert(this.Page, "对不起该方案内容已截至");
            return;
        }

        #region

        string LotteryNumber = "";

        int Multiple = 0;
        double SumMoney = 0;
        long IsuseID = 0;
        int Count = 0;
        int LotID = 0;
        int PlayID = 0;

        // 购买ID
        hidBuyID.Value = BuyID.ToString();

        DataRow dr = dt.Rows[0];

        string HidIsuseID = dt.Rows[0]["IsuseID"].ToString();
        string playType = dt.Rows[0]["PlayTypeID"].ToString();
        // Share  总分数 1 
        string tb_Share = "1";
        // 购买分数 为 1
        string tb_BuyShare = "1";
        // 不保密
        string tbSecrecyLevel = "0";

        string tb_LotteryNumber = dr["LotteryNumber"].ToString();
        // 购买金额等于 总金额
        string tb_hide_AssureMoney = "2";
        // 没有
        string HidLotteryID = "72";
        // 倍数1倍
        string tb_Multiple = "1";

        if (tb_Multiple == "")
        {
            tb_Multiple = "1";
        }

        int Share = 0;
        int BuyShare = 0;
        double AssureMoney = 0;
        short SecrecyLevel = 0;

        try
        {
            Share = int.Parse(tb_Share);
            BuyShare = int.Parse(tb_BuyShare);
            AssureMoney = double.Parse(tb_hide_AssureMoney);
            Multiple = int.Parse(tb_Multiple);
            SecrecyLevel = short.Parse(tbSecrecyLevel);
            PlayID = int.Parse(playType);
            LotID = int.Parse(HidLotteryID);
            IsuseID = long.Parse(HidIsuseID);
        }
        catch { }

        if ((BuyShare == Share) && (AssureMoney == 0))
        {
            Share = 1;
            BuyShare = 1;
        }

        double BuyMoney = BuyShare * (SumMoney / Share) + AssureMoney;

        LotteryNumber = tb_LotteryNumber;

        if (!string.IsNullOrEmpty(LotteryNumber) && LotteryNumber[LotteryNumber.Length - 1] == '\n')
        {
            LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
        }

        hidLotteryNumber.Value = LotteryNumber;

        if (string.IsNullOrEmpty(LotteryNumber))
        {
            Shove._Web.JavaScript.Alert(this.Page, "传递的参数错误，请重新发起操作！");

            return;
        }

        dt = new DAL.Tables.T_PassRate().Open("MatchID, MatchNumber, StopSellTime", "", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        hidMatchID.Value = "";

        foreach (DataRow drr in dt.Rows)
        {
            hidMatchID.Value += drr["MatchID"].ToString() + ",";
        }

        if (hidMatchID.Value.EndsWith(","))
        {
            hidMatchID.Value = hidMatchID.Value.Substring(0, hidMatchID.Value.Length - 1);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<table width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"1\" class=\"BgBlue\">");
        sb.Append("<tr align=\"center\" bgcolor=\"#FFFFFF\" class=\"BlueLightBg WhiteWords\">");
        sb.Append("<td width=\"8%\"><strong>序号</strong></td>");
        sb.Append("<td><strong>过关场次</strong></td>");
        sb.Append("<td width=\"10%\"><strong>过关方式</strong></td>");
        sb.Append("<td width=\"10%\"><strong>注数</strong></td>");
        sb.Append("<td width=\"10%\"><strong>投注金额(元)</strong></td></tr>");

        string[] LotteryNumbers = LotteryNumber.Replace("\r", "").Split('\n');
        string Number = "";
        int No = 0;
        string BuyWays = "";

        StringBuilder sbMatchIDs = new StringBuilder();

        PlayID = Shove._Convert.StrToInt(LotteryNumbers[0].Split(';')[0], 7201);
        LotID = Shove._Convert.StrToInt(PlayID.ToString().Substring(0, 2), 72);

        DateTime EndTime = DateTime.Now;

        foreach (string str in LotteryNumbers)
        {
            if (string.IsNullOrEmpty(str))
            {
                continue;
            }

            No++;
            Count++;

            if (str.Split(';').Length < 3)
            {
                continue;
            }

            try
            {
                Multiple = Shove._Convert.StrToInt(str.Split(';')[2].Substring(1, str.Split(';')[2].Length - 2).Substring(2), 1);
            }
            catch
            { }

            SumMoney += 2 * Multiple;

            if (No > 10)
            {
                continue;
            }

            sb.Append("<tr align=\"center\" class=\"" + ((No % 2 == 0) ? "BlueWord WhiteBg" : "BlueLightBg2 BlueWord") + "\">");
            sb.Append("<td>" + No.ToString() + "</td>");

            Number = str.Split(';')[1].Substring(1, str.Split(';')[1].Length - 2);
            string[] Numbers = Number.Split('|');

            if (Numbers.Length < 2)
            {
                continue;
            }

            sb.Append("<td height=\"20\">");

            BuyWays = Numbers.Length.ToString() + "串1";

            long MatchID = 0;

            for (int i = 0; i < Numbers.Length; i++)
            {
                if (Numbers[i].IndexOf("(") < 0)
                {
                    continue;
                }

                MatchID = Shove._Convert.StrToLong(Numbers[i].Substring(0, Numbers[i].IndexOf("(")), 1);

                DataRow[] dr2 = dt.Select("MatchID=" + MatchID.ToString());

                if (dr2.Length < 1)
                {
                    continue;
                }

                sbMatchIDs.Append(Numbers[i].Substring(0, Numbers[i].IndexOf("(")) + ",");

                sb.Append(dr2[0]["MatchNumber"].ToString() + "->" + PF.Getesult(PlayID.ToString(), Numbers[i].Substring(Numbers[i].IndexOf("(") + 1, Numbers[i].IndexOf(")") - Numbers[i].IndexOf("(") - 1)) + ";");
            }

            sb.Append("</td>");
            sb.Append("<td height=\"20\">" + BuyWays + "</td>");
            sb.Append("<td>1</td>");
            sb.Append("<td>" + (2 * Multiple).ToString() + "</td></tr>");
        }

        sb.Append("</table>");
        labLotteryNumber.Text = sb.ToString();

        string MatchIDs = sbMatchIDs.ToString();

        if (MatchIDs.EndsWith(","))
        {
            MatchIDs = MatchIDs.Substring(0, MatchIDs.Length - 1);
        }
        if (string.IsNullOrEmpty(MatchIDs.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "无法获取信息");
            return;
        }
        DataRow[] drTime = dt.Select("MatchID in (" + MatchIDs + ")", "StopSellTime");

        if (drTime.Length > 0)
        {
            EndTime = Shove._Convert.StrToDateTime(drTime[0]["StopSellTime"].ToString(), DateTime.Now.AddHours(1).ToString());
        }

        labEndTime.Text = EndTime.ToString("yyyy-MM-dd HH:mm:ss");
        HidIsuseEndTime.Value = EndTime.ToString();

        labMultiple.Text = Multiple.ToString();
        labSchemeMoney.Text = SumMoney.ToString();
        labNum.Text = Count.ToString();

        dt = new DAL.Tables.T_Isuses().Open("ID", "LotteryID= " + LotID.ToString(), "EndTime desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (dt.Rows.Count < 1)
        {
            PF.GoError(ErrorNumber.NoIsuse, "请添加期号", this.GetType().BaseType.FullName);

            return;
        }

        IsuseID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), 0);

        hidSchemeMoney.Value = SumMoney.ToString();
        hidMultiple.Value = Multiple.ToString();
        hidlotid.Value = LotID.ToString();
        hidplayid.Value = PlayID.ToString();
        hidSumNum.Value = Count.ToString();
        hidisuseid.Value = IsuseID.ToString();
        
        #endregion
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (_User == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您还没有登录，请登录后再进行操作！");

            return;
        }

        double SumMoney = 0;
        int Multiple = 0;
        short SecrecyLevel = 0;
        int PlayTypeID = 0;
        int LotteryID = 0;
        long IsuseID = 0;
        int SumNum = 0;

        try
        {
            SumMoney = double.Parse(hidSchemeMoney.Value);
            Multiple = int.Parse(hidMultiple.Value);
            PlayTypeID = int.Parse(hidplayid.Value);
            LotteryID = int.Parse(hidlotid.Value);
            
            IsuseID = long.Parse(hidisuseid.Value);
            SumNum = int.Parse(hidSumNum.Value);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if (SumMoney <= 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        if (SumMoney > _User.Balance)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您的余额不足，请充值。");

            return;
        }

        if (SumMoney > PF.SchemeMaxBettingMoney)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注金额不能大于" + PF.SchemeMaxBettingMoney.ToString() + "，谢谢。");

            return;
        }

        if (Multiple > 999)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注倍数不能大于 999 倍，谢谢。");

            return;
        }

         string LotteryNumber = "";
         string FileName = "";

         if (Shove._Convert.StrToLong(hidBuyID.Value, 0) < 1)
         {
             FileName = System.AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + Request.Cookies["ASP.NET_SessionId"].Value + ".txt";

             LotteryNumber = File.ReadAllText(FileName);

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
                 if (string.IsNullOrEmpty(str))
                 {
                     continue;
                 }

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
             LotteryNumber = hidLotteryNumber.Value;
         }

        if (DateTime.Now >= Shove._Convert.StrToDateTime(HidIsuseEndTime.Value.Replace("/", "-"), DateTime.Now.AddDays(-1).ToString()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "本期投注已截止，谢谢。");

            return;
        }

        double Price = 2.0;

        if (Price * SumNum * Multiple != SumMoney)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有错误，请仔细检查。");

            return;
        }

        string ReturnDescription = "";

        long SchemeID = _User.InitiateScheme(IsuseID, PlayTypeID, "(无标题)", "", LotteryNumber, "", Multiple, SumMoney, 0, 1, 1, "", SecrecyLevel, 0.04, ref ReturnDescription);

        if (SchemeID < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName + "(-347)");

            return;
        }

        File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + "SchemeInfo\\" + SchemeID.ToString() + ".txt", LotteryNumber + "$" + hidMatchID.Value);

        if (File.Exists(FileName))
        {
            try
            {
                File.Delete(FileName);
            }
            catch { }
        }

        Response.Redirect("/Home/Room/UserBuySuccess.aspx?LotteryID=" + LotteryID.ToString() + "&&Money=" + SumMoney.ToString() + "&SchemeID=" + SchemeID.ToString() + "");

        return;
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
}
