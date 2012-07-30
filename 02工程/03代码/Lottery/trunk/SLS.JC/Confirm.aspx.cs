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
using System.Collections.Specialized;
using System.IO;
using System.Text;

public partial class Confirm : RoomPageBase
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
        double summoney = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest("summoney"), 0);
        int isuseid = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("isuseid"), 0);
        string lotid = Shove._Web.Utility.GetRequest("lotid");
        string playid = Shove._Web.Utility.GetRequest("playid");
        string zhushu = Shove._Web.Utility.GetRequest("zhushu");
        string beishu = Shove._Web.Utility.GetRequest("beishu");
        string codes = Shove._Web.Utility.GetRequest("codes").Replace("#", "+");

        if (string.IsNullOrEmpty(zhushu) || string.IsNullOrEmpty(beishu) || string.IsNullOrEmpty(codes) || string.IsNullOrEmpty(lotid))
        {
            return;
        }

        if (summoney < 2 || isuseid == 0)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();

        string Number = codes;

        if (lotid == "74" || lotid == "75")
        {
            if (summoney == 2)
            {
                playid = lotid + "01";
            }
            else
            {
                playid = lotid + "02";
            }

            if (Number.Length < 9)
            {
                return;
            }

            DataTable dtMatch = new DAL.Tables.T_IsuseForSFC().Open("", "IsuseID=" + isuseid.ToString(), "No");

            if (dtMatch == null || dtMatch.Rows.Count != 14)
            {
                return;
            }

            sb.Append("<table bgcolor=\"#afcea4\" border=\"0\" cellpadding=\"4\" cellspacing=\"1\">")
                .Append("<tbody><tr class=\"gg_form_trt_02\"><td width=\"80\">场次</td><td width=\"270\">比赛对阵</td><td width=\"140\" align=\"center\">比赛时间</td><td width=\"100\" align=\"center\">投注结果</td></tr></tbody>")
                .Append("<tbody id=\"matchList\">");

            for (int i = 0; i < 14; i++)
            {
                sb.Append("<tr class=\"dg_trbg1\"><td>").Append(dtMatch.Rows[i]["NO"].ToString())
                    .Append("</td><td>").Append(dtMatch.Rows[i]["HostTeam"].ToString()).Append(" VS ").Append(dtMatch.Rows[i]["QuestTeam"].ToString())
                    .Append("</td><td>").Append(dtMatch.Rows[i]["DateTime"].ToString())
                    .Append("</td><td>");
                if (Number.Substring(0, 1).Equals("("))
                {
                    sb.Append(Number.Substring(1, Number.IndexOf(")") - 1));
                    Number = Number.Substring(Number.IndexOf(")") + 1);
                }
                else
                {
                    sb.Append(Number.Substring(0, 1));
                    Number = Number.Substring(1);
                }
                sb.Append("</td></tr>");
            }

            sb.Append("</tbody></table>");
        }
        else if (lotid == "15")
        {
            if (summoney == 2)
            {
                playid = lotid + "01";
            }
            else
            {
                playid = lotid + "02";
            }

            if (Number.Length < 12)
            {
                return;
            }

            DataTable dtMatch = new DAL.Tables.T_IsuseForLCBQC().Open("", "IsuseID=" + isuseid.ToString(), "No");

            if (dtMatch == null || dtMatch.Rows.Count != 6)
            {
                return;
            }

            sb.Append("<table bgcolor=\"#afcea4\" border=\"0\" cellpadding=\"4\" cellspacing=\"1\">")
                .Append("<tbody><tr class=\"gg_form_trt_02\"><td width=\"80\">场次</td><td width=\"270\">比赛对阵</td><td width=\"140\" align=\"center\">比赛时间</td><td width=\"100\" align=\"center\">投注结果</td></tr></tbody>")
                .Append("<tbody id=\"matchList\">");

            for (int i = 0; i < 6; i++)
            {
                sb.Append("<tr class=\"dg_trbg1\"><td>").Append(dtMatch.Rows[i]["NO"].ToString())
                    .Append("</td><td>").Append(dtMatch.Rows[i]["HostTeam"].ToString()).Append(" VS ").Append(dtMatch.Rows[i]["QuestTeam"].ToString())
                    .Append("</td><td>").Append(dtMatch.Rows[i]["DateTime"].ToString())
                    .Append("</td><td>");
                if (Number.Substring(0, 1).Equals("("))
                {
                    sb.Append(Number.Substring(1, Number.IndexOf(")") - 1));
                    Number = Number.Substring(Number.IndexOf(")") + 1);
                }
                else
                {
                    sb.Append(Number.Substring(0, 1));
                    Number = Number.Substring(1);
                }
                sb.Append("</td></tr>");
            }

            sb.Append("</tbody></table>");
        }
        else if (lotid == "2")
        {
            if (summoney == 2)
            {
                playid = lotid + "01";
            }
            else
            {
                playid = lotid + "02";                
            }

            if (Number.Length < 8)
            {
                return;
            }

            DataTable dtMatch = new DAL.Tables.T_IsuseForJQC().Open("", "IsuseID=" + isuseid.ToString(), "No");

            if (dtMatch == null || dtMatch.Rows.Count != 8)
            {
                return;
            }

            sb.Append("<table bgcolor=\"#afcea4\" border=\"0\" cellpadding=\"4\" cellspacing=\"1\">")
                .Append("<tbody><tr class=\"gg_form_trt_02\"><td width=\"80\">场次</td><td width=\"270\">比赛对阵</td><td width=\"140\" align=\"center\">比赛时间</td><td width=\"100\" align=\"center\">投注结果</td></tr></tbody>")
                .Append("<tbody id=\"matchList\">");

            for (int i = 0; i < 8; i = i + 2)
            {
                sb.Append("<tr class=\"dg_trbg1\"><td>").Append(dtMatch.Rows[i]["NO"].ToString())
                    .Append("</td><td>").Append(dtMatch.Rows[i]["Team"].ToString()).Append(" VS ").Append(dtMatch.Rows[i + 1]["Team"].ToString())
                    .Append("</td><td>").Append(dtMatch.Rows[i]["DateTime"].ToString())
                    .Append("</td><td>");
                if (Number.Substring(0, 1).Equals("("))
                {
                    sb.Append(Number.Substring(1, Number.IndexOf(")") - 1));
                    Number = Number.Substring(Number.IndexOf(")") + 1);
                }
                else
                {
                    sb.Append(Number.Substring(0, 1));
                    Number = Number.Substring(1);
                }
                sb.Append("</td></tr>");
            }

            sb.Append("</tbody></table>");
        }
        else if ((new SLS.Lottery.JCLQ().CheckPlayType(Shove._Convert.StrToInt(playid, -1))) || (new SLS.Lottery.JCZQ().CheckPlayType(Shove._Convert.StrToInt(playid, -1))))
        {
            codes = PF.GetScriptResTable(codes);
            sb.Append(codes);
        }
        else
        {
            codes = Shove._Convert.ToHtmlCode(codes) + "&nbsp;";
            sb.Append(codes);
        }

        labLotteryNumber.Text = sb.ToString();
        labMultiple.Text = beishu;
        labSchemeMoney.Text = summoney.ToString();
        labNum.Text = zhushu;

        DataTable dt = new DAL.Tables.T_Isuses().Open("", "ID=" + isuseid.ToString(), "");

        if (dt == null || dt.Rows.Count != 1)
        {
            return;
        }

        labEndTime.Text = Shove._Convert.StrToDateTime(dt.Rows[0]["EndTime"].ToString(), DateTime.Now.AddHours(1).ToString()).ToString("yyyy-MM-dd HH:mm");

        hidplayid.Value = playid.ToString();
        hidlotid.Value = lotid;
        hidSchemeMoney.Value = summoney.ToString();
        hidMultiple.Value = beishu;
        hidisuseid.Value = isuseid.ToString();
        hidcodes.Value = codes;
        hidSumNum.Value = zhushu;
        HidIsuseEndTime.Value = dt.Rows[0]["EndTime"].ToString();
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
            PlayTypeID = int.Parse(hidplayid.Value);
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

        string LotteryNumber = hidcodes.Value.Replace("&nbsp;", " ");

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

        if (DateTime.Now >= Shove._Convert.StrToDateTime(HidIsuseEndTime.Value.Replace("/", "-"), DateTime.Now.AddDays(-1).ToString()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "本期投注已截止，谢谢。");

            return;
        }

        double Price = 2.0;

        if (PlayTypeID == 3903 || PlayTypeID == 3904)
        {
            Price = 3.0;
        }

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

        Response.Redirect("Home/Room/UserBuySuccess.aspx?LotteryID=" + LotteryID.ToString() + "&&Money=" + SumMoney.ToString() + "&SchemeID=" + SchemeID.ToString() + "");

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

    private string splitLotteryNumber(string Lottery)
    {
        string xx = Lottery;

        string result = "";// 方法

        while (!string.IsNullOrEmpty(xx))
        {
            if (xx.Substring(0, 1).Equals("("))
            {
                result += xx.Substring(1, xx.IndexOf(")") + 1);
                xx = xx.Substring(xx.IndexOf(")") + 1);
            }
            else
            {
                result += xx.Substring(0, 1);
                xx = xx.Substring(1);
            }
            result += ",";
        }

        if (result.EndsWith(","))
        {
            result = result.Substring(0, result.Length - 1);
        }

        return result;
    }
}
