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

public partial class Cps_Administrator_BonusScaleSetupForCps : AdminPageBase
{
    string cacheKeyScaleList = "Cps_Administrator_BonusScaleSetup_T_ScaleList_";
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
            hfCpsID.Value = Cpsid.ToString();
            hfOwnerUserID.Value = curCps.OwnerUserID.ToString();
            lblCpsName.Text = curCps.Name;

            linkReturn.NavigateUrl = "BaseInfo.aspx?ID=" + Cpsid;
           
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        long ownerUserID = Shove._Convert.StrToLong(hfOwnerUserID.Value, -1);

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKeyScaleList + ownerUserID);

        if (dt == null )
        {
            int returnValue=-1;
            string returnDescription="";
            DataSet ds = null;

            int Result = DAL.Procedures.P_CpsGetUserBonusScaleList(ref ds,ownerUserID, ref returnValue, ref returnDescription);
 
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
        long ownerUserID = Shove._Convert.StrToLong(hfOwnerUserID.Value, -1);
        
        if (e.CommandName == "modify")
        {
            int rowIndex = e.Item.ItemIndex;
            long lotteryID = Shove._Convert.StrToLong(g.DataKeys[rowIndex].ToString(), -1);
            
            Double bonusScale = Shove._Convert.StrToDouble(((TextBox)g.Items[rowIndex].FindControl("tbBonusScale")).Text, -1);

            if (bonusScale > 0.1 || bonusScale < 0.0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "请正确输入返点比例!");

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

    //统一修改所有彩种的比例
    protected void btnModifyAll_Click(object sender, EventArgs e)
    {
        long ownerUserID = Shove._Convert.StrToLong(hfOwnerUserID.Value, -1);
       
        Double bonusScale = Shove._Convert.StrToDouble(tbAllScale.Text, -1);

        if (bonusScale > 0.08 || bonusScale < 0.0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请正确输入返点比例!");
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
}
