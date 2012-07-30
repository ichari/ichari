using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Lottery_DemoTest : System.Web.UI.Page
{

    public int LotteryID=5;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
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
}