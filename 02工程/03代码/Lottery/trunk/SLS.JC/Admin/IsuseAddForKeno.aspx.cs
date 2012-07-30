using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Shove.Database;

public partial class Admin_IsuseAddForKeno : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), -1);

            if (!PF.ValidLotteryID(_Site, LotteryID))
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_IsuseAddForKeno");

                return;
            }

            tbLotteryID.Text = LotteryID.ToString();

            string IntervalType = DAL.Functions.F_GetLotteryIntervalType(LotteryID);

            if (!IntervalType.StartsWith("分"))
            {
                this.Response.Redirect("IsuseAdd.aspx?LotteryID=" + LotteryID.ToString(), true);

                return;
            }

            object oLastDate = MSSQL.ExecuteScalar("SELECT ISNULL(MAX(case LotteryID when 28 then DATEADD([day], - 1, EndTime) else EndTime end), DATEADD([day], - 1, GETDATE())) AS LastDate from T_Isuses where LotteryID = " + LotteryID.ToString());

            if (oLastDate == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                return;
            }

            DateTime dtLastDate = DateTime.Parse(oLastDate.ToString()).AddDays(1);
            tbDate.Text = dtLastDate.Year.ToString() + "-" + dtLastDate.Month.ToString() + "-" + dtLastDate.Day.ToString();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme);

        base.OnInit(e);
    }

    #endregion

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        DateTime StartDate;

        try
        {
            StartDate = DateTime.Parse(tbDate.Text);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "开始日期输入错误。");

            return;
        }

        int Days = Shove._Convert.StrToInt(tbDays.Text, 0);

        if (Days < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入要连续增加的天数。");

            return;
        }

        if (Days > 10)
        {
            Shove._Web.JavaScript.Alert(this.Page, "高频彩种一次最多只能增加10天。");

            return;
        }

        int LotteryID = int.Parse(tbLotteryID.Text);

        if (LotteryID == SLS.Lottery.CQSSC.ID)
        {
            string IntervalType = DAL.Functions.F_GetLotteryIntervalType(LotteryID);
            int Interval = int.Parse(IntervalType.Substring(1, IntervalType.IndexOf(";") - 1));
            string FirstEndTime = IntervalType.Substring(IntervalType.IndexOf(";") + 1, 8);
            int Degree = int.Parse(IntervalType.Substring(IntervalType.LastIndexOf(";") + 1));

            for (int i = 0; i < Days; i++)
            {
                string sDate = StartDate.Date.ToShortDateString();
                DataTable dt = dt = new DAL.Tables.T_Isuses().Open("[ID]", "StartTime between '" + sDate + " 02:00:00' and '" + sDate + " 23:59:59' and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(tbLotteryID.Text), "");

                if (dt == null)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    StartDate = StartDate.AddDays(1);

                    continue;
                }

                DateTime IsuseStartTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " 0:0:0");
                DateTime IsuseEndTime = IsuseStartTime.AddMinutes(5);
                string Isuse = StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + "-001";

                long NewIsuseID = 0;
                string ReturnDescription = "";

                int Result = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, "", ref NewIsuseID, ref ReturnDescription);

                if (Result < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (NewIsuseID < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_IsuseAddForKeno");

                    return;
                }

                //凌晨 0:05分到 01:55
                for (int j = 2; j <= 23; j++)
                {
                    IsuseStartTime = IsuseEndTime;
                    IsuseEndTime = IsuseEndTime.AddMinutes(5);
                    Isuse = Isuse = StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + "-" + j.ToString().PadLeft(3, '0');
                    if (DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, "", ref NewIsuseID, ref ReturnDescription) < 0)
                    {
                        break;
                    }
                }

                //凌晨 01:55 到 晚上 22:00
                IsuseStartTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " 01:55:00");
                IsuseEndTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " " + FirstEndTime);
                Isuse = StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + "-024";
                Result = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, "", ref NewIsuseID, ref ReturnDescription);

                if (Result < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                for (int j = 25; j <= 96; j++)
                {
                    IsuseStartTime = IsuseEndTime;
                    IsuseEndTime = IsuseEndTime.AddMinutes(Interval);
                    Isuse = Isuse = StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + "-" + j.ToString().PadLeft(3, '0');

                    if (DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, "", ref NewIsuseID, ref ReturnDescription) < 0)
                    {
                        break;
                    }
                }

                //22:00:00 点到 00:00:00

                //重庆时时彩 夜场
                if (LotteryID == SLS.Lottery.CQSSC.ID)
                {
                    for (int j = 97; j <= 120; j++)
                    {
                        IsuseStartTime = IsuseEndTime;
                        IsuseEndTime = IsuseEndTime.AddMinutes(5);
                        Isuse = ConvertIsuseName(LotteryID, StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + "-" + j.ToString().PadLeft(3, '0'));
                        if (DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, "", ref NewIsuseID, ref ReturnDescription) < 0)
                        {
                            break;
                        }
                    }
                }

                StartDate = StartDate.AddDays(1);
            }
        }
        else
        {
            string IntervalType = DAL.Functions.F_GetLotteryIntervalType(LotteryID);
            int Interval = int.Parse(IntervalType.Substring(1, IntervalType.IndexOf(";") - 1));
            string FirstEndTime = IntervalType.Substring(IntervalType.IndexOf(";") + 1, 8);
            int Degree = int.Parse(IntervalType.Substring(IntervalType.LastIndexOf(";") + 1));

            if (FirstEndTime.EndsWith(";"))
            {
                FirstEndTime = FirstEndTime.Substring(0, FirstEndTime.Length - 1);
            }

            for (int i = 0; i < Days; i++)
            {
                string sDate = StartDate.Date.ToShortDateString();
                DataTable dt = new DAL.Tables.T_Isuses().Open("[ID]", "StartTime between '" + sDate + " 0:0:0' and '" + sDate + " 23:59:59' and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(tbLotteryID.Text), "");

                if (dt == null)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    StartDate = StartDate.AddDays(1);

                    continue;
                }

                DateTime IsuseStartTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " 0:0:0");
                DateTime IsuseEndTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " " + FirstEndTime);
                string Isuse = ConvertIsuseName(LotteryID, StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + "01");

                long NewIsuseID = -1;
                string ReturnDescription = "";

                int Result = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, "", ref NewIsuseID, ref ReturnDescription);

                if (Result < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (NewIsuseID < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_IsuseAddForKeno");

                    return;
                }

                for (int j = 2; j <= Degree; j++)
                {
                    IsuseStartTime = IsuseEndTime;
                    IsuseEndTime = IsuseEndTime.AddMinutes(Interval);
                    Isuse = ConvertIsuseName(LotteryID, StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + j.ToString().PadLeft(2, '0'));
                    int Results = -1;
                    Results = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, "", ref NewIsuseID, ref ReturnDescription);
                    if (Results < 0)
                    {
                        break;
                    }
                }

                StartDate = StartDate.AddDays(1);
            }
        }

        this.Response.Redirect("Isuse.aspx?LotteryID=" + tbLotteryID.Text, true);
    }

    protected void btnBack_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("Isuse.aspx?LotteryID=" + tbLotteryID.Text, true);
    }

    private string ConvertIsuseName(int LotteryID, string IsuseName)
    {
        switch (LotteryID)
        {
            case SLS.Lottery.HBKLPK.ID:
                return PF.ConvertIsuseName_HBKLPK(IsuseName);
            case SLS.Lottery.SDKLPK.ID:
                return PF.ConvertIsuseName_SDKLPK(IsuseName);
            case SLS.Lottery.HeBKLPK.ID:
                return PF.ConvertIsuseName_HeBKLPK(IsuseName);
            case SLS.Lottery.AHKLPK.ID:
                return PF.ConvertIsuseName_AHKLPK(IsuseName);
            case SLS.Lottery.HLJKLPK.ID:
                return PF.ConvertIsuseName_HLJKLPK(IsuseName);
            case SLS.Lottery.LLKLPK.ID:
                return PF.ConvertIsuseName_LLKLPK(IsuseName);
            case SLS.Lottery.SXKLPK.ID:
                return PF.ConvertIsuseName_SXKLPK(IsuseName);
            case SLS.Lottery.ZJKLPK.ID:
                return PF.ConvertIsuseName_ZJKLPK(IsuseName);
            case SLS.Lottery.SCKLPK.ID:
                return PF.ConvertIsuseName_SCKLPK(IsuseName);
            case SLS.Lottery.ShXKLPK.ID:
                return PF.ConvertIsuseName_ShXKLPK(IsuseName);
            case SLS.Lottery.JXSSC.ID:
                return PF.ConvertIsuseName_JxSSC(IsuseName);
            case SLS.Lottery.SYYDJ.ID:
                return PF.ConvertIsuseName_SYYDJ(IsuseName);
            case SLS.Lottery.JX11X5.ID:
                return PF.ConvertIsuseName_SYYDJ(IsuseName);
            case SLS.Lottery.GD11X5.ID:
                return PF.ConvertIsuseName_SYYDJ(IsuseName);
            default:
                return IsuseName;
        }
    }
}
