using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Admin_CommendPromoterBonusScaleEidt : CpsPageBase
{
    string cacheKeyScaleList = "CPS_Admin_CommendPromoterBonusScaleEidt_ScaleList_";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            long Cpsid = Shove._Convert.StrToLong(Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("ID")), -1);

            Cps curCps = new Cps();
            curCps.SiteID = _Site.ID;
            curCps.ID = Cpsid;

            string returndescription = string.Empty;
            curCps.GetCpsInformationByID(ref returndescription);

            if (_User.cps.ID < 0 || curCps.ID < 0 || _User.cps.ID != curCps.ParentID)
            {
                PF.GoError(ErrorNumber.NotEnoughCompetence, "当前推广员不是你发展的下线,没有权限查看.", this.GetType().BaseType.FullName);

                return;
            }

            hfCpsID.Value = Cpsid.ToString();
            hfOwnerUserID.Value = curCps.OwnerUserID.ToString();
            lblCpsName.Text = curCps.Name;

            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Admin/Default.aspx?SubPage=CommendPromoterBonusScaleEidt.aspx";

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        long ownerUserID = Shove._Convert.StrToLong(hfOwnerUserID.Value, -1);

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKeyScaleList + ownerUserID);
        if (dt == null)
        {
            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;

            int Result = DAL.Procedures.P_CpsGetUserBonusScaleList(ref ds, ownerUserID, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "系统出错!");

                return;

            }

            if (ds == null || ds.Tables.Count < 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "获取数据出错!");

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKeyScaleList + ownerUserID, dt, 100000);
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView view = ((DataRowView)e.Item.DataItem);
            ((TextBox)e.Item.FindControl("tbBonusScale")).Text = view.Row["BonusScale"].ToString();

        }
    }

    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        long cpsID = Shove._Convert.StrToLong(hfCpsID.Value, -1);
        long ownerUserID = Shove._Convert.StrToLong(hfOwnerUserID.Value, -1);

        if (e.CommandName == "modify")
        {
            int rowIndex = e.Item.ItemIndex;
            long lotteryID = Shove._Convert.StrToLong(g.DataKeys[rowIndex].ToString(), -1);

            Double bonusScale = Shove._Convert.StrToDouble(((TextBox)g.Items[rowIndex].FindControl("tbBonusScale")).Text, -1);

            if (bonusScale > 0.08 || bonusScale < 0.0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "输入返点比例不能小于0或超过0.08,请正确输入!");

                return;
            }

            string errorMsg = "";
            if (!CheckBonusValue(bonusScale, lotteryID, ref  errorMsg))
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错:" + errorMsg);

                return;
            }

            int returnValue = -1;
            string returnDescription = "";

            int Result = DAL.Procedures.P_CpsSetUserBonusScale(ownerUserID, lotteryID, bonusScale, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "数据访问出错!");

                return;
            }

            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错:" + returnDescription);

                return;
            }

            Shove._Web.Cache.ClearCache(cacheKeyScaleList + ownerUserID);
            BindData();
            Shove._Web.JavaScript.Alert(this.Page, "修改成功!");
        }

    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void btnModifyAll_Click(object sender, EventArgs e)
    {
        long ownerUserID = Shove._Convert.StrToLong(hfOwnerUserID.Value, -1);

        Double bonusScale = Shove._Convert.StrToDouble(tbAllScale.Text, -1);

        if (bonusScale > 0.08 || bonusScale < 0.0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入返点比例小于0或过大,请正确输入!");

            return;
        }

        string errorMsg = "";
        if (!CheckBonusValue(bonusScale, 0, ref  errorMsg))
        {
            Shove._Web.JavaScript.Alert(this.Page, "出错:" + errorMsg);
            return;
        }


        int returnValue = -1;
        string returnDescription = "";

        int Result = DAL.Procedures.P_CpsSetUserBonusScale(ownerUserID, 0, bonusScale, ref returnValue, ref returnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "数据访问出错!");

            return;
        }

        if (returnValue < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "出错:" + returnDescription);

            return;
        }

        Shove._Web.Cache.ClearCache(cacheKeyScaleList + ownerUserID);
        BindData();
        Shove._Web.JavaScript.Alert(this.Page, "修改成功!");
    }

    private bool CheckBonusValue(double bonusScale, long lotteryID, ref string errorMsg)
    {
        int returnValue = -1;
        string returnDescription = "";
        DataSet ds = null;
        DataTable dt = null;
        //获取当前商家本身的返点列表
        int Result = DAL.Procedures.P_CpsGetUserBonusScaleList(ref ds, _User.ID, ref returnValue, ref returnDescription);

        if (Result < 0)
        {
            errorMsg = "系统出错!";
            return false;

        }
        if (ds == null || ds.Tables.Count < 1)
        {
            errorMsg = "获取数据出错!";
            return false;
        }

        dt = ds.Tables[0];

        if (lotteryID > 0)//指定某一个彩种时
        {
            DataRow[] rows = dt.Select("LotteryID=" + lotteryID);
            if (rows.Length > 0)
            {
                if (bonusScale >= Shove._Convert.StrToDouble(rows[0]["BonusScale"].ToString(), 0))
                {
                    errorMsg = rows[0]["LotteryName"].ToString() + "设置的返点比例大于自身的返点比例" + rows[0]["BonusScale"].ToString();
                    return false;
                }
            }

        }
        else//所有彩种
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (bonusScale >= Shove._Convert.StrToDouble(dt.Rows[i]["BonusScale"].ToString(), 0))
                {
                    errorMsg = dt.Rows[i]["LotteryName"].ToString() + "设置的返点比例大于自身的返点比例" + dt.Rows[i]["BonusScale"].ToString();
                    return false;
                }
            }
        }
        return true;
    }
}
