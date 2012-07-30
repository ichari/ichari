using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using SLS;

public partial class LotteryPackage : RoomPageBase
{
    public string script = "";
    private static Dictionary<int, int[]> NumberCount = new Dictionary<int, int[]>();

    static LotteryPackage()
    {
        NumberCount[5] = new int[] { 6, 1 };
        NumberCount[6] = new int[] { 3, 0 };
        NumberCount[39] = new int[] { 5, 2 };
        NumberCount[63] = new int[] { 3, 0 };
        NumberCount[64] = new int[] { 5, 0 };
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(LotteryPackage), this.Page);

        if (!IsPostBack)
        {
            if (_User == null)
            {
                NoLogin.Visible = true;
                Login.Visible = false;
            }
            else
            {
                NoLogin.Visible = false;
                Login.Visible = true;
            }

            HidLotteryID.Value = Shove._Web.Utility.GetRequest("LotteryID");

            if (HidLotteryID.Value == "" || (HidLotteryID.Value != "5" && HidLotteryID.Value != "6" && HidLotteryID.Value != "39"))
            {
                HidLotteryID.Value = "5";
            }

            HidType.Value = Shove._Web.Utility.GetRequest("Type");

            if (HidType.Value == "" || Shove._Convert.StrToInt(HidType.Value, 0) > 4 || Shove._Convert.StrToInt(HidType.Value, 0) < 1)
            {
                HidType.Value = "4";
            }

            BindUsers();
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

    private void BindUsers()
    {
        string Key = "LotteryPackage_BindUsers";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select UserID,LotteryID,b.Name as UserName,c.Name as LotteryName,case Type when 1 then '吉祥包月' when 2 then '如意包季' when 3 then '幸福半年' when 4 then '安康包年' end as TypeName, IsuseCount*Price*Multiple*Nums as Moneys from T_Chases a inner join T_Users b on a.UserID = b.ID inner join T_Lotteries c on a.LotteryID = c.ID order by a.ID desc");

            if (dt == null)
            {
                return;
            }

            Shove._Web.Cache.SetCache(Key, dt, 3600);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<div id='scrollWinUsers' style='overflow:hidden;height:290px'>")
            .Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr>")
                .Append("<td width=\"23%\" height=\"24\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"blue12\">")
                .Append("<a href='Home/Web/Score.aspx?id=" + dr["UserID"].ToString() + "&LotteryID=" + dr["LotteryID"].ToString() + "' target=\"_blank\" title='" + dr["UserName"].ToString() + "'>" + Shove._String.Cut(dr["UserName"].ToString(), 3) + "</a>")
                .Append("</td>")
                .Append("<td width=\"25%\" height=\"24\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"blue12\">")
                .Append(dr["LotteryName"].ToString())
                .Append("</td>")
                .Append("<td width=\"24%\" height=\"24\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"red12_2\">")
                .Append(dr["TypeName"].ToString())
                .Append("</td>")
                .Append("<td width=\"27%\" height=\"24\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"blue12\">")
                .Append(Shove._Convert.StrToDouble(dr["Moneys"].ToString(), 0).ToString("N").Split('.')[0])
                .Append("</td>")
                .Append("</tr>");
        }

        sb.Append("</table></div>");

        tdUsers.InnerHtml = sb.ToString();
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string InitLuckLotteryNumber(int LotteryID)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">")
            .AppendLine("<tr>")
            .AppendLine("<td height=\"22\" align=\"center\">")
            .AppendLine("&nbsp;</td>");

        for (int i = 0; i < NumberCount[LotteryID][0]; i++)
        {
            sb.AppendLine("<td align=\"center\">")
                .AppendLine("<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"Home/Room/images/ssq_bg_td.jpg\">")
                .AppendLine("<tr>")
                .AppendLine("<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.ToString() + "'>")
                .AppendLine("-")
                .AppendLine("</td></tr></table></td>");
        }
        for (int i = 0; i < NumberCount[LotteryID][1]; i++)
        {
            sb.AppendLine("<td align=\"center\">")
                .AppendLine("<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"Home/Room/images/ssq_bg_td.jpg\">")
                .AppendLine("<tr>")
                .AppendLine("<td height=\"22\" align=\"center\" class=\"blue12\" id='tdLuckNumber" + (NumberCount[LotteryID][0] + i).ToString() + "'>")
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
            if (LotteryID == 5)
            {
                LotteryNumber = new SLS.Lottery()[LotteryID].BuildNumber(6, 1, 1);
            }
            else if (LotteryID == 39)
            {
                LotteryNumber = new SLS.Lottery()[LotteryID].BuildNumber(5, 2, 1);
            }
            else
            {
                LotteryNumber = new SLS.Lottery()[LotteryID].BuildNumber(1);
            }

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

        string Red = null;
        string Blue = null;

        if (LotteryID == 5 || LotteryID == 59 || LotteryID == 13 || LotteryID == 39 || LotteryID == 9 || LotteryID == 65)
        {
            LotteryNumber = LotteryNumber.Replace(" + ", " ");
        }

        if (LotteryID == 58 || LotteryID == 6 || LotteryID == 3 || LotteryID == 63 || LotteryID == 64)
        {
            Red = LotteryNumber.Split('+')[0];
            try
            {
                Blue = LotteryNumber.Split('+')[1];
            }
            catch { Blue = ""; }

            LotteryNumber = "";

            for (int i = 0; i < Red.Length; i++)
            {
                LotteryNumber += Red.Substring(i, 1) + " ";
            }

            LotteryNumber += Blue;

            LotteryNumber = LotteryNumber.Trim();
        }

        return LotteryNumber;
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string SplitScheme(string LotteryNumber, int LotteryID)
    {
        SLS.Lottery slsLottery = new SLS.Lottery();
        string Number = "";
        string[] t_lotterys = slsLottery[LotteryID].ToSingle(LotteryNumber, ref Number, LotteryID * 100 + 1);

        if ((t_lotterys == null) || (t_lotterys.Length < 1))
        {
            return "";
        }

        LotteryNumber = "";
        foreach (string s in t_lotterys)
        {
            LotteryNumber += s + ",";
        }

        return LotteryNumber + t_lotterys.Length.ToString();
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string AnalyseScheme(string Content, int LotteryID)
    {
        string Result = new SLS.Lottery()[LotteryID].AnalyseScheme(Content, LotteryID * 100 + 1);

        return Result.Trim();
    }

    protected void btnOK_Click(object sender, System.EventArgs e)
    {
        int IsuseCount = Shove._Convert.StrToInt(HidIsuseCount.Value, -1);
        int LotteryID = Shove._Convert.StrToInt(HidLotteryID.Value, -1);
        short Type = Shove._Convert.StrToShort(HidType.Value, -1);
        int Multiple = Shove._Convert.StrToInt(HidMultiple.Value, -1);
        int Nums = Shove._Convert.StrToInt(HidNums.Value, -1);
        short BetType = Shove._Convert.StrToShort(HidBetType.Value, -1);
        double Money = Shove._Convert.StrToDouble(HidMoney.Value, -1);
        string Title = Shove._Web.Utility.GetRequest("tbTitle1");
        int PlayTypeID = Shove._Convert.StrToInt(HidPlayTypeID.Value, -1);
      
        if (string.IsNullOrEmpty(Title))
        {
            Title = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        if (IsuseCount < 0 || LotteryID < 0 || Type < 0 || Multiple < 0 || Nums < 0 || BetType < 0 || Money < 0 || PlayTypeID < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注信息有误！");

            return;
        }

        if (_User.Balance < Money)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您的余额不足，请充值！");

            return;
        }

        int Price = 2;
        if (PlayTypeID == 3903)
        {
            Price = 3;
        }

        if (IsuseCount * Multiple * Nums * Price != Money)
        {
            Shove._Web.JavaScript.Alert(this.Page, "投注总金额与投注倍数、注数、期数不相符！");

            return;
        }

        int Result = -1;

        DateTime EndTime = DateTime.Now;

        switch (Type)
        {
            case 1:
                {
                    EndTime = EndTime.AddMonths(1);
                } break;

            case 2:
                {
                    EndTime = EndTime.AddMonths(3);
                } break;

            case 3:
                {
                    EndTime = EndTime.AddMonths(6);
                } break;

            case 4:
                {
                    EndTime = EndTime.AddYears(1);
                } break;
        }

        string ChaseXML = "";

        if (BetType == 1)
        {
            if (Nums != Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("selectMultiple"), 0))
            {
                Shove._Web.JavaScript.Alert(this.Page, "投注注数出现异常！");

                return;
            }

            if (Multiple != 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "投注倍数出现异常！");

                return;
            }

            HidLotteryNumber.Value = "";
            string[] lotteryNumber;
            ChaseXML = "<ChaseDetails>";

            Lottery l = new Lottery();

            if (LotteryID == 5)
            {
                lotteryNumber = l[5].BuildNumber(6, 1, Nums*IsuseCount).Split(new String[] { "\n" }, StringSplitOptions.None);
            }
            else if (LotteryID == 39)
            {
                lotteryNumber = l[39].BuildNumber(5, 2, Nums * IsuseCount).Split(new String[] { "\n" }, StringSplitOptions.None);
            }
            else
            {
                lotteryNumber = l[LotteryID].BuildNumber(Nums * IsuseCount).Split(new String[] { "\n" }, StringSplitOptions.None);
            }

            if (lotteryNumber.Length != Nums * IsuseCount)
            {
                Shove._Web.JavaScript.Alert(this.Page, "随机生成号码时出现异常！");

                return;
            }

            int i = 1;
            int j = 0;
            string LotteryNumber = "";
            foreach (string s in lotteryNumber)
            {
                LotteryNumber += s + "\n";

                if (i % Nums == 0)
                {
                    ChaseXML += "<Chase ChaseLotteryNumber=\"" + LotteryNumber + "\"/>";
                    j++;
                    LotteryNumber = "";
                }

                i++;
            }

            if (j != IsuseCount)
            {
                Shove._Web.JavaScript.Alert(this.Page, "随机生成号码时出现异常！");

                return;
            }

            ChaseXML += "</ChaseDetails>";
        }
        else
        {
            if (string.IsNullOrEmpty(HidLotteryNumber.Value))
            {
                Shove._Web.JavaScript.Alert(this.Page, "发起追号套餐有异常！");

                return;
            }

            if (Multiple != Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("selectMultiple"), 0))
            {
                Shove._Web.JavaScript.Alert(this.Page, "投注倍数出现异常！");

                return;
            }

            SLS.Lottery slsLottery = new SLS.Lottery();
            string[] t_lotterys = SplitLotteryNumber(HidLotteryNumber.Value);

            if ((t_lotterys == null) || (t_lotterys.Length < 1))
            {
                Shove._Web.JavaScript.Alert(this.Page, "选号发生异常，请重新选择号码投注，谢谢。");

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

            if (ValidNum != Nums)
            {
                Shove._Web.JavaScript.Alert(this.Page, "选号发生异常，请重新选择号码投注，谢谢。");

                return;
            }
        }

        string Description = "";

        Result = _User.InitiateCustomChase(LotteryID,PlayTypeID,Price, Type, EndTime, IsuseCount, Multiple, Nums, BetType, HidLotteryNumber.Value, 1, 0, Money, Title, ChaseXML, ref Description);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, Description);

            return;
        }

        Shove._Web.Cache.ClearCache("LotteryPackage_BindUsers");
        Shove._Web.Cache.ClearCache(_Site.ID.ToString() + "AccountFreezeDetail_" + _User.ID.ToString());
        Shove._Web.Cache.ClearCache("Home_Room_ViewChaseCombo_BindData" + _User.ID.ToString());

        Shove._Web.JavaScript.Alert(this.Page, "定制追号套餐成功！", "LotteryPackage.aspx");
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
