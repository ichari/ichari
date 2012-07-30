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

using Shove.Database;

public partial class Home_Room_FollowFriendView : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            lbUserName.Text = System.Web.HttpUtility.UrlDecode(Shove._Web.Utility.GetRequest("FollowUserName"));
            int ID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("ID"), -1);

            if (ID < 0 || lbUserName.Text == "")
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误，系统异常。", this.GetType().BaseType.FullName);

                return;
            }

            HidID.Value = ID.ToString();

            BindData();
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

    private void BindData()
    {
        string strCmd = @"select a.*,b.Name from T_CustomFriendFollowSchemes a join T_Users b on a.UserID = b.ID where a.FollowUserID = " + HidID.Value;
        DataTable dt = MSSQL.Select(strCmd);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.Unknow, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        lbFollowCount.Text = dt.Rows.Count.ToString();

        PF.DataGridBindData(g, dt, gPager);

        gPager.Visible = true;
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataRow dr = drv.Row;

            //定制时间
            e.Item.Cells[0].Text = Shove._Convert.StrToDateTime(dr["DateTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            //END

            //彩种
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

            if (dr["LotteryID"].ToString()=="-1")
            {
                e.Item.Cells[2].Text = "全部彩种";
            }
            else
            {
                e.Item.Cells[2].Text = dtLotteries.Select("ID = " + dr["LotteryID"].ToString())[0]["Name"].ToString();
            }
            //END

            //玩法
            //玩法信息缓存 6000 秒
            CacheKey = "dtPlayTypes";
            DataTable dtPlayTypes = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

            if (dtPlayTypes == null)
            {
                dtPlayTypes = new DAL.Tables.T_PlayTypes().Open("", "LotteryID in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[ID]");

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

            if (dr["PlayTypeID"].ToString() == "-1")
            {
                e.Item.Cells[3].Text = "全部玩法";
            }
            else
            {
                e.Item.Cells[3].Text = dtPlayTypes.Select("ID = " + dr["PlayTypeID"].ToString())[0]["Name"].ToString();
            }
            //END

            //认购金额
            e.Item.Cells[4].Text = Shove._Convert.StrToDouble(dr["MoneyStart"].ToString(), 0).ToString("N") + "&nbsp;至&nbsp;" + Shove._Convert.StrToDouble(dr["MoneyEnd"].ToString(), 0).ToString("N") + " 元";
            //END

            e.Item.Cells[5].Text = dr["Type"].ToString() == "1" ? "用户定制" : "发起人指定";
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

}
