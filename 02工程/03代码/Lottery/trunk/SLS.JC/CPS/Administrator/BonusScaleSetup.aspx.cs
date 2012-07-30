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

public partial class Cps_Administrator_BonusScaleSetup : AdminPageBase
{
    string cacheKeyScale = "Cps_Administrator_BonusScaleSetup_Scale";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
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
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKeyScale);
        
        if (dt == null )
        {
            dt = Shove.Database.MSSQL.Select("select LotteryID,LotteryName,UnionBonusScale,SiteBonusScale from T_CpsBonusScaleSetting order by TypeID");
            
            if (dt == null)
            {
                Shove._Web.JavaScript.Alert(this.Page, "获取数据出错!");

                return;
            }
            
            Shove._Web.Cache.SetCache(cacheKeyScale, dt, 100000);
        }

        PF.DataGridBindData(g, dt,gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView view = ((DataRowView)e.Item.DataItem);
            
            ((TextBox)e.Item.FindControl("tbUnionBonusScale")).Text = view.Row["UnionBonusScale"].ToString();
            ((TextBox)e.Item.FindControl("tbCommenderBonusScale")).Text = view.Row["SiteBonusScale"].ToString();
        }
    }

    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "modify")
        {
            int rowIndex = e.Item.ItemIndex;
            long lotteryID = Shove._Convert.StrToLong(g.DataKeys[rowIndex].ToString(), -1);

            Double UnionBonusScale = Shove._Convert.StrToDouble(((TextBox)g.Items[rowIndex].FindControl("tbUnionBonusScale")).Text, -1);
           
            Double SiteBonusScale = Shove._Convert.StrToDouble(((TextBox)g.Items[rowIndex].FindControl("tbCommenderBonusScale")).Text, -1);

            if (UnionBonusScale > 0.08 || SiteBonusScale > 0.08 || UnionBonusScale < 0 || SiteBonusScale < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "请正确输入返点比例!");

                return;
            }

            int returnValue = -1;
            string returnDescription = "";

            int Result = DAL.Procedures.P_CpsSetBonusScaleSetting(lotteryID, UnionBonusScale, SiteBonusScale, ref returnValue, ref returnDescription);

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

            Shove._Web.Cache.ClearCache(cacheKeyScale);
            BindData();
            Shove._Web.JavaScript.Alert(this.Page, "修改成功!");
        }
       
    }
}
