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

public partial class Home_Room_FollowFriendSchemeAdd_User : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            lbUserName.Text = System.Web.HttpUtility.UrlDecode(Shove._Web.Utility.GetRequest("FollowUserName"));
            long FollowUserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("FollowUserID"), -1);
            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), -1);
            if (!DataCache.Lotteries.ContainsKey(LotteryID))
            {
                LotteryID = 5;
            }

            if (FollowUserID < 0 || lbUserName.Text == "")
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误，系统异常。", this.GetType().BaseType.FullName);

                return;
            }

            HidFollowUserID.Value = FollowUserID.ToString();

            BindDataForLottery(LotteryID);
            //ddlLotteries.SelectedValue = "" + -1;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery(int LotteryID)
    {
        string CacheKey = "dtLotteriesUseLotteryList";
        DataTable dtLotteries = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtLotteries == null)
        {
            dtLotteries = new DAL.Tables.T_Lotteries().Open("[ID], [Name], [Code]", "[ID] in(" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[ID]");

            if (dtLotteries == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-39)");

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtLotteries, 6000);
        }

        if (dtLotteries == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-49)");

            return;
        }

        ddlLotteries.Items.Clear();
        ddlLotteries.Items.Add(new ListItem("全部彩种", "-1"));

        foreach (DataRow dr in dtLotteries.Rows)
        {
            string strLotteryName = dr["Name"].ToString();
            ddlLotteries.Items.Add(new ListItem(strLotteryName , dr["ID"].ToString()));
        }

        if (ddlLotteries.Items.FindByValue(LotteryID.ToString()) != null)
        {
            for (int i = 0; i < ddlLotteries.Items.Count; i++)
            {
                if (ddlLotteries.Items[i].Value == LotteryID.ToString())
                {
                    ddlLotteries.SelectedIndex = i;
                }
            }
        }

        ddlLotteries_SelectedIndexChanged(ddlLotteries, new EventArgs());
    }

    protected void ddlLotteries_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLotteries.SelectedValue == "-1")
        {
            ddlPlayTypes.Items.Clear();
            ddlPlayTypes.Items.Add(new ListItem("全部玩法", "-1"));

            return;
        }

        //玩法信息缓存 6000 秒
        string CacheKey = "dtPlayTypes_" + ddlLotteries.SelectedValue;
        DataTable dtPlayTypes = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtPlayTypes == null)
        {
            dtPlayTypes = new DAL.Tables.T_PlayTypes().Open("", "LotteryID in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ") and LotteryID = " + Shove._Convert.StrToInt(ddlLotteries.SelectedValue, -1).ToString(), "[ID]");

            if (dtPlayTypes == null || dtPlayTypes.Rows.Count < 1)
            {
                PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName + "(-85)");

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtPlayTypes, 6000);
        }

        if (dtPlayTypes == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-95)");

            return;
        }

        ddlPlayTypes.Items.Clear();
        ddlPlayTypes.Items.Add(new ListItem("全部玩法", "-1"));

        foreach (DataRow dr in dtPlayTypes.Rows)
        {
            ddlPlayTypes.Items.Add(new ListItem(dr["Name"].ToString(), dr["ID"].ToString()));
        }
    }

    protected void btn_OK_Click(object sender, EventArgs e)
    {
        double MoneyStart = 0;
        double MoneyEnd = 0;
        int BuyShareStart = 1;
        int BuyShareEnd = 1;

        //if (!_User.isAlipayNameValided)
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "对不起，您的支付宝账号还未绑定，请先绑定您的支付宝账号。谢谢！", "BindAlipay.aspx");

        //    return;
        //}

        if (string.IsNullOrEmpty(_User.IDCardNumber) || string.IsNullOrEmpty(_User.RealityName))
        {
            Shove._Web.JavaScript.Alert(this.Page, "对不起，您的身份证号码或者真实姓名没有填写，为了不影响您的领奖，请先完善这些资料。谢谢！", "UserEdit.aspx");

            return;
        }

        try
        {
            MoneyEnd = double.Parse(tbMaxMoney.Text);
            MoneyStart = double.Parse(tbMinMoney.Text);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有误");

            return;
        }

        if (MoneyEnd < MoneyStart)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入有误，最低金额不能大于最高金额");

            return;
        }

        BuyShareStart = Shove._Convert.StrToInt(tbBuyShareStart.Text, -1);
        BuyShareEnd = Shove._Convert.StrToInt(tbBuyShareEnd.Text, -1);

        if (BuyShareStart < 1 || BuyShareEnd < 1 || BuyShareStart > BuyShareEnd)
        {
            Shove._Web.JavaScript.Alert(this.Page, "份数输入有误");

            return;
        }

        if (_User.ID == Shove._Convert.StrToLong(HidFollowUserID.Value, 0))
        {
            Shove._Web.JavaScript.Alert(this.Page,"您不能定制自己的跟单!");

            return;
        }
        DAL.Tables.T_CustomFriendFollowSchemes tcffs = new DAL.Tables.T_CustomFriendFollowSchemes();

        tcffs.SiteID.Value = 1;
        tcffs.UserID.Value = _User.ID;
        tcffs.FollowUserID.Value = HidFollowUserID.Value;
        tcffs.LotteryID.Value = ddlLotteries.SelectedValue;
        tcffs.PlayTypeID.Value = ddlPlayTypes.SelectedValue;
        tcffs.MoneyStart.Value = MoneyStart;
        tcffs.MoneyEnd.Value = MoneyEnd;
        tcffs.BuyShareStart.Value = BuyShareStart;
        tcffs.BuyShareEnd.Value = BuyShareEnd;
        tcffs.Type.Value = 1;

        long l = tcffs.Insert();

        if (l > 0)
        {
            //刷新缓存
            string CacheKey = "T_CustomFriendFollowSchemes" + ddlLotteries.SelectedValue;
            Shove._Web.Cache.ClearCache(CacheKey);

            if (ddlLotteries.SelectedValue == "-1")
            {
                foreach (KeyValuePair<int, string> kvp in DataCache.Lotteries)
                {
                    CacheKey = "T_CustomFriendFollowSchemes" + kvp.Key.ToString();

                    Shove._Web.Cache.ClearCache(CacheKey);
                }
            }

            Response.Redirect("FollowSchemeHistory.aspx?Type='divSetMyFollowSchemes", true);

            return;
        }
        else
        {
            PF.GoError(ErrorNumber.Unknow, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("FollowSchemeHistory.aspx?Type='divSetMyFollowSchemes'", true);
    }
}
