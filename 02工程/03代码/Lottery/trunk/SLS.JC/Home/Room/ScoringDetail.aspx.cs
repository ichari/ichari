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

public partial class Home_Room_ScoringDetail : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDateTime();
            BindDataForYearMonth();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;
        isRequestLogin = true;

        base.OnInit(e);
    }

    #endregion

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnGO.Enabled = false;
            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;

        ddlMonth.SelectedIndex = Month - 1;
    }

    private void BindData()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Shove._Web.WebConfig.GetAppSettingsString("SystemPreFix") + _Site.ID.ToString() + "ScoringDetail_" + _User.ID.ToString() + "_DateTime" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString());

        if (dt == null)
        {
            if (ddlYear.Items.Count < 1)
            {
                return;
            }

            int ReturnValue = 0;
            string ReturnDescription = "";

            DataSet ds = null;

            DAL.Procedures.P_GetUserScoringDetail(ref ds, _Site.ID, _User.ID, int.Parse(ddlYear.SelectedValue), int.Parse(ddlMonth.SelectedValue), ref ReturnValue, ref ReturnDescription);

            if ((ds == null) || (ds.Tables.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Home_Room_ScoringDetail");

                return;
            }

            dt = ds.Tables[0];

            Shove._Web.Cache.SetCache(Shove._Web.WebConfig.GetAppSettingsString("SystemPreFix") + _Site.ID.ToString() + "ScoringDetail_" + _User.ID.ToString() + "_DateTime" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString(), dt);
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            long SchemeID = Shove._Convert.StrToLong(e.Item.Cells[5].Text, 0);

            if (SchemeID > 0)
            {
                e.Item.Cells[1].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'><font color=\"#330099\">" + e.Item.Cells[1].Text + "</Font></a>";
            }
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void btnGO_Click(object sender, System.EventArgs e)
    {
        Shove._Web.Cache.ClearCache("ScoringDetail_" + _User.ID.ToString());

        BindData();
    }

    protected void BindDateTime()
    {
        int Month = DateTime.Now.Month;

        for (int i = 1; i <= 12; i++)
        {
            if (i <= Month)
            {
                ListItem it = new ListItem(i + "月", i.ToString());
                ddlMonth.Items.Add(it);
            }
            else
            {
                break;
            }
        }        
    }
}
