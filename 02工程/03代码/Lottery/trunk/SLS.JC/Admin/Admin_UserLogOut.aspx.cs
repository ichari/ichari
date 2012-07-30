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

public partial class Admin_Admin_UserLogOut : AdminPageBase
{
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
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.MemberManagement, Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_Users().Open("ID,SiteID ,Name ,RealityName, IDCardNumber ,Email ,QQ,Telephone , Mobile ,isCanLogin ,Reason", "SiteID = " + _Site.ID.ToString() + "and isCanLogin= 0", "");
        
        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Admin_UserLogOut");

            return;
        }

        DataTable dtData = dt.Clone();
        DataRow[] drs;

        if (rbUser.Checked)
        {
            drs = dt.Select("Reason is not null");
        }
        else
        {
            drs = dt.Select("Reason is null");
        }

        foreach (DataRow dr in drs)
        {
            dtData.Rows.Add(dr.ItemArray);
        }

        PF.DataGridBindData(g, dtData, gPager);

    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {

            CheckBox cb = (CheckBox)e.Item.Cells[7].FindControl("cbisCanLogin");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[10].Text, true);

            string str = e.Item.Cells[8].Text;
            if (str.Length > 20)
            {
                e.Item.Cells[8].Text = str.Substring(0, 20) + "……";
            }

            e.Item.Cells[8].Attributes.Add("title", str);
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

    protected void rbAdmin_CheckedChanged(object sender, EventArgs e)
    {
        tdDscrition.InnerHtml = "管理员限制详情";
        BindData();
    }

    protected void rbUser_CheckedChanged(object sender, EventArgs e)
    {
        tdDscrition.InnerHtml = "用户注销详情";
        BindData();
    }
}
