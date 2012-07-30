using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Join_Project_List : System.Web.UI.Page
{
    public string PlayType = "";
    public string IsuseInfo = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("id"), 5);

            hidLotteryID.Value = LotteryID.ToString();
            
            BindPlay(LotteryID);
            BindData(LotteryID);
            // 更换发起合买Url
            SetSchemesUrl(LotteryID);
        }
    }

    private void BindPlay(int LotteryID)
    {
        string CacheKey = "Join_Project_List_dtVPlayTypes_" + LotteryID.ToString();
        DataTable dtPlayTypes = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtPlayTypes == null)
        {
            dtPlayTypes = new DAL.Tables.T_PlayTypes().Open("ID, Name", "LotteryID =" + LotteryID.ToString() + " AND ID in " + GetLotteryPlayType(LotteryID), "ID");

            if (dtPlayTypes == null)
            {
                PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);
                return;
            }
          //  Shove._Web.Cache.SetCache(CacheKey, dtPlayTypes, 6000);
        }

        PlayType = "";
        if (LotteryID == SLS.Lottery.CQSSC.ID)
            return;
        StringBuilder sb = new StringBuilder();

        for (int j = 0; j < dtPlayTypes.Rows.Count; j++)
        {
            sb.Append("<li mid=\""+dtPlayTypes.Rows[j]["ID"].ToString()+"\">"+dtPlayTypes.Rows[j]["Name"].ToString()+"</li>");
        }
        PlayType = sb.ToString().Replace("包点", "和值");
    }

    private string GetLotteryPlayType(int LotteryID)
    {
        string str = null;
        // 0 for no playtype selected
        switch (LotteryID)
        {
            case SLS.Lottery.SSQ.ID:
                str = "(501, 502)";
                break;
            case SLS.Lottery.FC3D.ID:
                str = "(601, 602, 606)";
                break;
            case SLS.Lottery.QLC.ID:
                str = "(1301, 1302)";
                break;
            case SLS.Lottery.CQSSC.ID:
                str = "(0)";
                break;
            default:
                str = "(0)";
                break;
        }
        return str;
    }

    private void BindData(int LotteryID)
    {
        DataTable dt = new DAL.Tables.T_Isuses().Open("top 1 *", "lotteryid=" + LotteryID.ToString() + " and getdate() between StartTime and EndTime", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        string IsuseName = dt.Rows[0]["Name"].ToString();

        int SystemEndAheadMinute = DataCache.LotteryEndAheadMinute[LotteryID];
        DateTime EndTime = Convert.ToDateTime(dt.Rows[0]["EndTime"]);
        DateTime SystemEndTime = EndTime.AddMinutes(SystemEndAheadMinute * -1);

        IsuseInfo = DataCache.Lotteries[LotteryID].ToString() + "合买方案</span><span class=\"fn\">第<strong>" + IsuseName + "</strong>期  截止时间：" + SystemEndTime.ToString("yyyy-MM-dd hh:mm:ss");

        if (LotteryID == 72 || LotteryID == 73)
        {
            IsuseInfo = "";
        }
    }

    private void SetSchemesUrl(int LotteryID)
    {
        // Add Url
        if (LotteryID == 72) {
            hlUrl.NavigateUrl ="/JCZC/Buy_SPF.aspx";            
        } else if (LotteryID == 73) {
            hlUrl.NavigateUrl = "/JCLC/Buy_SF.aspx";
        } else if (LotteryID == 74) {
            hlUrl.NavigateUrl = "/Lottery/Buy_SFC.aspx";
        } else if (LotteryID == 75) {
            hlUrl.NavigateUrl = "/Lottery/Buy_SFC_9_D.aspx";
        } else if (LotteryID == 3) {
            hlUrl.NavigateUrl = "/Lottery/Buy_QXC.aspx";
        } else if (LotteryID == 39) {
            hlUrl.NavigateUrl = "/Lottery/Buy_CJDLT.aspx";
        } else if (LotteryID == 63) {
            hlUrl.NavigateUrl = "/Lottery/Buy_PL3.aspx";
        } else if (LotteryID == 64) {
            hlUrl.NavigateUrl = "/Lottery/Buy_PL5.aspx";
        } else if (LotteryID == 2) {
            hlUrl.NavigateUrl = "/Lottery/Buy_JQC.aspx";
        } else if (LotteryID == 15) {
            hlUrl.NavigateUrl = "/Lottery/Buy_LCBQC.aspx";
        } else if (LotteryID == 5) {
            hlUrl.NavigateUrl = "/Lottery/Buy_SSQ.aspx";
        } else if (LotteryID == 6) {
            hlUrl.NavigateUrl = "/Lottery/Buy_3D.aspx";
        } else if (LotteryID == 13) {
            hlUrl.NavigateUrl = "/Lottery/Buy_QLC.aspx";
        } else {
            //defuatl Url
            hlUrl.NavigateUrl = "/JCZC/Buy_SPF.aspx";
        }
    }
}
