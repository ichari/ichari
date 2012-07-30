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
using System.Text;

public partial class Home_Web_Score : SitePageBase
{
    public int LotteryID = 72;
    private bool IsShow = false;
    public string dingZhi = "";
    public int Source = -1;
    protected void Page_Load(object sender, System.EventArgs e)
    {
        long UserID = -1;
        try
        {
            UserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);
            Source = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("Source"), -1);
        }
        catch { }

        if (UserID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }
        if (!IsPostBack)
        {
            try
            {
                LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), -1);
            }
            catch { }
        }
        else
        {
            ControlCollection ccl = rbList.Controls;
            for (int i = 0; i < ccl.Count; i++)
            {
                if (ccl[i].ID == null || ccl[i].ID == "")
                {
                    continue;
                }
                if (ccl[i].ID.IndexOf("rb") > -1)
                {
                    CheckBox cb = ccl[i] as CheckBox;
                    if (cb.Checked)
                    {
                        LotteryID = Shove._Convert.StrToInt(cb.ID.Substring(2), LotteryID);
                        break;
                    }
                }
            }
        }
        if (!new SLS.Lottery().ValidID(LotteryID))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误！(彩种)", this.GetType().FullName);

            return;
        }

        if (LotteryID == 72 || LotteryID == 73)
        {
            btn_Single.Visible = false;
            btn_All.Visible = false;
        }

        //判断是分页或者显示中奖方案回发时load事件中不绑定
        string post = Shove._Web.Utility.GetRequest("__EVENTTARGET");
        if (!(post.Equals("cbShowWin") || post.Equals("gPager")))
        {
            BindData(UserID, LotteryID);
        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    #endregion

    private void BindData(long UserID, int LotteryID)
    {
        string ReturnDescription = "";
        string IsShowWin = "";
        Users tu = new Users(_Site.ID);
        tu.ID = UserID;

        if (tu.GetUserInformationByID(ref ReturnDescription) != 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName);

            return;
        }

        labUserName.Text = tu.Name;
        labUserRegisterTime.Text = tu.RegisterTime.ToString("yyyy-MM-dd HH:mm:ss");
        labUserType.Text = "普通会员";
        dingZhi = "&FollowUserID=" + UserID + "&FollowUserName=" + HttpUtility.UrlEncode(tu.Name) + "\"";
        DataTable dt = null;

        if (Source == -1)
        {
            dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[id] = " + LotteryID.ToString(), "");
        }
        else if (Source == 1)
        {
            dt = Shove.Database.MSSQL.Select("Select ID,Name from  T_Lotteries where ID=" + LotteryID.ToString());
        }
        else if (Source == 2)
        {
            dt = Shove.Database.MSSQL.Select("Select ID,Name from  T_Lotteries where ID=" + LotteryID.ToString());
        }
        else 
        {
            dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[id] = " + LotteryID.ToString(), "");
        }

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }
        if (cbShowWin.Checked)
        {
            IsShowWin = " where WinMoney>0";
        }
        string CacheKey = tu.ID + "_" + LotteryID.ToString() + "_Home_Web_Score_"+cbShowWin.Checked;

        dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);
        if (dt == null || IsShow == true)
        {
            string sqlText = "select * from (";
            
            sqlText += GetSQL(UserID, LotteryID, "", ",source=1") + ") d  " + IsShowWin + "  order by TopMoney desc,id desc";
            dt = Shove.Database.MSSQL.Select(sqlText);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }
            Shove._Web.Cache.SetCache(CacheKey, dt, 300);
        }
        PF.DataGridBindData(g, dt, gPager);

        gPager.Visible = true;
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            int Source = Shove._Convert.StrToInt(e.Item.Cells[11].Text, 0);

            if (Source == 1)
            {
                e.Item.Cells[1].Text = "<a href='../Room/Scheme.aspx?id=" + e.Item.Cells[10].Text + "' target=_blank>" + e.Item.Cells[1].Text + "</a>";

                //if (Shove._Convert.StrToInt(e.Item.Cells[11].Text, 0) != 2)
                //{
                //    e.Item.Cells[1].Text = "<a href='../Room/Scheme.aspx?id=" + e.Item.Cells[10].Text + "' target=_blank>" + e.Item.Cells[1].Text + "</a>";
                //}
                //else
                //{
                //    e.Item.Cells[1].Text = "<a href='../Room/Scheme.aspx?Source=1&id=" + e.Item.Cells[10].Text + "' target=_blank>" + e.Item.Cells[1].Text + "</a>";
                //}
            }
            else if (Source== 2)
            {
                e.Item.Cells[1].Text = "<a href='../Room/Scheme.aspx?Source=1&id=" + e.Item.Cells[10].Text + "' target=_blank>" + e.Item.Cells[1].Text + "</a>";
            }
            else if (Source == 3)
            {
                e.Item.Cells[1].Text = "<a href='../Room/Scheme.aspx?Source=2&id=" + e.Item.Cells[10].Text + "' target=_blank>" + e.Item.Cells[1].Text + "</a>";
            }
            string str = e.Item.Cells[3].Text;
            if (str.Length > 25)
            {
                str = str.Substring(0, 23) + "..";
            }
            e.Item.Cells[3].Text = str;

            double money;
            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            e.Item.Cells[5].Text = (money == 0) ? "" : money.ToString("N");

            short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[7].Text, 0);
            bool Buyed = Shove._Convert.StrToBool(e.Item.Cells[8].Text, false);
            int Schedule = Shove._Convert.StrToInt(e.Item.Cells[9].Text, 0);

            if (Buyed)
            {
                e.Item.Cells[6].Text = "<font color='red'>已成功</font>";
            }
            else
            {
                if (QuashStatus != 0)
                {
                    e.Item.Cells[6].Text = "未成功";
                }
                else
                {
                    if (Schedule < 100)
                    {
                        e.Item.Cells[6].Text = "未成功";
                    }
                    else
                    {
                        e.Item.Cells[6].Text = "<font color='blue'>未录入</font>";
                    }
                }
            }
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        long UserID = -1;

        try
        {
            UserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);
        }
        catch { }

        if (UserID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }
        if (!new SLS.Lottery().ValidID(LotteryID))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误！(彩种)", this.GetType().FullName);

            return;
        }

        BindData(UserID, LotteryID);
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        long UserID = -1;

        try
        {
            UserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);
        }
        catch { }

        if (UserID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }
        if (!new SLS.Lottery().ValidID(LotteryID))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误！(彩种)", this.GetType().FullName);

            return;
        }

        BindData(UserID, LotteryID);
    }
    protected void cbShowWin_CheckedChanged(object sender, EventArgs e)
    {
        IsShow = true;
        BindData(Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1), LotteryID);
    }
  
    private void ResponseTailor(string headMethod)
    {
        long UserID = -1;

        try
        {
            UserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);
        }
        catch { }

        if (UserID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }
        if (!new SLS.Lottery().ValidID(LotteryID))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误！(彩种)", this.GetType().FullName);

            return;
        }
        Users tu = new Users(_Site.ID);
        tu.ID = UserID;

        string ReturnDescription = "";
        if (tu.GetUserInformationByID(ref ReturnDescription) != 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName);

            return;
        }
        dingZhi = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), headMethod + dingZhi);
        if (LotteryID == 74)
        {
            Response.Redirect("../../Lottery/Buy_SFC.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 75)
        {
            Response.Redirect("../../Lottery/Buy_SFC_9_D.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 2)
        {
            Response.Redirect("../../Lottery/Buy_JQC.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 15)
        {
            Response.Redirect("../../Lottery/Buy_LCBQC.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 39)
        {
            Response.Redirect("../../Lottery/Buy_CJDLT.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 3)
        {
            Response.Redirect("../../Lottery/Buy_QXC.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 9)
        {
            Response.Redirect("../../Lottery/Buy_22X5.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 63)
        {
            Response.Redirect("../../Lottery/Buy_PL3.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 64)
        {
            Response.Redirect("../../Lottery/Buy_PL5.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 5)
        {
            Response.Redirect("../../Lottery/Buy_SSQ.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 6)
        {
            Response.Redirect("../../Lottery/Buy_3D.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 13)
        {
            Response.Redirect("../../Lottery/Buy_QLC.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 61)
        {
            Response.Redirect("../../Lottery/Buy_SSC.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 62)
        {
            Response.Redirect("../../Lottery/Buy_SYYDJ.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 70)
        {
            Response.Redirect("../../Lottery/Buy_JX11X5.aspx?DZ=" + dingZhi);
        }
        else if (LotteryID == 29)
        {
            Response.Redirect("../../Lottery/Buy_SSL.aspx?DZ=" + dingZhi);
        }
    }
    private string GetSQL(long userID, int lotteryID, string dataBaseName, string source)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("(select  WinMoneyNoWithTax as TopMoney,")
                        .AppendLine("IsuseName, SchemeNumber, PlayTypeName, Title, [Money],WinMoney, QuashStatus, Buyed, Schedule, id" + source)
                        .AppendLine(" from " + dataBaseName + "V_Schemes where InitiateUserID = " + userID.ToString() + " and LotteryID = " + lotteryID.ToString() + ")");
        return sb.ToString();
    }

    protected void btn_Single_Click(object sender, ImageClickEventArgs e)
    {
        ResponseTailor("followScheme(" + LotteryID + ");$Id(\"iframeFollowScheme\").src=\"../Home/Room/FollowFriendSchemeAdd.aspx?LotteryID=" + LotteryID + "&DzLotteryID=" + LotteryID);
    }
    protected void btn_All_Click(object sender, ImageClickEventArgs e)
    {
        ResponseTailor("followScheme(-1);$Id(\"iframeFollowScheme\").src=\"../Home/Room/FollowFriendSchemeAdd.aspx?LotteryID=-1&FollowFriendID=-1");
    }
}