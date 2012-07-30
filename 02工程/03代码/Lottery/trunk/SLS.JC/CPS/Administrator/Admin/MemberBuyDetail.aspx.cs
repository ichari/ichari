using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;

public partial class CPS_Administrator_Admin_MemberBuyDetail : CpsAdminPageBase
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
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Administrator/Admin/Default.aspx";
        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion


    private void BindData()
    {
        //处理日期
        DateTime StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//默认为本月1日
        DateTime EndTime = DateTime.Now;
        if (Request["StartTime"] != null && Request["EndTime"] != null)
        {
            StartTime = Shove._Convert.StrToDateTime(Shove._Web.Utility.GetRequest("StartTime") + " 0:0:0", DateTime.Now.ToString());
            EndTime = Shove._Convert.StrToDateTime(Shove._Web.Utility.GetRequest("EndTime") + " 23:59:59", DateTime.Now.ToString());
        }
        
        //查询查询的会员ID
        long UserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1);
        if (UserID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().BaseType.FullName);

            return;
        }

        //获取数据
        string Key = "Cps_Admin_MemberBuyDetail_" + UserID + StartTime.ToString() + EndTime.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);
       
        if (dt == null)
        {
            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;

            int Result = DAL.Procedures.P_GetCpsMemberBuyDetail(ref ds, _Site.ID, _User.cps.ID, UserID, StartTime, EndTime, ref returnValue, ref returnDescription);
            
            if ((ds == null) || (ds.Tables.Count < 1) || Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(Key, dt, 100000);

        }

        PF.DataGridBindData(g, dt, gPager);
        gPager.Visible = g.PageCount > 1;
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
