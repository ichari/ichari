using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Home_Room_MyPromotion_UserAccountDetail  : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            
            if (Request["StartTime"] != null && Request["EndTime"] != null)
            {
                tbBeginTime.Text = (Shove._Convert.StrToDateTime(Shove._Web.Utility.GetRequest("StartTime") + " 0:0:0", DateTime.Now.ToString())).ToString("yyyy-MM-dd");
                tbEndTime.Text = (Shove._Convert.StrToDateTime(Shove._Web.Utility.GetRequest("EndTime") + " 23:59:59", DateTime.Now.ToString())).ToString("yyyy-MM-dd");
            }
            else
            {
                tbBeginTime.Text =  DateTime.Now.ToString("yyyy-MM-dd");
                tbEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

            BindUserAccount();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindUserAccount()
    {
        DateTime fromDate = Shove._Convert.StrToDateTime(tbBeginTime.Text + " 0:0:0", DateTime.Now.ToString());
        DateTime toDate = Shove._Convert.StrToDateTime(tbEndTime.Text + " 23:59:59", DateTime.Now.ToString());
        long memberID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1);

        if (memberID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().BaseType.FullName);

            return;
        }

        string cacheKey = "Home_Room_MyPromotion_UserAccountDetail_" + memberID +"_"+ _User.ID + "_" + fromDate.ToString("yyyyMMdd") + toDate.ToString("yyyyMMdd");
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKey);
        if (dt == null)
        {

            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;
            int Result = DAL.Procedures.P_CpsGetCommendMemberBuyDetail(ref ds, _Site.ID, _User.ID, memberID, fromDate, toDate, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);

                return;
            }
            if ((ds == null) || (ds.Tables.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKey, dt);
        }

        PF.DataGridBindData(g, dt, gPager);
        gPager.Visible = g.PageCount > 1;
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindUserAccount();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindUserAccount();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!Shove._String.Valid.isDate(tbBeginTime.Text) || !Shove._String.Valid.isDate(tbEndTime.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输正确的查询日期格式.如 2009-01-01");
            return;
        }
        BindUserAccount();
    }
}
